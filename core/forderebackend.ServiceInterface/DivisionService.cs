using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Bar;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
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