using Fordere.RestService.Smtp;
using Fordere.ServiceInterface.Messages;

namespace Fordere.RestService
{
    public class RegistrationService : BaseService
    {
        public void Post(TournamentRegistrationStsRequest request)
        {
            var tournaments = "";

            foreach (var registration in request.Tournaments)
            {
                if (!string.IsNullOrEmpty(tournaments))
                {
                    tournaments += ", ";
                }

                tournaments += registration.TournamentIdentifier.ToUpper();

                if (!string.IsNullOrEmpty(registration.TeamMate))
                {
                    tournaments += " mit " + registration.TeamMate;
                }
            }

            MailSender.SendTournamentRegistrationStsMail(request.UserMail, request.UserName, tournaments);
            MailSender.SendTournamentRegistrationStsMail("info@fordere.ch", request.UserName, tournaments);
        }

        public void Post(TournamentRegistrationReisliRequest request)
        {
            if (request.Tournament == "1")
            {
                MailSender.SendReisliDoubleDoubleRegistrationStsMail(request.Player1.Mail, request);
                MailSender.SendReisliDoubleDoubleRegistrationStsMail("reisli@fordere.ch", request);
            }
            else
            {
                MailSender.SendReisliHeroesRegistrationStsMail(request.Player1.Mail, request);
                MailSender.SendReisliHeroesRegistrationStsMail("reisli@fordere.ch", request);
            }
        }
    }
}
