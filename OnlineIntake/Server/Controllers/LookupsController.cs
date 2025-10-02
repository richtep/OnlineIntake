using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace OnlineIntake.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupsController : ControllerBase
{
    [HttpGet("nationalities")]
    public ActionResult<List<string>> GetNationalities()
    {
        var codes = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new RegionInfo(c.Name))
            .Select(r => r.TwoLetterISORegionName)
            .Where(code => !string.IsNullOrEmpty(code) && code.Length == 2)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(code => code)
            .ToList();

        return Ok(codes);
    }
}