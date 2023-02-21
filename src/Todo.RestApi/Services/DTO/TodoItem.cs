// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using Todo.Data;

namespace Todo.RestApi.Services.DTO;

public class TodoItem
{
    [Required]
    public Guid? Id { get; set; }
    [Required]
    public Guid? ListId { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public TodoItemState State { get; set; } = TodoItemState.Todo;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }
    [Required]
    public DateTimeOffset? CreatedDate { get; set; }
    [Required]
    public DateTimeOffset? UpdatedDate { get; set; }
}
