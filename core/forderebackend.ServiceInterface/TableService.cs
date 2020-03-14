using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.Extensions;

using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Table;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    
    public class TableService : BaseService
    {
        [Authenticate]
        public object Get(GetTablesInBarForCompetitionRequest request)
        {
            var allTablesInBar = Db.Select<Table>(x => x.BarId == request.BarId);

            var competition = Db.LoadSingleById<Competition>(request.CompetitionId);
            competition.Throw404NotFoundIfNull("Competition not found");

            var tables = allTablesInBar.Where(x => competition.TableTypes.Any(t => t.TableTypeValue == x.TableTypeValue)).Select(x => x.ToDto());
            return tables;
        }

        // TODO: Do we need GettablesInBarForCompetition and GetTablesInBarForCup?
        [Authenticate]
        public object Get(GetTablesInBarForCupRequest request)
        {
            var allTablesInBar = Db.Select<Table>(x => x.BarId == request.BarId);

            var cup = Db.LoadSingleById<Cup>(request.CupId).Competition;
            cup.Throw404NotFoundIfNull("Cup not found");

            return allTablesInBar.Where(x => cup.TableTypes.Any(t => t.TableTypeValue == x.TableTypeValue)).Select(x => x.ToDto());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllTablesRequest tables)
        {
            return Db.LoadSelect<Table>().Select(x => x.ToDto());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetTablesInBarRequest request)
        {
            return Db.LoadSelect<Table>().Where(x => x.BarId == request.BarId).Select(x => x.ToDto());
        }

        [Authenticate]
        public object Get(GetTableByIdRequest request)
        {
            return Db.LoadSingleById<Table>(request.TableId).ToDto();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(CreateTableRequest request)
        {
            var tableToUpdate = new Table();
            tableToUpdate.PopulateWith(request);
            Db.Save(tableToUpdate);
            return tableToUpdate.ConvertTo<TableDto>();
        }


        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(UpdateTableRequest request)
        {
            var tableToUpdate = Db.LoadSingleById<Table>(request.Id);
            tableToUpdate.PopulateWith(request);
            Db.Save(tableToUpdate);
            return tableToUpdate.ConvertTo<TableDto>();
        }


    }
}