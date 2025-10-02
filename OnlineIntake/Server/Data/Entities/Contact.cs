using OnlineIntake.Shared.Contracts;

namespace OnlineIntake.Server.Data.Entities;
public class Contact
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? BirthNumber { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public bool GdprConsent { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}