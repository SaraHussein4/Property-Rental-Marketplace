using PropertyRentalDAL.Models;
using System.Net;
using System.Net.Mail;

namespace PropertyRentalMarketplace.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("saraomran8122@gmail.com", "sjjsjgytyolsrynv");
            Client.Send("saraomran8122@gmail.com", email.To, email.Subject, email.Body);
        }
            
    }
}
