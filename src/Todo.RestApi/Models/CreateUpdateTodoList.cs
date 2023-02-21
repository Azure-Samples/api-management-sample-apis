// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

namespace Todo.RestApi.Models;

/// <summary>
/// The input record for updating or creating a TodoList.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
public record CreateUpdateTodoList(
    string Name,
    string? Description = null
);