﻿using B3Digitas.Architecture.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace B3Digitas.Architecture.Infrastructure;

public class FakeEmailSender : IEmailSender
{
  private readonly ILogger<FakeEmailSender> _logger;

  public FakeEmailSender(ILogger<FakeEmailSender> logger)
  {
    _logger = logger;
  }
  public Task SendEmailAsync(string to, string from, string subject, string body)
  {
    _logger.LogInformation("Not actually sending an email to {to} from {from} with subject {subject}", to, from, subject);
    return Task.CompletedTask;
  }
}