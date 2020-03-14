using forderebackend.ServiceModel.Dtos.FinalDay;
using forderebackend.ServiceModel.Messages.Final;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    internal class FinalDayService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetFinalDayRequest request)
        {
            return Db.LoadSingleById<Entities.Final.FinalDay>(request.Id).ConvertTo<FinalDayDto>();
        }
    }
}