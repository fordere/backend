using System.Collections.Generic;
using forderebackend.ServiceInterface.Entities;

namespace forderebackend.ServiceInterface.LeagueExecution.Standings.OrderRules
{
    public interface IOrderRule : IComparer<TableEntry>
    {

    }
}