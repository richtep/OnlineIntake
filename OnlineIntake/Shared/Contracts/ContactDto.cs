using System.ComponentModel.DataAnnotations;

namespace OnlineIntake.Shared.Contracts;

public sealed class ContactDto
{
    public Guid Id { get; set; }
    
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? BirthNumber { get; set; }

    [Required]
    public DateOnly BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Required, EmailAddress, MaxLength(254)]
    public string Email { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string Nationality { get; set; } = string.Empty;

    [Required]
    public bool GdprConsent { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
}