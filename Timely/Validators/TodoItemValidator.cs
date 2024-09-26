namespace Timely.Validators;

using FluentValidation;

using Timely.Models;

/// <summary>
/// A validator for checking property values on existing todo item
/// updates.
/// </summary>
public class TodoItemValidator : ValidatorBase<EditTodoItem>
{
    /// <summary>
    /// Create a TodoItem validator for validating item updates.
    /// </summary>
    public TodoItemValidator() =>
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(1, 100).WithMessage("Description must be between 1 and 100 characters in length");
}
