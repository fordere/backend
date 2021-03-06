﻿using System.Linq;

using Fordere.RestService.Entities.Final;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos.FinalDay;
using Fordere.ServiceInterface.Messages.Final;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class FinaldayTableService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Delete(DeleteFinalDayTableRequest request)
        {
            return Db.DeleteById<FinalDayTable>(request.Id);
            ;
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetFinalDayTableRequest request)
        {
            return Db.LoadSingleById<FinalDayTable>(request.Id).ConvertTo<FinalDayTableDto>();
        }

        //[Authenticate]
        //[RequiredRole(RoleNames.Admin)]
        public object Get(GetAllFinalDayTablesRequest request)
        {
            var finalDayTables = Db.Select<FinalDayTable>(table => table.FinalDayId == request.FinalDayId).OrderBy(table => table.Number);
            return finalDayTables.Select(s => s.ConvertTo<FinalDayTableDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddFinalDayTableRequest request)
        {
            var newId = Db.Insert(new FinalDayTable() {FinalDayId = request.FinalDayId, Number = request.Number, TableType = request.TableType, Disabled = false}, true);
            return Get(new GetFinalDayTableRequest {Id = (int)newId});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(UpdateFinalDayTableNumberRequest request)
        {
            var finalDayTable = Db.SingleById<FinalDayTable>(request.Id);
            finalDayTable.Number = request.Number;
            Db.Save(finalDayTable);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(UpdateFinalDayTableDisabledRequest request)
        {
            var finalDayTable = Db.SingleById<FinalDayTable>(request.Id);
            finalDayTable.Disabled = request.Disabled;
            Db.Save(finalDayTable);
        }
    }
}