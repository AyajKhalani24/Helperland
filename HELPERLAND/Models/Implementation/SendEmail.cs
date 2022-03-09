using System.Net.Mail;
using HELPERLAND.Models.ViewModels;

namespace HELPERLAND.Models.Implementation
{
    public class SendEmail
    {
        public Boolean sendMail(EmailViewmodel email)
        {
            try
            {
                MailMessage mm = new MailMessage()
                {
                    From = new MailAddress("chachamehta33@gmail.com")
                };
                mm.To.Add(email.To);
                mm.Subject = email.Subject;
                mm.Body = email.Body;
                mm.IsBodyHtml = email.isHTML;
                if (email.Attachment != null)
                {

                    string fileName = Path.GetFileName(email.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(email.Attachment.OpenReadStream(), fileName));
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("chachamehta33@gmail.com", "Chacha@2114");
                smtp.EnableSsl = true;
                smtp.Send(mm);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}