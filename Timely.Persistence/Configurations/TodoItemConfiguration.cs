namespace Timely.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Timely.Domain.Features.TodoItems;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        _ = builder.HasKey(t => t.Id);

        _ = builder
            .Property(t => t.Description)
            .IsRequired();

        _ = builder
            .Property(t => t.CompleteDate);

        _ = builder
            .ToTable("TodoItems");
    }
}
