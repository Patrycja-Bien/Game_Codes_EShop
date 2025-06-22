using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NotificationService;

public class OrderProcessedEmail
{
    private readonly ILogger<LoginNotificationEmail> _logger;

    public OrderProcessedEmail(ILogger<LoginNotificationEmail> logger)
    {
        _logger = logger;
    }

    [Function("OrderProcessedEmail")]
    public async Task Run(
        [KafkaTrigger(
            "kafka:9092",
            "order-processed-email-topic",
            ConsumerGroup = "function-consumer-group")]
        KafkaMessage message)
    {
        Dictionary<string, string> order = (Dictionary<string, string>)JsonConvert.DeserializeObject(message.Value);
        await SendEmailAsync(order.ToString(), order["Email"]);
        _logger.LogInformation($"Messege received from Kafka: {order.ToString()}");
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