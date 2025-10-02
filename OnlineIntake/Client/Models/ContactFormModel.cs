using OnlineIntake.Client.Resources;
using OnlineIntake.Client.Validation;
using OnlineIntake.Shared.Contracts;
using System.ComponentModel.DataAnnotations;

namespace OnlineIntake.Client.Models;

public class ContactFormModel
{
    [Display(Name = "FirstName", ResourceType = typeof(Labels))]
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    public string FirstName { get; set; } = "";

    [Display(Name = "LastName", ResourceType = typeof(Labels))]
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    public string LastName { get; set; } = "";

    public bool NoBirthNumber { get; set; }

    [Display(Name = "BirthNumber", ResourceType = typeof(Labels))]
    [RequiredIfFalse(nameof(NoBirthNumber),
        ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "BirthNumberRequired")]
    [MaxLength(20)]
    public string? BirthNumber { get; set; }

    [Display(Name = "BirthDate", ResourceType = typeof(Labels))]
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    public DateOnly BirthDate { get; set; }

    [Display(Name = "Gender", ResourceType = typeof(Labels))]
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    public Gender? Gender { get; set; }

    [Display(Name = "Email", ResourceType = typeof(Labels))]
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "EmailAddress")]
    [MaxLength(254)]
    public string Email { get; set; } = "";

    [Display(Name = "Nationality", ResourceType = typeof(Labels))]
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    [MaxLength(100)]
    public string Nationality { get; set; } = "";

    [Display(Name = "GdprConsent", ResourceType = typeof(Labels))]
    [Range(typeof(bool), "true", "true", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Required")]
    public bool GdprConsent { get; set; }

    public CreateContactRequest ToRequest() => new()
    {
        FirstName = FirstName,
        LastName = LastName,
        BirthNumber = NoBirthNumber ? null : BirthNumber,
        BirthDate = BirthDate,
        Gender = Gender ?? OnlineIntake.Shared.Contracts.Gender.Male,
        Email = Email,
        Nationality = Nationality,
        GdprConsent = GdprConsent
    };
}
