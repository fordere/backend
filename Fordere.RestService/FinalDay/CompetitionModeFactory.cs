using System;
using System.Data;

using Fordere.RestService.Entities;
using Fordere.RestService.Entities.Final;
using Fordere.ServiceInterface.Messages.Final;

using ServiceStack.OrmLite;

namespace Fordere.RestService.FinalDay
{
    public class CompetitionModeFactory
    {
        public static ICompetitionMode GetCompetitionMode(IDbConnection dbConnection, CompetitionMode competitionMode)
        {
            switch (competitionMode)
            {
                case CompetitionMode.SingleKO:
                    return new SingleKoCompetitionMode(dbConnection);
                case CompetitionMode.DoubleKO:
                    return new DoubleKoCompetitionMode();
                case CompetitionMode.Group:
                    return new GroupCompetitionMode(dbConnection);
                case CompetitionMode.CrazyDyp:
                    return new CrazyDypCompetitionMode(dbConnection);
            }

            throw new InvalidOperationException("Got unknown Competition Mode");
        }

        public static ICompetitionMode GetCompetitionMode(IDbConnection dbConnection, Match match)
        {
            var finalDayCompetition = dbConnection.SingleById<FinalDayCompetition>(match.FinalDayCompetitionId);
            return GetCompetitionMode(dbConnection, finalDayCompetition.CompetitionMode);
        }
    }
}
