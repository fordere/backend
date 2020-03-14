using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.Entities.Final;

using ServiceStack.OrmLite;

namespace Fordere.RestService.FinalDay
{
    public class SingleKoTeamSorter
    {
        public static List<TeamInGroup> Sort(List<TeamInGroup> teams)
        {
            // http://stackoverflow.com/questions/5770990/sorting-tournament-seeds
            var bracketList = new List<TeamInGroup>();
            bracketList.AddRange(teams);

            int slice = 1;
            while (slice < bracketList.Count / 2)
            {
                var temp = bracketList;
                bracketList = new List<TeamInGroup>();

                while (temp.Count > 0)
                {
                    for (int i = 0; i < slice; i++)
                    {
                        bracketList.Add(temp.First());
                        temp.Remove(temp.First());
                    }

                    for (int i = 0; i < slice; i++)
                    {
                        bracketList.Add(temp.Last());
                        temp.Remove(temp.Last());
                    }
                }

                slice *= 2;
            }

            return bracketList;
        }
    }

    public class SingleKoCompetitionMode : ICompetitionMode
    {
        private readonly IDbConnection dbConnection;

        public SingleKoCompetitionMode(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public List<Match> GenerateMatches(int finalDayCompetitionId)
        {
            var groups = dbConnection.LoadSelect<Group>(x => x.FinalDayCompetitionId == finalDayCompetitionId);
            var generatedMatches = new List<Match>();

            foreach (var finalDayGroup in groups)
            {
                var teams = finalDayGroup.Teams;
                generatedMatches.AddRange(GenerateMatchesForGroup(teams, finalDayCompetitionId));
            }

            generatedMatches.ForEach(x => x.FinalDayCompetitionId = finalDayCompetitionId);

            return generatedMatches;
        }

        private List<Match> GenerateMatchesForGroup(List<TeamInGroup> teams, int finalDayCompetitionId)
        {
            if (teams == null)
            {
                return new List<Match>();
            }

            var orderedTeams = teams.OrderBy(x => x.Settlement).ToList();
            int bracketSize = FindSmallestPossibleBracket(teams.Count);

            FillWithFreilos(orderedTeams, bracketSize);

            // TODO SSH DI?!?
            orderedTeams = SingleKoTeamSorter.Sort(orderedTeams);

            var matches = new List<Match>();
            int counter = 0;
            while (orderedTeams.Any())
            {
                TeamInGroup homeTeam = orderedTeams.First();
                TeamInGroup guestTeam = orderedTeams.Skip(1).First();

                orderedTeams.Remove(homeTeam);
                orderedTeams.Remove(guestTeam);

                var match = new Match { HomeTeamId = homeTeam.TeamId, GuestTeamId = guestTeam.TeamId, RoundOrder = counter, CupRound = 1, FinalDayCompetitionId = finalDayCompetitionId };
                // TODO SSH: 0/4 müssen konfigurierbar sein
                // TODO SSH Code Duplication.. 
                if (IsFreeTicket(homeTeam))
                {
                    match.HomeTeamScore = 0;
                    match.GuestTeamScore = 4;
                    match.ResultDate = DateTime.Now;
                    match.RegisterDate = DateTime.Now;
                    match.PlayDate = DateTime.Now;
                }

                if (IsFreeTicket(guestTeam))
                {
                    match.HomeTeamScore = 4;
                    match.GuestTeamScore = 0;
                    match.RegisterDate = DateTime.Now;
                    match.ResultDate = DateTime.Now;
                    match.PlayDate = DateTime.Now;
                }

                matches.Add(match);
                counter++;
            }

            return matches;
        }

        private bool IsFreeTicket(TeamInGroup team)
        {
            if (team.Team != null)
            {
                return team.Team.IsFreeTicket;
            }

            return dbConnection.SingleById<Team>(team.TeamId).IsFreeTicket;
        }

        public List<Match> GenerateMatchAfterMatchResultEntered(Match match)
        {
            Match correspondingMatch = FindCorrespondingMatch(match);
            if (correspondingMatch == null || !match.HasResult)
            {
                return new List<Match>();
            }

            if (!match.RoundOrder.HasValue || !correspondingMatch.RoundOrder.HasValue)
            {
                return new List<Match>();
            }

            if (IsFinal(match.FinalDayCompetitionId.Value, match.CupRound))
            {
                // Nothing to generate after final matches
                return new List<Match>();
            }

            int nextRoundOrder = Math.Max(match.RoundOrder.Value, correspondingMatch.RoundOrder.Value) / 2;
            var nextCupRound = match.CupRound + 1;

            var furtherMatch = LoadMatch(nextCupRound, nextRoundOrder, match.FinalDayCompetitionId.Value);
            if (furtherMatch != null)
            {
                if (furtherMatch.HomeTeamId != match.WinnerTeamId || furtherMatch.GuestTeamId != correspondingMatch.WinnerTeamId)
                {
                    DeleteAllFurtherMatches(nextCupRound, nextRoundOrder, match.FinalDayCompetitionId.Value);
                }
                else
                {
                    // Do nothing, result has changed but winner is the same...
                    return new List<Match>();
                }
            }

            var upcommingMatches = new List<Match>();
            var upcommingMatch = new Match { HomeTeamId = match.WinnerTeamId, GuestTeamId = correspondingMatch.WinnerTeamId, CupRound = nextCupRound, RoundOrder = nextRoundOrder, FinalDayCompetitionId = match.FinalDayCompetitionId };

            if (IsFinal(upcommingMatch.FinalDayCompetitionId.Value, upcommingMatch.CupRound))
            {
                var matchPlace3 = new Match { HomeTeamId = match.LoserTeamId, GuestTeamId = correspondingMatch.LoserTeamId, CupRound = nextCupRound, RoundOrder = nextRoundOrder + 1, FinalDayCompetitionId = match.FinalDayCompetitionId };
                upcommingMatches.Add(matchPlace3);
            }

            upcommingMatches.Add(upcommingMatch);

            return upcommingMatches;
        }

        private bool IsFinal(int finalCompetitionId, int cupRound)
        {
            var numberOfFirstRoundMatches = dbConnection.Count(dbConnection.From<Match>().Where(x => x.FinalDayCompetitionId == finalCompetitionId && x.CupRound == 1));

            int finalRound = 1;
            while (numberOfFirstRoundMatches > 1)
            {
                finalRound++;
                numberOfFirstRoundMatches /= 2;
            }

            return finalRound == cupRound;
        }

        public void AfterMatchSafe(List<Match> matches, int finalDayCompetitionId)
        {
            // TODO SSH Refactor this
            var teamsMatchesCreatedFor = new List<int>();
            var newMatches = new List<Match>();
            foreach (var matchWithResult in matches.Where(x => x.HasResult))
            {
                if (teamsMatchesCreatedFor.Contains(matchWithResult.HomeTeamId) || teamsMatchesCreatedFor.Contains(matchWithResult.GuestTeamId))
                {
                    continue;
                }

                var matchesToAdd = GenerateMatchAfterMatchResultEntered(matchWithResult);
                if (matchesToAdd.Any())
                {
                    foreach (var match in matchesToAdd)
                    {
                        newMatches.Add(match);

                        teamsMatchesCreatedFor.Add(match.HomeTeamId);
                        teamsMatchesCreatedFor.Add(match.GuestTeamId);
                    }
                }
            }

            dbConnection.SaveAll(newMatches);
        }

        public long GetNumberOfMatches(int finalDayCompetitionId)
        {
            var numberOfMatchesFirstRound = dbConnection.Count<MatchView>(x => x.FinalDayCompetitionId == finalDayCompetitionId && x.CupRound == 1);

            // Anzahl Spiele = (Anzahl teams) -1, da wir noch 3. platz ausspielen = anzahl teams
            return numberOfMatchesFirstRound * 2;
        }

        public bool ShouldFinishCompetitionWhenNoMoreMatchesOpen()
        {
            return true;
        }

        private void DeleteAllFurtherMatches(int cupRound, int roundOrder, int finalDayCompetitionId)
        {
            var matchToDelete = LoadMatch(cupRound, roundOrder, finalDayCompetitionId);
            while (matchToDelete != null)
            {
                dbConnection.DeleteById<Match>(matchToDelete.Id);

                cupRound++;
                roundOrder = (int)Math.Ceiling((decimal)roundOrder / 2);

                if (IsFinal(matchToDelete.FinalDayCompetitionId.Value, matchToDelete.CupRound.Value))
                {
                    matchToDelete = LoadMatch(matchToDelete.CupRound.Value, matchToDelete.RoundOrder + 1, finalDayCompetitionId);
                }
                else
                {
                    matchToDelete = LoadMatch(cupRound, roundOrder, finalDayCompetitionId);
                }
            }
        }

        private MatchView LoadMatch(int cupRound, int roundOrder, int finalDayCompetitionId)
        {
            return dbConnection.Single(dbConnection.From<MatchView>().Where(x => x.CupRound == cupRound && x.RoundOrder == roundOrder && x.FinalDayCompetitionId == finalDayCompetitionId));
        }

        private Match FindCorrespondingMatch(Match match)
        {
            if (!match.RoundOrder.HasValue)
            {
                return null;
            }

            int expectedRoundOrder = match.RoundOrder.Value - 1;
            if (match.RoundOrder % 2 == 0)
            {
                expectedRoundOrder = match.RoundOrder.Value + 1;
            }

            var matchingMatches = dbConnection.Select<Match>(x => x.RoundOrder == expectedRoundOrder && x.CupRound == match.CupRound && x.Id != match.Id && x.FinalDayCompetitionId == match.FinalDayCompetitionId && x.HomeTeamScore != null && x.GuestTeamScore != null);
            if (matchingMatches.Any())
            {
                return matchingMatches.Single();
            }

            return null;
        }

        private void FillWithFreilos(ICollection<TeamInGroup> orderedTeams, int bracketSize)
        {
            while (orderedTeams.Count < bracketSize)
            {
                orderedTeams.Add(new TeamInGroup { TeamId = CreateNewFreilos(), Settlement = orderedTeams.Max(x => x.Settlement) + 1 });
            }
        }

        private int CreateNewFreilos()
        {
            return (int)dbConnection.Insert(Team.CreateFreeTicket(), true);
        }

        private static int FindSmallestPossibleBracket(int numberOfTeams)
        {
            int bracketSize = 2;
            while (numberOfTeams > bracketSize)
            {
                bracketSize *= 2;
            }

            return bracketSize;
        }
    }
}
