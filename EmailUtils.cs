using System;
using System.Net;
using System.Net.Mail;

namespace ChatterBoxServerNEW
{
    class EmailUtils
    {
        private static int n = 0;
        public static void send(string username, string email, ClientObject client)
        {
            Random r = new Random();
            n = r.Next(100000, 999999);
            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(email);
                msg.From = new MailAddress("mail.showy@gmail.com");
                msg.Subject = ("Здравствуйте, " + username + ". " + "Благодарим за регистрацию в нашем мессенджере!");
                msg.Body = ("Ваш код для завершения регистрации: " + n);
                SmtpClient smt = new SmtpClient();
                smt.Host = "smtp.gmail.com";
                NetworkCredential ntcd = new NetworkCredential();
                ntcd.UserName = "mail.showy@gmail.com";
                ntcd.Password = "K@8Fe9tgm39Qz6s6$ZsM^R^CIvgGx!Fm";
                smt.Credentials = ntcd;
                smt.EnableSsl = true;
                smt.Port = 587;
                smt.Send(msg);
                Handlers.RegistrationHandler.sendRegistrationPackets(n, client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        
    }
}
