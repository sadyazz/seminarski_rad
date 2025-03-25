using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Quartz;
using eReservation.Data;
using Microsoft.EntityFrameworkCore;

public class EmailJob : IJob
{
    private readonly DataContext _db;

    public EmailJob(DataContext db)
    {
        _db = db;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Quartz job triggered.");

        var latestToken = _db.AutentifikacijaToken
            .Include(t => t.korisnickiNalog)
            .OrderByDescending(t => t.vrijemeEvidentiranja)
            .FirstOrDefault();

        if (latestToken == null || latestToken.korisnickiNalog == null)
        {
            Console.WriteLine("No logged-in user found.");
            return;
        }

        var user = latestToken.korisnickiNalog;

        if (string.IsNullOrWhiteSpace(user.Korisnik.Email))
        {
            Console.WriteLine("User's email address is not valid.");
            return;
        }

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("eReservation", "ereservation149@gmail.com"));
        emailMessage.To.Add(new MailboxAddress(user.Username, user.Korisnik.Email));
        emailMessage.Subject = "Exclusive Offer: Book Your Dream Stay with eReservation!";
        emailMessage.Body = new TextPart("html")
        {
            Text = $@"
        <html>
        <body style='font-family: Arial, sans-serif;'>
            <h1 style='color: #2a2a2a;'>Hello {user.Korisnik.Name},</h1>
            <p style='font-size: 16px;'>We have an exclusive offer just for you!</p>
            <p style='font-size: 16px;'>Book your next stay at one of our stunning properties with <strong>eReservation</strong>!</p>

            <p style='font-size: 16px;'>Visit our website for more details.</p>

            <footer style='margin-top: 30px; font-size: 12px; color: #a0a0a0;'>
                <p>&copy; {DateTime.Now.Year} eReservation. All rights reserved.</p>
            </footer>
        </body>
        </html>"
        };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("ereservation149@gmail.com", "");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                Console.WriteLine($"Email sent successfully to {user.Korisnik.Email} at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while sending email: {ex.Message}");
            }
        }
    }
}
