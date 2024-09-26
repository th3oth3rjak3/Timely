namespace Timely.Validators;
using System.Collections.Generic;
using System.Linq;

using FluentValidation;

/// <summary>
/// The base validator which contains a validate method.
/// </summary>
public abstract class ValidatorBase<T> : AbstractValidator<T> where T : class
{
    /// <summary>
    /// Validate all of the properties in the model.
    /// </summary>
    public Func<object, string, Task<IEnumerable<string>>> ValidateModel => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid) return [];
        return result.Errors.Select(err => err.ErrorMessage);
    };
}
