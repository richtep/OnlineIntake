using OnlineIntake.Shared.Contracts;
using System.Globalization;

namespace OnlineIntake.Shared.Validation;

public static class CzechBirthNumber
{
    /// <summary>
    /// Try parse Czech birth number (rodné číslo).
    /// </summary>
    public static bool TryParseBirthNumber(string? birthNumberRaw, out DateOnly date, out Gender? gender)
    {
        date = default;
        gender = null;

        if (string.IsNullOrWhiteSpace(birthNumberRaw)) return false;

        var birthNumber = birthNumberRaw.Replace("/", "").Trim();
        if (birthNumber.Length is not (9 or 10)) return false;
        if (birthNumber.Any(t => t is < '0' or > '9'))
        {
            return false;
        }


        var yy = Parse2(birthNumber.AsSpan(0, 2));
        var mm = Parse2(birthNumber.AsSpan(2, 2));
        var dd = Parse2(birthNumber.AsSpan(4, 2));

        // month + gender
        // women have month +50; additional after 2004 +20 (i.e. +70), men have +0 or additional +20
        var g = Gender.Male;
        switch (mm)
        {
            case >= 70:
                mm -= 70; g = Gender.Female;
                break;
            case >= 50:
                mm -= 50; g = Gender.Female;
                break;
            case >= 20:
                mm -= 20;
                break;
        }

        //9 digits(until 1953 included), 10 digits(from 1954)
        var year = (birthNumber.Length == 9) ? 1900 + yy : (yy >= 54 ? 1900 + yy : 2000 + yy);

        // checksum for 10 digits
        // (first 9 digits) mod 11; 10 -> 0
        if (birthNumber.Length == 10)
        {
            var first9 = long.Parse(birthNumber.AsSpan(0, 9), NumberStyles.None, CultureInfo.InvariantCulture);
            var mod = (int)(first9 % 11);
            var expectedCheck = (mod == 10) ? 0 : mod;
            var actualCheck = birthNumber[9] - '0'; //takes real 10th digit
            if (expectedCheck != actualCheck) return false;
        }

        // calendar date check
        if (mm is < 1 or > 12) return false;
        try
        {
            date = new DateOnly(year, mm, dd);
        }
        catch
        {
            return false;
        }

        gender = g;
        return true;

        static int Parse2(ReadOnlySpan<char> s)
            => (s[0] - '0') * 10 + (s[1] - '0');
    }
}
