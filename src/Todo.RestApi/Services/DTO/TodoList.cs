// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Todo.RestApi.Services.DTO;

public class TodoList
{
    [Required]
    public Guid? Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public DateTimeOffset? CreatedDate { get; set; }
    [Required]
    public DateTimeOffset? UpdatedDate { get; set; }
}
