using ListaSpesaWish.EF;
using ListaSpesaWish.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ListaSpesaWish.Service
{
    public static class EmailService
    {
        public static async Task<bool> SendAsync(ListaSpesaDto listaSpesa)
        {
            SmtpClient client = null;
            bool result = false;
            try
            {
                EmailDto message = await CreateBody(listaSpesa);

                // Credentials:
                var credentialUserName = ConfigurationManager.AppSettings["emailFrom"];
                var sentFrom = ConfigurationManager.AppSettings["emailFrom"];
                var pwd = ConfigurationManager.AppSettings["emailPassword"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);

                // Configure the client:
                client = new SmtpClient(ConfigurationManager.AppSettings["smtpClient"])
                {
                    Port = port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpEnableSSL"]),
                    Credentials = new NetworkCredential(credentialUserName, pwd)
                };

                AlternateView av = AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html);

                // Create the message:
                MailMessage mail = new MailMessage()
                {
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    From = new MailAddress(ConfigurationManager.AppSettings["emailFrom"], "WishDays"),
                    Subject = message.Subject
                };
                mail.AlternateViews.Add(av);
                (await GetAddresses(message.Destination)).ToList().ForEach(m => { mail.To.Add(m); });                
                mail.From = new MailAddress(sentFrom);
                mail.Subject = message.Subject;

                // Send:
                await client.SendMailAsync(mail);
                result = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }

        public static async Task<EmailDto> CreateBody(ListaSpesaDto listaSpesa)
        {
            StringBuilder bodyMail = new StringBuilder();
            // Caricare il testo della mail e riempire i dati
            string mailText = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Service/mailTemplate.html"));
            mailText = mailText.Replace("{USER}", await CreateUserRows(listaSpesa));
            mailText = mailText.Replace("{VOCE}", await CreateVociRows(listaSpesa));

            return new EmailDto()
            {
                Body = mailText,
                Destination = listaSpesa.Utenti.Select(x => x.Email).Aggregate((current, next) => String.Concat(current, ";", next)),
                Subject = String.Format("Invio lista della spesa {0}", listaSpesa.Nome)
            };
        }

        private static async Task<string> CreateUserRows(ListaSpesaDto listaSpesa)
        {
            return await Task.Run(() =>
            {
                StringBuilder rows = new StringBuilder();
                foreach (Utente u in listaSpesa.Utenti)
                {
                    rows.AppendFormat("<tr><td>{0}</td></tr>", u.Username);
                }
                return rows.ToString();
            });
        }

        private static async Task<string> CreateVociRows(ListaSpesaDto listaSpesa)
        {
            return await Task.Run(() =>
            {
                StringBuilder rows = new StringBuilder();
                foreach (VoceDto v in listaSpesa.Voci)
                {
                    rows.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", v.Name, v.Comprata? "Si":"No");
                }
                return rows.ToString();
            });
        }

        private static async Task<MailAddressCollection> GetAddresses(string addresses)
        {
            MailAddressCollection addressList = new MailAddressCollection();
            return await Task.Run(() =>
            {
                addresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                     .ToList()
                     .ForEach(address =>
                     {
                         addressList.Add(new MailAddress(address));
                     });

                return addressList;
            });
        }
    } 
}