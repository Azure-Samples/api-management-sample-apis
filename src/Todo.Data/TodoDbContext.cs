// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace Todo.Data;

/// <summary>
/// The database context for an Azure SQL database.
/// </summary>
public class TodoDbContext : DbContext, IDatabaseInitializer
{
    /// <summary>
    /// Creates a new <see cref="TodoDbContext"/>.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Create the model for the database.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureModel<TodoItem>();
        modelBuilder.ConfigureModel<TodoList>();

        modelBuilder.Entity<TodoItem>()
            .HasOne(m => m.List)
            .WithMany(m => m.Items)
            .HasForeignKey(m => m.ListId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// The data set for the items.
    /// </summary>
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    /// <summary>
    /// The data set for the lists.
    /// </summary>
    public DbSet<TodoList> TodoLists => Set<TodoList>();

    /// <summary>
    /// Initializes the database.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    public async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }
}

internal static class ModelBuilderExtensions
{
    internal static void ConfigureModel<T>(this ModelBuilder modelBuilder) where T : TodoBaseModel
    {
        modelBuilder.Entity<T>().HasKey(m => m.Id);
        modelBuilder.Entity<T>().Property(m => m.CreatedDate).ValueGeneratedOnAdd();
    }
}
