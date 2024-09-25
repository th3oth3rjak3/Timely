namespace Timely.Persistence.Repositories;
using System.Collections.Generic;

using Functional;

using Timely.Domain.Features.TodoItems;

using static Functional.Prelude;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly TimelyContext _context;

    /// <summary>
    /// Create a new todo item repository to manage todo item storage and retrieval.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TodoItemRepository(TimelyContext context) => _context = context;

    /// <inheritdoc />
    public List<TodoItem> TodoItems(TodoItemType type) =>
        type switch
        {
            TodoItemType.Unfinished => UnfinishedTodoItems,
            TodoItemType.Finished => FinishedTodoItems,
            TodoItemType.All => AllTodoItems,
            _ => throw new NotImplementedException()
        };

    /// <inheritdoc />
    private List<TodoItem> UnfinishedTodoItems =>
        _context
            .TodoItems
            .Where(item => item.CompleteDate == null)
            .ToList();

    /// <inheritdoc />
    private List<TodoItem> FinishedTodoItems =>
        _context
            .TodoItems
            .Where(item => item.CompleteDate != null)
            .ToList();

    /// <inheritdoc />
    private List<TodoItem> AllTodoItems =>
        _context
            .TodoItems
            .ToList();

    /// <inheritdoc />
    public Result<TodoItem, string> AddTodoItem(TodoItem newTodoItem) =>
        Try(() =>
        {
            _context.TodoItems.Add(newTodoItem);
            _context.SaveChanges();
            return newTodoItem;
        })
        .MapError(exn => exn.Message);

    /// <inheritdoc />
    public Result<Unit, string> ClearFinishedTodoItems() =>
        Try(() =>
        {
            var finishedItems = FinishedTodoItems.ToList();
            _context.TodoItems.RemoveRange(finishedItems);
            _context.SaveChanges();
        })
        .MapError(exn => exn.Message);

    /// <inheritdoc />
    public Result<TodoItem, string> CompleteTodoItem(TodoItem finishedTodoItem, DateTime completeDate) =>
        Try(() =>
        {
            finishedTodoItem.CompleteDate = completeDate;
            _context.TodoItems.Update(finishedTodoItem);
            _context.SaveChanges();
            return finishedTodoItem;
        })
        .MapError(exn => exn.Message);

    /// <inheritdoc />
    public Result<Unit, string> DeleteTodoItem(TodoItem todoItem) =>
        Try(() =>
        {
            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
        })
        .MapError(exn => exn.Message);
}
