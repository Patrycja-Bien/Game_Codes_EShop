using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace NotificationService;

public class LoginNotificationEmail
{
    private readonly ILogger<LoginNotificationEmail> _logger;

    public LoginNotificationEmail(ILogger<LoginNotificationEmail> logger)
    {
        _logger = logger;
    }

    [Function("LoginNotificationEmail")]
    public async Task Run(
        [KafkaTrigger(
            "kafka:9092",
            "after-login-email-topic",
            ConsumerGroup = "function-consumer-group")]
        KafkaMessage message)
    {
        await SendEmailAsync("User logged in successfully.", message.Value.ToString());
        _logger.LogInformation($"Messege received from Kafka: {message.ToString()}");
    }

    public static async Task SendEmailAsync(string message, string toEmail)
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
                    From = new MailAddress("test@gmail.com"),
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