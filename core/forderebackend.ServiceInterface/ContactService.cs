using forderebackend.ServiceInterface.Smtp;
using forderebackend.ServiceModel.Messages;

namespace forderebackend.ServiceInterface
{
    public class ContactService : BaseService
    {
        public void Post(ContactRequest contact)
        {
            MailSender.SendContactMail(DivisionId, contact.Name, contact.Mail, contact.Comment, contact.Mail);
        }
    }
}