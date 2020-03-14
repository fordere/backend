using System;
using System.IO;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages;

using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;

namespace Fordere.RestService.Smtp
{
    public static class MailSender
    {
        private const string templateRoot = "./MailTemplates/";

        private static ILog log = LogManager.GetLogger(typeof(MailSender));

        private static string ReadSubject(string template, int? divisionId = null)
        {
            var path = Path.Combine(templateRoot, GetTemplateName(divisionId, template));

            // First line of the template is the subject
            if (File.Exists(path))
            {
                return File.ReadLines(path).First();
            }

            log.ErrorFormat("E-Mail Template not found: '{0}'", path);

            return string.Empty;
        }

        private static string GetTemplateName(int? divisionId, string template)
        {
            if (divisionId.HasValue)
            {
                return divisionId.Value + "_" + template;
            }

            return template;
        }

        private static string ReadBody(string template, int? divisionId = null)
        {
            var path = Path.Combine(templateRoot, GetTemplateName(divisionId, template));

            if (File.Exists(path))
            {
                var lines = File.ReadLines(path).Skip(1).ToList();
                return string.Join(Environment.NewLine, lines);
            }

            log.ErrorFormat("E-Mail Template not found: '{0}'", path);

            return string.Empty;
        }

        /// <summary>
        /// Email after registration
        /// </summary>
        public static void SendWelcomeMail(string to, string firstName, string lastName)
        {
            var subject = ReadSubject("Welcome.html");
            var body = ReadBody("Welcome.html");

            if (string.IsNullOrEmpty(body) == false &&
                string.IsNullOrEmpty(subject) == false)
            {
                body = body.Replace("%FIRSTNAME%", firstName);
                body = body.Replace("%LASTNAME%", lastName);
                body = body.Replace("%ACTIVATIONLINK%", "");

                log.InfoFormat("Sending Welcome Mail to {0} {1} ({2})", firstName, lastName, to);
                SmtpClientWrapper.Send(to, subject, body);
            }
        }

        public static void SendReisliDoubleDoubleRegistrationStsMail(string to, TournamentRegistrationReisliRequest request)
        {
            var subject = ReadSubject("RegisterDoubleDouble.html");
            var body = ReadBody("RegisterDoubleDouble.html");

            body = body.Replace("%PLAYER1_NAME%", request.Player1.Name);
            body = body.Replace("%PLAYER1_ABO%", request.Player1.Abo);
            body = body.Replace("%PLAYER1_MAIL%", request.Player1.Mail);

            body = body.Replace("%PLAYER2_NAME%", request.Player2.Name);
            body = body.Replace("%PLAYER2_ABO%", request.Player2.Abo);
            body = body.Replace("%PLAYER2_MAIL%", request.Player2.Mail);

            body = body.Replace("%PLAYER3_NAME%", request.Player3.Name);
            body = body.Replace("%PLAYER3_ABO%", request.Player3.Abo);
            body = body.Replace("%PLAYER3_MAIL%", request.Player3.Mail);

            body = body.Replace("%PLAYER4_NAME%", request.Player4.Name);
            body = body.Replace("%PLAYER4_ABO%", request.Player4.Abo);
            body = body.Replace("%PLAYER4_MAIL%", request.Player4.Mail);

            body = body.Replace("%TOTAL%", request.Total + "");
            body = body.Replace("%COMMENT%", request.Comment);
            body = body.Replace("%TEAMNAME%", request.TeamName);

            SmtpClientWrapper.Send(to, subject, body, "reisli@fordere.ch");
        }

        public static void SendReisliHeroesRegistrationStsMail(string to, TournamentRegistrationReisliRequest request)
        {
            var subject = ReadSubject("RegisterHeroes.html");
            var body = ReadBody("RegisterHeroes.html");

            body = body.Replace("%PLAYER1_NAME%", request.Player1.Name);
            body = body.Replace("%PLAYER1_ABO%", request.Player1.Abo);
            body = body.Replace("%PLAYER1_MAIL%", request.Player1.Mail);

            body = body.Replace("%PLAYER2_NAME%", request.Player2.Name);
            body = body.Replace("%PLAYER2_ABO%", request.Player2.Abo);
            body = body.Replace("%PLAYER2_MAIL%", request.Player2.Mail);

            body = body.Replace("%TOTAL%", request.Total + "");
            body = body.Replace("%COMMENT%", request.Comment);
            body = body.Replace("%TEAMNAME%", request.TeamName);

            SmtpClientWrapper.Send(to, subject, body, "reisli@fordere.ch");
        }

        public static void SendTournamentRegistrationStsMail(string to, string name, string tournaments)
        {
            var subject = ReadSubject("TournamentRegistration.html");
            var body = ReadBody("TournamentRegistration.html");

            body = body.Replace("%NAME%", name);
            body = body.Replace("%TOURNAMENTS%", tournaments);

            log.InfoFormat("Sending Contact Mail of {0} ({1})", name, to);
            SmtpClientWrapper.Send(to, subject, body, "info@fordere.ch");
        }

        public static void SendContactMail(int? divisionId, string name, string userMail, string comment, string replyTo = null)
        {
            var subject = ReadSubject("Contact.html");
            var body = ReadBody("Contact.html");

            body = body.Replace("%NAME%", name);
            body = body.Replace("%USERMAIL%", userMail);
            body = body.Replace("%COMMENT%", comment);

            log.InfoFormat("Sending Contact Mail of {0} ({1})", name, userMail);
            if (divisionId == 3)
            {
                // TODO this info should be moved into the divisions-table
                SmtpClientWrapper.Send("info.luzern@fordere.ch", subject, body, replyTo);
            }
            else
            {
                SmtpClientWrapper.Send("info@fordere.ch", subject, body, replyTo);
            }

        }

        public static void SendPasswordRecoveryMail(UserAuth user)
        {
            var subject = ReadSubject("PasswordRecovery.html");
            var body = ReadBody("PasswordRecovery.html");

            if (string.IsNullOrEmpty(body) == false && string.IsNullOrEmpty(subject) == false)
            {
                var link = "https://fordere.ch/new-password/{0}".Fmt(user.RecoveryToken);
                body = body.Replace("%FIRSTNAME%", user.FirstName);
                body = body.Replace("%LASTNAME%", user.LastName);
                body = body.Replace("%LINK%", link);

                log.InfoFormat("Sending Password Recovery Mail to {0} {1} ({2}) [{3}]", user.FirstName, user.LastName, user.Email, link);
                SmtpClientWrapper.Send(user.Email, subject, body);
            }
        }

        public static void SendRegistrationConfirmation(int? divisionId, string teamName, string teamMotto, string teamWishPlayDay, UserAuth player1, UserAuth player2, Competition league, Bar homeBar)
        {
            var subject = ReadSubject("RegistrationConfirmation.html", divisionId);
            subject = subject.Replace("%LEAGUENAME%", league.Name);

            var body = ReadBody("RegistrationConfirmation.html", divisionId);

            body = body.Replace("%LEAGUENAME%", league.Name);
            body = body.Replace("%TEAMNAME%", teamName);
            body = body.Replace("%MOTTOSAISONZIEL%", teamMotto);
			//body = body.Replace("%WUNSCHLIGA%", teamWishLeague);
			body = body.Replace("%WUNSCHSPIELTAG%", teamWishPlayDay);
            body = body.Replace("%HOMELOCATION%", homeBar.Name);
            body = body.Replace("%PLAYER1.NAME%", string.Format("{0} {1}", player1.FirstName, player1.LastName));
            body = body.Replace("%PLAYER1.MAIL%", player1.Email);
            body = body.Replace("%PLAYER1.PHONE%", player1.PhoneNumber);
            body = body.Replace("%PLAYER2.NAME%", string.Format("{0} {1}", player2.FirstName, player2.LastName));
            body = body.Replace("%PLAYER2.MAIL%", player2.Email);
            body = body.Replace("%PLAYER2.PHONE%", player2.PhoneNumber);

            var recipients = new[] { GetDivisionMail(divisionId), player1.Email, player2.Email };
            var allRecipients = string.Join(",", recipients);

            log.InfoFormat("Sending RegistrationConfirmation for League '{0}' to {1}", league.Name, allRecipients);
            SmtpClientWrapper.Send(allRecipients, subject, body);
        }

        public static void SendGenericMessage(string mailAddress, string subject, string message)
        {
            SmtpClientWrapper.Send(mailAddress, subject, message);
        }

        public static void SendMatchRecall(string mailAddress, Match match)
        {
            // TODO ssh
        }

        public static void SendMatchAssigend(string mailAddress, Match match)
        {
            // TODO ssh
        }

        private static string GetDivisionMail(int? divisionId)
        {
            if (divisionId == null)
            {
                return "info@fordere.ch";
            }

            switch (divisionId)
            {
                case 1:
                    return "info@fordere.ch";
                case 2:
                    return "sg@fordere.ch";
                case 3:
                    return "info.luzern@fordere.ch";
                case 4:
                    return "winti@fordere.ch";
            }

            return "info@fordere.ch";
        }
    }
}
