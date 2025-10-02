using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineIntake.Server.Data;
using OnlineIntake.Server.Data.Entities;
using OnlineIntake.Shared.Contracts;

namespace OnlineIntake.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ContactDto>> Create([FromBody] CreateContactRequest createContactRequest)
    {
        if (!createContactRequest.GdprConsent)
            return BadRequest(new { message = "GDPR consent is required." });
        
        // If valid BirthNumber exists, fill BirthDate and Gender from it
        if (!string.IsNullOrWhiteSpace(createContactRequest.BirthNumber))
        {
            var parsed = Shared.Validation.CzechBirthNumber.TryParseBirthNumber(createContactRequest.BirthNumber!, out var birthDate, out var gender);
            if (!parsed)
                return BadRequest(new { message = "Invalid birth number." });
            
            createContactRequest.BirthDate = birthDate == default ? createContactRequest.BirthDate : birthDate;
            createContactRequest.Gender = gender ?? createContactRequest.Gender;
        }
        
        var entity = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = createContactRequest.FirstName.Trim(),
            LastName = createContactRequest.LastName.Trim(),
            BirthNumber = string.IsNullOrWhiteSpace(createContactRequest.BirthNumber) ? null : createContactRequest.BirthNumber,
            BirthDate = createContactRequest.BirthDate,
            Gender = createContactRequest.Gender,
            Email = createContactRequest.Email.Trim(),
            Nationality = createContactRequest.Nationality.Trim(),
            GdprConsent = createContactRequest.GdprConsent,
            CreatedAtUtc = DateTime.UtcNow
        };
        
        db.Contacts.Add(entity);
        await db.SaveChangesAsync();
        
        var dto = new ContactDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            BirthNumber = entity.BirthNumber,
            BirthDate = entity.BirthDate,
            Gender = entity.Gender,
            Email = entity.Email,
            Nationality = entity.Nationality,
            GdprConsent = entity.GdprConsent,
            CreatedAtUtc = entity.CreatedAtUtc
        };
        
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContactDto>> GetById(Guid id)
    {
        var contact = await db.Contacts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (contact is null) return NotFound();
        return new ContactDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            BirthNumber = contact.BirthNumber,
            BirthDate = contact.BirthDate,
            Gender = contact.Gender,
            Email = contact.Email,
            Nationality = contact.Nationality,
            GdprConsent = contact.GdprConsent,
            CreatedAtUtc = contact.CreatedAtUtc
        };
    }
}
