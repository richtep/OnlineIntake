using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OnlineIntake.Client.Validation;

public sealed class RequiredIfFalseAttribute : ValidationAttribute
{
    public string BooleanProperty { get; }
    public RequiredIfFalseAttribute(string booleanProperty) => BooleanProperty = booleanProperty;

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        var propertyInfo = context.ObjectType.GetProperty(BooleanProperty, BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo is null) return new ValidationResult($"Unknown property {BooleanProperty}.");

        var flag = (bool)(propertyInfo.GetValue(context.ObjectInstance) ?? false);

        // If not set I have no BirthNumber, BirthNumber is mandatory 
        if (!flag && string.IsNullOrWhiteSpace(Convert.ToString(value)))
            return new ValidationResult(FormatErrorMessage(context.DisplayName));

        return ValidationResult.Success;
    }
}