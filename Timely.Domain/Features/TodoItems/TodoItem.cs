namespace Timely.Domain.Features.TodoItems;
using System;

/// <summary>
/// A representation of work the user wants to accomplish.
/// </summary>
public record TodoItem
{
    /// <summary>
    /// Create a new todo item with the given description.
    /// </summary>
    /// <param name="description">The description of the work to do.</param>
    public TodoItem(string description) =>
        Description = description;

    /// <summary>
    /// The persistent id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The description of the work to be done.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The date the work was finished, null if not complete.
    /// </summary>
    public DateTime? CompleteDate { get; set; }
}
