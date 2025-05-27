using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService;

public class KafkaMessage
{
    public long Offset { get; set; }
    public int Partition { get; set; }
    public string Topic { get; set; }
    public DateTime Timestamp { get; set; }
    public string Value { get; set; }
    public string Key { get; set; }
    public List<object> Headers { get; set; }

    static async Task SendEmailAsync(string message, string toEmail)
    {
        try
        {
            string smtpHost = Environment.GetEnvironmentVariable("smtpHost");
            int smtpPort = Int32.Parse(Environment.GetEnvironmentVariable("smtpPort"));
            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                string smtpUsername = Environment.GetEnvironmentVariable("smtpUsername");
                string smtpPassword = Environment.GetEnvironmentVariable("smtpPassword");
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("test@bien.pl"),
                    Subject = "Kafka message",
                    Body = message,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"E-mail sent to {toEmail} with message: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while sending e-mail: {ex.Message}");
        }
    }
}
