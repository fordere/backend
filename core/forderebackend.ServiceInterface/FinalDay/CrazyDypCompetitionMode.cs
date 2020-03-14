using System.Collections.Generic;
using System.Data;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceInterface.Extensions;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    public class CrazyDypCompetitionMode : ICompetitionMode
    {
        private readonly IDbConnection dbConnection;

        public CrazyDypCompetitionMode(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        // TODO SSH To nod recreate already once played matches
        public List<Match> GenerateMatches(int finalDayCompetitionId)
        {
            var players = dbConnection.LoadSelect(dbConnection.From<PlayerInFinalDayCompetition>()
                .Where(x => x.IsActive && x.FinalDayCompetitionId == finalDayCompetitionId));

            var groupId = dbConnection.Insert(new Group {FinalDayCompetitionId = finalDayCompetitionId}, true);

            players.Shuffle();

            var matches = new List<Match>();

            while (players.Count >= 4)
            {
                var hometeam = new Team
                {
                    Player1Id = players[0].PlayerId, Player2Id = players[1].PlayerId,
                    Name = players[0].Player.FirstName + " " + players[0].Player.LastName + " / " +
                           players[1].Player.FirstName + " " + players[1].Player.LastName
                };
                var homeTeamId = dbConnection.Insert(hometeam, true);

                var guestteam = new Team
                {
                    Player1Id = players[2].PlayerId, Player2Id = players[3].Player.Id,
                    Name = players[2].Player.FirstName + " " + players[2].Player.LastName + " / " +
                           players[3].Player.FirstName + " " + players[3].Player.LastName
                };
                var guestTeamId = dbConnection.Insert(guestteam, true);

                dbConnection.Insert(new TeamInGroup {TeamId = (int) homeTeamId, GroupId = (int) groupId});
                dbConnection.Insert(new TeamInGroup {TeamId = (int) guestTeamId, GroupId = (int) groupId});

                matches.Add(new Match
                {
                    HomeTeamId = (int) homeTeamId,
                    GuestTeamId = (int) guestTeamId,
                    FinalDayCompetitionId = finalDayCompetitionId
                });


                players.RemoveRange(0, 4);
            }

            return matches;
        }

        public List<Match> GenerateMatchAfterMatchResultEntered(Match match)
        {
            if (match.FinalDayCompetitionId.HasValue)
                CompetitionPlayerStandingsCalculator.Calculate(dbConnection, match.FinalDayCompetitionId.Value);

            // By Default no new Matches are generated -> This only happens on a new Crazy-DYP Round
            return new List<Match>();
        }

        public void AfterMatchSafe(List<Match> dayCompetitionId, int finalDayCompetitionId)
        {
            CompetitionPlayerStandingsCalculator.Calculate(dbConnection, finalDayCompetitionId);
        }

        public long GetNumberOfMatches(int finalDayCompetitionId)
        {
            return dbConnection.Count(dbConnection.From<MatchView>()
                .Where(x => x.FinalDayCompetitionId == finalDayCompetitionId));
        }

        public bool ShouldFinishCompetitionWhenNoMoreMatchesOpen()
        {
            return false;
        }
    }
}