namespace Timely.Validators;

using FluentValidation;

using Timely.Models;

/// <summary>
/// A fluent validator for creating new todo items.
/// </summary>
public class NewTodoItemValidator : ValidatorBase<NewTodoItem>
{
    /// <summary>
    /// Create a NewTodoItem validator to check for valid inputs.
    /// </summary>
    public NewTodoItemValidator() =>
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(1, 100).WithMessage("Description must be between 1 and 100 characters in length.");
}
