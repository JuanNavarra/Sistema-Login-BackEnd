namespace Services
{
    using Dtos;
    using System.Net;
    using System.Net.Mail;

    public static class MessageSender
    {

        #region Methods

        /// <summary>
        /// Envia un email a un usuario
        /// </summary>
        /// <param name="mailDto"></param>
        /// <param name="emailBody"></param>
        public static void SendEmail(SendEMailDto mailDto, EmailBodyEnum emailBody)
        {
            switch (emailBody)
            {
                case EmailBodyEnum.confirmationEmail:
                    mailDto.Body = TemplateHtml.GenerateTemplateConfirmation(mailDto.Body);
                    break;
                case EmailBodyEnum.recoverPassEmail:
                    mailDto.Body = TemplateHtml.GenerateTemplateRecoverPass(mailDto.Body);
                    break;
                default:
                    break;
            }
            MailMessage mailMessage = new MailMessage(mailDto.EmailFrom, mailDto.EmailTo, mailDto.Subject, mailDto.Body)
            {
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailDto.EmailFrom, mailDto.PasswordFrom);
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
            smtpClient.Dispose();
        }
        #endregion
    }
}
