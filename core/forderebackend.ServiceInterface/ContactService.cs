﻿
using Fordere.RestService.Smtp;
using Fordere.ServiceInterface.Messages;

using ServiceStack;

namespace Fordere.RestService
{
    
    public class ContactService : BaseService
    {
        public void Post(ContactRequest contact)
        {
            MailSender.SendContactMail(DivisionId, contact.Name, contact.Mail, contact.Comment, contact.Mail);
        }
    }
}