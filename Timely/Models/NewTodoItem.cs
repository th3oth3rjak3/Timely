namespace Timely.Models;

/// <summary>
/// The model for creating a new todo item.
/// </summary>
public class NewTodoItem
{
    /// <summary>
    /// The description of the task to be done.
    /// </summary>
    public string? Description { get; set; } = null;
}