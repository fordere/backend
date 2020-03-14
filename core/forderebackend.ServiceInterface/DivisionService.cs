using System.Linq;

using Fordere.RestService.Entities;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Bar;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    public class DivisionService : BaseService
    {
        public object Get(GetAllDivisionsRequest request)
        {
            return Db.Select(Db.From<Division>()).Select(x => x.ConvertTo<DivisionDto>());
        }

        public object Get(GetPaymentInformationsRequest request)
        {
            return Db.LoadSingleById<Division>(DivisionId).ConvertTo<DivisionPaymentInfosDto>();
        }
    }
}