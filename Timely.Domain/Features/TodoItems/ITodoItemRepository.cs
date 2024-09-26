namespace Timely.Domain.Features.TodoItems;
using System.Collections.Generic;

using Functional;

/// <summary>
/// A repository that manages TodoItems.
/// </summary>
public interface ITodoItemRepository
{
    /// <summary>
    /// Get a list of unfinished todo items.
    /// </summary>
    public List<TodoItem> TodoItems(TodoItemType type);

    /// <summary>
    /// Add a new todo item.
    /// </summary>
    /// <param name="newTodoItem">The new todo item to add.</param>
    /// <returns>The newly created todo item.</returns>
    public Result<TodoItem, string> AddTodoItem(TodoItem newTodoItem);

    /// <summary>
    /// Mark a todo item as completed.
    /// </summary>
    /// <param name="finishedTodoItem">The todo item that is finished.</param>
    /// <param name="completeDate">When the item was finished.</param>
    /// <returns>The completed todo item.</returns>
    public Result<TodoItem, string> CompleteTodoItem(TodoItem finishedTodoItem, DateTime completeDate);

    /// <summary>
    /// Clear all of the finished todo items out of the database.
    /// </summary>
    public Result<Unit, string> ClearFinishedTodoItems();

    /// <summary>
    /// Delete an unfinished todo item from the database.
    /// </summary>
    /// <param name="todoItem">The todo item to remove.</param>
    public Result<Unit, string> DeleteTodoItem(TodoItem todoItem);

    /// <summary>
    /// Update a todo item in the database.
    /// </summary>
    /// <param name="todoItem">The contents of the new todo item to be updated.</param>
    /// <returns>The updated todo item or an error message.</returns>
    public Result<TodoItem, string> UpdateTodoItem(TodoItem todoItem);
}
