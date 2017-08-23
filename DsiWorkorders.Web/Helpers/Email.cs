using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;

namespace DsiWorkorders.Web.Helpers
{
    public class Email
    {

        public static void SendEmail(string toEmails, string emailSubject, string htmlEmailBody, string plainTextEmailBody, string replyToEmail, string ccEmails)
        {
            // create email message
            MailMessage msg = GetMailMessageObject(toEmails, emailSubject, htmlEmailBody, plainTextEmailBody,
                                                   replyToEmail, ccEmails);


            //create a Smtp Mail which will automatically get the smtp server details 
            //from web.config or app.config
            SmtpClient SmtpMail = new SmtpClient();

            // send message.
            SmtpMail.Send(msg);

        }

        public static void SendEmail(string toEmails, string emailSubject, string htmmlEmailBody, string plainTextEmailBody, List<Attachment> attachments, string replyToEmail, string ccEmails)
        {
            // create email message
            MailMessage msg = GetMailMessageObject(toEmails, emailSubject, htmmlEmailBody, plainTextEmailBody,
                                                   replyToEmail, ccEmails);

            //add attachments to email message
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    msg.Attachments.Add(new System.Net.Mail.Attachment(attachment.Stream, attachment.AttachmentName, attachment.ContentType));
                }
            }

            //create a Smtp Mail which will automatically get the smtp server details 
            //from web.config or app.config
            SmtpClient SmtpMail = new SmtpClient();

            // send message.
            SmtpMail.Send(msg);

        }

        public static MailMessage GetMailMessageObject(string toEmails, string emailSubject, string htmmlEmailBody, string plainTextEmailBody,
                                                       string replyToEmails, string ccEmails)
        {
            // create email message
            MailMessage msg = new MailMessage();


            toEmails = toEmails.Replace(";", ",");

            //remove comma in case there was one by mistake
            toEmails = toEmails.TrimEnd(',');
            
            // add the Recipient Email IDs
            msg.To.Add(toEmails);

            //add reply to addresses if not null
            if (!string.IsNullOrEmpty(replyToEmails))
            {
                replyToEmails = replyToEmails.Replace(";", ",");

                msg.ReplyToList.Add(replyToEmails.TrimEnd(','));
            }

            //add bcc address if not null
            if (!string.IsNullOrEmpty(ccEmails))
            {
                ccEmails = ccEmails.Replace(";", ",");

                msg.CC.Add(ccEmails.TrimEnd(','));
            }

            //add reply to address
            //msg.ReplyToList.Add(new MailAddress(replyToEmail, fromName));

            //setting email subject and body
            msg.Subject = emailSubject;
            //msg.Body = htmmlEmailBody;
            // msg.IsBodyHtml = true;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainTextEmailBody, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmmlEmailBody, null, MediaTypeNames.Text.Html));

            return msg;
        }

        public class Attachment
        {
            public Stream Stream { get; set; }
            public string AttachmentName { get; set; }
            public string ContentType { get; set; }
        }
    }
}