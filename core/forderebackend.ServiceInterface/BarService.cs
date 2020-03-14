using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Bar;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class BarService : BaseService
    {
        [Authenticate]
        public object Get(GetBarByIdRequest request)
        {
            var bar = Db.SingleById<Bar>(request.Id);

            if (bar == null)
            {
                throw HttpError.NotFound("Bar not found");
            }

            return bar;
        }

        [Authenticate]
        public object Get(GetAllBarsRequest request)
        {
            return Db.Select(Db.From<Bar>().Where(x => x.DivisionId == DivisionId).OrderBy(o => o.Name));
        }

        public object Get(GetAllBarsWithTableAvailability request)
        {
            var tables = Db.LoadSelect<Table>().ToList();
            return tables.Where(t => t.TableAvailabilities != null).Select(t => t.Bar).Where(x => x.DivisionId == DivisionId).Distinct();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(UpdateBarRequest request)
        {
            var barToUpdate = new Bar();
            if (request.Id != 0)
            {
                barToUpdate = Db.LoadSingleById<Bar>(request.Id);
            }

            barToUpdate.PopulateWith(request);
            barToUpdate.DivisionId = DivisionId;
            Db.Save(barToUpdate);
            return barToUpdate.ConvertTo<BarDto>();
        }
    }
}