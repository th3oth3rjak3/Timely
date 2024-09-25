namespace Timely.Components.Pages;

using System.ComponentModel.DataAnnotations;

using Functional;

using Microsoft.AspNetCore.Components;

using MudBlazor;

using Timely.Components.Dialogs;
using Timely.Domain.Features.TodoItems;

/// <summary>
/// This page shows the list of todo items.
/// </summary>
public partial class TodoItemList
{
    ///<inheritdoc cref="ITodoItemRepository"/>
    [Inject]
    public required ITodoItemRepository TodoItemRepository { get; set; }

    /// <summary>
    /// The snack bar to add error messages.
    /// </summary>
    [Inject]
    public required ISnackbar SnackBar { get; set; }

    /// <summary>
    /// A dialog service for opening modal dialogs.
    /// </summary>
    [Inject]
    public required IDialogService DialogService { get; set; }

    /// <summary>
    /// Which type of items to retrieve, complete, incomplete, or all items.
    /// </summary>
    private TodoItemType ItemType { get; set; } = TodoItemType.All;

    /// <summary>
    /// The list of todo items to show on the page.
    /// </summary>
    private List<TodoItem> todoItems = [];

    /// <summary>
    /// On initialization, get the todo items to show the user.
    /// </summary>
    protected override void OnInitialized() => GetTodoItems();

    /// <summary>
    /// Open a new todo item dialog for a user to enter a new todo task, validate user input, and save valid todo item.
    /// </summary>
    /// <returns>A task to be awaited.</returns>
    private async Task AddNewTodoItem()
    {
        var options = new DialogOptions { BackgroundClass = "dialog-blur", BackdropClick = false, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AddTodoItemDialog>("New Task", options);
        var result = await dialog.Result;

        if (result?.Data is not TodoItem data) return;

        await TodoItemRepository
            .AddTodoItem(data)
            .Effect(
                () => SnackBar.Add($"Added task '{data.Description}'.", Severity.Success),
                err => SnackBar.Add($"Error adding new task: {err}", Severity.Error))
            .Effect(GetTodoItems)
            .Async()
            .EffectAsync(() => InvokeAsync(StateHasChanged));
    }

    /// <summary>
    /// Display a confirmation dialog to mark the item as completed. If confirmed, completed date/time is saved.
    /// </summary>
    /// <param name="item">The todo item to mark completed.</param>
    /// <returns>A task to be awaited.</returns>
    private async Task MarkTodoItemCompleted(TodoItem item)
    {
        var options = new DialogOptions() { BackgroundClass = "dialog-blur", CloseButton = true };
        var dialogParams = new DialogParameters<ConfirmationDialog>
        {
            { x => x.ContentText, $"Mark task '{item.Description}' complete?" },
            { x => x.ConfirmButtonText, "Confirm" },
            { x => x.ConfirmButtonColor, Color.Primary },
        };
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", dialogParams, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        await TodoItemRepository.CompleteTodoItem(item, DateTime.Now)
            .Effect(
                () => SnackBar.Add($"Task '{item.Description}' completed.", Severity.Success),
                err => SnackBar.Add($"Error marking task completed: {err}", Severity.Error))
            .Effect(GetTodoItems)
            .Async()
            .EffectAsync(() => InvokeAsync(StateHasChanged));
    }

    /// <summary>
    /// Get all todo items to display to the user.
    /// </summary>
    private void GetTodoItems() =>
        todoItems = TodoItemRepository.TodoItems(ItemType).OrderBy(x => x.Description).ToList();

    /// <summary>
    /// Delete the todo item after showing the user a confirmation dialog.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    /// <returns>A task to be awaited.</returns>
    private async Task DeleteTodoItem(TodoItem item)
    {
        var options = new DialogOptions() { BackgroundClass = "dialog-blur", CloseButton = true };
        var dialogParams = new DialogParameters<ConfirmationDialog>
        {
            { x => x.ContentText, $"Delete task '{item.Description}'?" },
            { x => x.ConfirmButtonText, "Delete" },
            { x => x.ConfirmButtonColor, Color.Error },
        };
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", dialogParams, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        await TodoItemRepository.DeleteTodoItem(item)
            .Effect(
                () => SnackBar.Add($"Deleted task '{item.Description}'.", Severity.Success),
                err => SnackBar.Add($"Error deleting task: {err}", Severity.Error))
            .Effect(GetTodoItems)
            .Async()
            .EffectAsync(() => InvokeAsync(StateHasChanged));

    }
    private async Task ClearAllCompletedItems()
    {
        var options = new DialogOptions() { BackgroundClass = "dialog-blur", CloseButton = true };
        var dialogParams = new DialogParameters<ConfirmationDialog>
        {
            { x => x.ContentText, "Delete all completed tasks?" },
            { x => x.ConfirmButtonText, "Delete All" },
            { x => x.ConfirmButtonColor, Color.Error },
        };
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", dialogParams, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        if (!todoItems.Where(item => item.CompleteDate is not null).Any())
        {
            SnackBar.Add("No completed tasks to delete.", Severity.Info);
            return;
        }

        await TodoItemRepository
            .ClearFinishedTodoItems()
            .Effect(
                () => SnackBar.Add("Deleted completed tasks.", Severity.Success),
                err => SnackBar.Add($"Error deleting completed tasks: {err}", Severity.Error))
            .Effect(GetTodoItems)
            .Async()
            .EffectAsync(() => InvokeAsync(StateHasChanged));

    }

    private class NewTodoItemForm
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string? Description { get; set; } = null;
    }
}