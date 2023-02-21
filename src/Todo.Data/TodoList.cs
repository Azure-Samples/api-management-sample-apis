// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

namespace Todo.Data;

public class TodoList : TodoBaseModel
{
    public TodoList(string name) : base(name)
    {
    }

    /// <summary>
    /// The list of items in the dataset.
    /// </summary>
    public List<TodoItem>? Items { get; set; }
}
