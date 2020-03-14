using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceModel.Messages.Table;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    
    public class TableAvailabilityService : BaseService
    {
        [Authenticate]
        public object Get(TimeSlotRequest request)
        {
            var tableId = request.TableId;
            var dayOfWeek = (int)request.Day.DayOfWeek;
            var tableAvailability = this.Db.Single<TableAvailability>(x => x.TableId == request.TableId && x.FirstTimeSlotDayOfWeek == dayOfWeek);

            tableAvailability.Throw404NotFoundIfNull("An diesem Tag ist dieser Tisch nicht verfügbar!");

            var possibleTimeSlots = TimeSlotFactory.GetPossibleTimeSlots(request.Day, tableAvailability);

            List<Match> matches = Db.Select<Match>(sql => sql.PlayDate != null && sql.ResultDate == null && sql.TableId == tableId);
            var filterdTimeSlots = possibleTimeSlots.Except(matches.Select(x => x.PlayDate.Value));

            // TODO Add a Z to make it UTC is a huge hack...
            return filterdTimeSlots.Select(slot => slot.ToString("o") + "Z");
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllTableAvailabilitiesRequest request)
        {
            return Db.Select<TableAvailability>(x => x.TableId == request.TableId).Select(x => x.ToDto());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Delete(DeleteTableAvailabilityRequest request)
        {
            Db.DeleteById<TableAvailability>(request.TableAvailabilityId);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(AddTableAvailabilityRequest request)
        {
            var availability = request.ConvertTo<TableAvailability>();

            var id = (int)Db.Insert(availability, true);

            return Db.SingleById<TableAvailability>(id).ToDto();
        }
    }
}