using System.Collections.Generic;

using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public interface IOrderRule : IComparer<TableEntry>
    {

    }
}