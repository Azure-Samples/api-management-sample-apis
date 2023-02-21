// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

namespace Todo.Data;

/// <summary>
/// Interface to handle database initialization.
/// </summary>
public interface IDatabaseInitializer
{
    /// <summary>
    /// Initializes the database.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    Task InitializeDatabaseAsync(CancellationToken cancellationToken = default);
}
