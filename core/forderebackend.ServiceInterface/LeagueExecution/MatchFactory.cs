using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceModel.Dtos;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.LeagueExecution
{
    public static class MatchFactory
    {
        public static IEnumerable<Match> CreateLeagueMatches(IEnumerable<Team> teams, Func<int, LeagueMatchCreationMode> getMatchCreationMode)
        {
            var generatedMatches = new List<Match>();

            var teamsGroupedByLeague = teams.GroupBy(g => g.LeagueId).ToDictionary(k => k.Key);

            foreach (var key in teamsGroupedByLeague.Keys)
            {
                CreateLeagueMatches(teamsGroupedByLeague[key].ToList(), generatedMatches, key.Value, getMatchCreationMode);
            }

            return generatedMatches;
        }

        public static IEnumerable<Match> CreateMatchesForMovedTeam(Team movedTeam, List<Team> existingTeamsInLeague, League league)
        {
            switch (league.LeagueMatchCreationMode)
            {
                case LeagueMatchCreationMode.Single:
                    return new SingleRoundMatchCreator().CreateMatches(existingTeamsInLeague, movedTeam);
                case LeagueMatchCreationMode.Double:
                    return new DoubleRoundMatchCreator().CreateMatches(existingTeamsInLeague, movedTeam);
                default:
                    throw new Exception("Unknown mode in team move...");
            }
        }
       
        private static void CreateLeagueMatches(IList<Team> teams, ICollection<Match> listOfMatches, int leagueId, Func<int, LeagueMatchCreationMode> getMatchCreationMode)
        {
            teams.Shuffle();

            var mode = getMatchCreationMode(leagueId);

            List<Match> generateMatches;
            switch (mode)
            {
                case LeagueMatchCreationMode.Single:
                    generateMatches = new SingleRoundMatchCreator().CreateMatches(teams);
                    break;
                case LeagueMatchCreationMode.Double:
                    generateMatches = new DoubleRoundMatchCreator().CreateMatches(teams);
                    break;
                default:
                    throw new Exception("Unknown match creation mode");
            }

            foreach (var match in generateMatches)
            {
                match.LeagueId = leagueId;
                listOfMatches.Add(match);
            }
        }

        private static void SetFreeTicketResult(Match match)
        {
            if (match.GuestTeam.IsFreeTicket && match.HomeTeam.IsFreeTicket == false)
            {
                match.ResultDate = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
                match.PlayDate = match.ResultDate;
                match.HomeTeamScore = 10;
                match.GuestTeamScore = 0;
            }
            else if (match.HomeTeam.IsFreeTicket && match.GuestTeam.IsFreeTicket == false)
            {
                match.ResultDate = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
                match.PlayDate = match.ResultDate;
                match.HomeTeamScore = 0;
                match.GuestTeamScore = 10;
            }
        }

        public static List<Match> CreateCupMatches(List<Team> teams, List<League> leagues, IDbConnection db)
        {
            var listOfMatches = new List<Match>();

            teams.Shuffle();

            int roundOrder = 1;
            var freeTickets = CreateFreeTickets(teams.Count, db);

            while (teams.Any())
            {
                var firstTeam = teams.Pop(0);
                var secondTeam = freeTickets.Any() ? freeTickets.Pop(0) : teams.Pop(0);

                var match = new Match { CupRound = 1, RoundOrder = roundOrder++, TableId = null, PlayDate = null };

                var firstTeamLeague = leagues.First(p => p.Id == firstTeam.LeagueId);
                var secondTeamLeague = leagues.FirstOrDefault(p => p.Id == secondTeam.LeagueId);

                if (secondTeam.IsFreeTicket || (firstTeamLeague.Number == secondTeamLeague.Number || firstTeamLeague.Number > secondTeamLeague.Number))
                {
                    match.HomeTeamId = firstTeam.Id;
                    match.HomeTeam = firstTeam;
                    match.GuestTeamId = secondTeam.Id;
                    match.GuestTeam = secondTeam;
                }
                else
                {
                    match.HomeTeamId = secondTeam.Id;
                    match.HomeTeam = secondTeam;
                    match.GuestTeamId = firstTeam.Id;
                    match.GuestTeam = firstTeam;
                }

                SetFreeTicketResult(match);

                listOfMatches.Add(match);
            }

            return listOfMatches;
        }

        public static List<Match> CreateNextCupRound(IEnumerable<Match> previousRound, IEnumerable<Team> teams, IEnumerable<League> leagues, IDbConnection db)
        {
            var matchesOrdered = previousRound.OrderBy(o => o.RoundOrder).ToList();
            var leagueLookup = leagues.ToDictionary(k => k.Id);
            var teamLookup = teams.ToDictionary(k => k.Id);
            var matches = new List<Match>(matchesOrdered.Count / 2);
            var round = matchesOrdered.First().CupRound + 1;
            var cupId = matchesOrdered.First().CupId;
            var roundOrder = 1;

            for (var i = 0; i < matchesOrdered.Count; i = i + 2)
            {
                var match1 = matchesOrdered[i];
                var match2 = matchesOrdered[i + 1];

                var nextMatch = new Match { CupId = cupId, CupRound = round, RoundOrder = roundOrder++, PlayDate = null };

                if (match1.HasResult && match2.HasResult)
                {
                    var match1WinnerTeamId = match1.WinnerTeamId;
                    var match2WinnerTeamId = match2.WinnerTeamId;

                    var match1Team = teamLookup[match1WinnerTeamId];
                    var match2Team = teamLookup[match2WinnerTeamId];

                    var team1League = match1Team.LeagueId.HasValue ? leagueLookup[match1Team.LeagueId.Value] : null;
                    var team2League = match2Team.LeagueId.HasValue ? leagueLookup[match2Team.LeagueId.Value] : null;

                    SetNextMatchTeams(match1Team, nextMatch, match2Team, team1League, team2League);
                }
                else
                {
                    if (match1.HasResult == false && match2.HasResult)
                    {
                        // one predecessor didnt play
                        nextMatch.HomeTeamId = match2.WinnerTeamId;
                        nextMatch.HomeTeam = teamLookup[match2.WinnerTeamId];
                        nextMatch.GuestTeamId = match1.HomeTeamId;
                        nextMatch.GuestTeam = match1.GuestTeam;

                        UpdateNotPlayedMatch(match1, db);
                        SetResultForAbordedMatch(nextMatch);
                    }
                    else if (match1.HasResult && match2.HasResult == false)
                    {
                        // one predecessor didnt play
                        nextMatch.HomeTeamId = match1.WinnerTeamId;
                        nextMatch.HomeTeam = teamLookup[match1.WinnerTeamId];
                        nextMatch.GuestTeamId = match2.HomeTeamId;
                        nextMatch.GuestTeam = match2.HomeTeam;

                        UpdateNotPlayedMatch(match2, db);
                        SetResultForAbordedMatch(nextMatch);
                    }
                    else
                    {
                        // both predecessors didnt play
                        nextMatch.HomeTeamId = match1.HomeTeamId;
                        nextMatch.HomeTeam = match1.HomeTeam;
                        nextMatch.GuestTeamId = match2.HomeTeamId;
                        nextMatch.GuestTeam = match2.HomeTeam;

                        db.Update<Team>(new { IsFreeTicket = true }, p => p.Id == nextMatch.HomeTeamId);
                        db.Update<Team>(new { IsFreeTicket = true }, p => p.Id == nextMatch.GuestTeamId);

                        SetResultForAbordedMatch(nextMatch);
                    }
                }

                matches.Add(nextMatch);
            }

            return matches;
        }

        private static void UpdateNotPlayedMatch(Match match, IDbConnection db)
        {
            SetResultForAbordedMatch(match);
            db.Update(match);

            db.Update<Team>(new { IsFreeTicket = true }, p => p.Id == match.HomeTeamId);
            db.Update<Team>(new { IsFreeTicket = true }, p => p.Id == match.GuestTeamId);
        }

        private static void SetResultForAbordedMatch(Match match)
        {
            match.HomeTeamScore = 10;
            match.GuestTeamScore = 0;
            match.PlayDate = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
            match.RegisterDate = DateTime.UtcNow;
        }

        private static void SetNextMatchTeams(Team match1Team, Match nextMatch, Team match2Team, League team1League, League team2League)
        {
            if (match2Team.IsFreeTicket)
            {
                // team 2 is a free ticket
                nextMatch.HomeTeamId = match1Team.Id;
                nextMatch.HomeTeam = match1Team;
                nextMatch.GuestTeamId = match2Team.Id;
                nextMatch.GuestTeam = match2Team;
                SetResultForAbordedMatch(nextMatch);
            }
            else if (match1Team.IsFreeTicket)
            {
                // team 1 is a free ticket
                nextMatch.HomeTeamId = match2Team.Id;
                nextMatch.HomeTeam = match2Team;
                nextMatch.GuestTeamId = match1Team.Id;
                nextMatch.GuestTeam = match1Team;
                SetResultForAbordedMatch(nextMatch);
            }
            else if (team1League.Number > team2League.Number)
            {
                // none of the teams is a free ticket, league number decides home team
                nextMatch.HomeTeamId = match2Team.Id;
                nextMatch.HomeTeam = match2Team;
                nextMatch.GuestTeamId = match1Team.Id;
                nextMatch.GuestTeam = match1Team;
            }
            else if (team1League.Number < team2League.Number)
            {
                // none of the teams is a free ticket, league number decides home team
                nextMatch.HomeTeamId = match1Team.Id;
                nextMatch.HomeTeam = match1Team;
                nextMatch.GuestTeamId = match2Team.Id;
                nextMatch.GuestTeam = match2Team;
            }
            else
            {
                var random = new Random();
                if (random.Next(0, 999) < 500)
                {
                    nextMatch.HomeTeamId = match1Team.Id;
                    nextMatch.HomeTeam = match1Team;
                    nextMatch.GuestTeamId = match2Team.Id;
                    nextMatch.GuestTeam = match2Team;
                }
                else
                {
                    nextMatch.HomeTeamId = match2Team.Id;
                    nextMatch.HomeTeam = match2Team;
                    nextMatch.GuestTeamId = match1Team.Id;
                    nextMatch.GuestTeam = match1Team;
                }
            }
        }

        public static List<Team> CreateFreeTickets(int countTeams, IDbConnection db)
        {
            var countFreeTickets = (int)Math.Pow(2, Math.Ceiling(Math.Log(countTeams, 2))) - countTeams;

            var freeTickets = new List<Team>(countFreeTickets);

            for (var i = 0; i < countFreeTickets; ++i)
            {
                var freeTicket = Team.CreateFreeTicket();
                freeTicket.Id = (int)db.Insert(freeTicket, true);
                freeTickets.Add(freeTicket);
            }

            return freeTickets;
        }

        public static void ForfaitOpenCupMatchesForTeam(int id, IDbConnection db)
        {
            var homeMatches = db.Select(db.From<Match>().Where(p => p.HomeTeamId == id && p.CupId > 0));
            var guestMatches = db.Select(db.From<Match>().Where(p => p.GuestTeamId == id && p.CupId > 0));

            var time = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));

            homeMatches.ForEach(match => db.Update<Match>(new { HomeTeamScore = 0, GuestTeamScore = 10, PlayDate = time }, p => p.Id == match.Id));
            guestMatches.ForEach(match => db.Update<Match>(new { HomeTeamScore = 10, GuestTeamScore = 0, PlayDate = time }, p => p.Id == match.Id));
        }
    }
}
