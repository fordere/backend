using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceModel.Messages.Season;
using forderebackend.ServiceModel.Messages.User;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Stripe.Types;

namespace forderebackend.ServiceInterface
{
    
    public class PaymentService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetOpenPayments request)
        {
            int seasonId = request.SeasonId;
            var payments = Db.LoadSelect<Payment>(sql => sql.SeasonId == seasonId && !sql.HasPaid).Select(x => x.ToDto()).OrderBy(x => x.User.Name).ToList();

            foreach (var paymentDto in payments)
            {
                paymentDto.UserTeams = QueryUserTeams(paymentDto.UserId, seasonId);
            }

            return payments;
        }

        [Authenticate]
        public object Get(GetPaymentCurrentUserCurrentSeason request)
        {
            // TODO extrag current season resolve into own service...
            var season = this.Db.Select(Db.From<Season>().Where(x => x.DivisionId == this.DivisionId).OrderByDescending(k => k.Id).Limit(1)).FirstOrDefault();

            return Db.Select<Payment>(sql => sql.SeasonId == season.Id && sql.UserId == SessionUserId).SingleOrDefault();
        }

        [Authenticate]
        public object Get(GetUserOpenPaymentsForCurrentSeason request)
        {
            var openPayments = GetOpenPayments();
            return openPayments.Select(payment => new OpenUserPaymentResponse { Amount = 25, Name = payment.User.FirstName + " " + payment.User.LastName }).ToList();
        }

        private List<Payment> GetOpenPayments()
        {
            // TODO SSH Maybe we have to handle that somehow different
            var season = Db.LoadSelect(Db.From<Season>().Where(x => x.State != SeasonState.Archived && x.DivisionId == DivisionId)).Single();

            var competitionIds = season.Competitions?.Select(x => x.Id) ?? new List<int>();

            var usersTheUserCanPayFor = Db.Select(Db.From<TeamInscription>().Where(x => Sql.In(x.CompetitionId, competitionIds) && (x.Player1Id == SessionUserId || x.Player2Id == SessionUserId))).SelectMany(x => new List<int> { x.Player1Id, x.Player2Id });
            var openPayments = Db.LoadSelect(Db.From<Payment>().Where(x => Sql.In(x.UserId, usersTheUserCanPayFor) && !x.HasPaid && x.SeasonId == season.Id));
            return openPayments;
        }

        [Authenticate]
        public void Post(PayRequest request)
        {
            ServicePointManager.ServerCertificateValidationCallback = Callback;
            var openPayments = GetOpenPayments();
            var usersPayedFor = openPayments.Select(x => x.User.FirstName + " " + x.User.LastName).Aggregate((i, j) => i + ", " + j);

            var division = Db.LoadSingleById<Division>(DivisionId);
            var gateway = new FordereStripeGateway(division.PrivateStripeKey);
            gateway.Post(new ChargeStripeCustomer()
            {
                Amount = request.Amount,
                Currency = Currencies.SwissFranc,
                Description = "Mitgliederbeitrag von " + usersPayedFor,
                Source = request.Token
            });

            openPayments.ForEach(x =>
            {
                x.HasPaid = true;
                x.Comment = "Kreditkarte";
            });

            Db.UpdateAll(openPayments);
        }

        private bool Callback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            LogManager.GetLogger(GetType()).Info("Cert callback");
            return true;
        }

        public object Get(GetPaymentInformationsRequest request)
        {
            return Db.SingleById<Division>(DivisionId).ConvertTo<PaymentInformationsDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetDonePayments request)
        {
            int seasonId = request.SeasonId;
            var payments = Db.LoadSelect<Payment>(sql => sql.SeasonId == seasonId && sql.HasPaid).Select(x => x.ToDto()).OrderBy(x => x.User.Name).ToList();

            foreach (var paymentDto in payments)
            {
                paymentDto.UserTeams = QueryUserTeams(paymentDto.UserId, seasonId);
            }

            return payments;
        }

        private string QueryUserTeams(int userId, int seasonId)
        {
            var competitionIds = Db.Select<Competition>(x => x.SeasonId == seasonId).Select(x => x.Id).ToList();

            var teams = Db.Select<TeamInscription>(t => Sql.In(t.CompetitionId, competitionIds) && (t.Player1Id == userId || t.Player2Id == userId))
                          .Select(x => x.Name).ToArray();

            return string.Join(" / ", teams);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(SavePayment request)
        {
            var payment = Db.Select<Payment>(x => x.Id == request.Id).SingleOrDefault();

            payment.Throw404NotFoundIfNull("Payment not found");

            payment.PopulateWith(request);

            this.Db.Update(payment);
        }
    }

    [Route("/charges")]
    public class ChargeStripeCustomer : IPost, IVerb, IReturn<StripeCharge>, IReturn
    {
        public int Amount { get; set; }

        public string Currency { get; set; }

        public string Customer { get; set; }

        public string Card { get; set; }

        public string Description { get; set; }

        public bool? Capture { get; set; }

        public int? ApplicationFee { get; set; }

        public string Source { get; set; }
    }
}