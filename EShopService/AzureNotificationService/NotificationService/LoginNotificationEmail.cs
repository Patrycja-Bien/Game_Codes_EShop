using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions;
using System.Net.Mail;
using System.Net;

namespace NotificationService;

public class LoginNotificationEmail
{
    private readonly ILogger<LoginNotificationEmail> _logger;

    public LoginNotificationEmail(ILogger<LoginNotificationEmail> logger)
    {
        _logger = logger;
    }

    [Function(nameof(LoginNotificationEmail))]
    public void Run(
        [KafkaTrigger(
            "localhost:7156",
            "after-login-email-topic",
            ConsumerGroup = "function-consumer-group")] 
        KafkaMessage message)
    {
        _logger.LogInformation($"Messege received from Kafka: {message}");
    }
}