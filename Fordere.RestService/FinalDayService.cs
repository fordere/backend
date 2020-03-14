using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos.FinalDay;
using Fordere.ServiceInterface.Messages.Final;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class FinalDayService : BaseService
    {

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetFinalDayRequest request)
        {
            return Db.LoadSingleById<Entities.Final.FinalDay>(request.Id).ConvertTo<FinalDayDto>();
        }
    }
}
