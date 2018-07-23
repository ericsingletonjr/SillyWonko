using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
	public class EmailSender : IEmailSender
	{
		IConfiguration Configuration { get; }

		public EmailSender(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var client = new SendGridClient(Configuration["APIKeys:Api_Key"]);
			var msg = new SendGridMessage();

			msg.SetFrom("admin@sillywonko.com", "Silly Wonko");
			msg.AddTo(email);
			msg.SetSubject(subject);
			msg.AddContent(MimeType.Html, htmlMessage);

			var response = await client.SendEmailAsync(msg);
		}
	}
}
