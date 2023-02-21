// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Todo.Data;

public abstract class TodoBaseModel
{
    public TodoBaseModel(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The ID of the entity.
    /// </summary>
    [Key]
    public Guid? Id { get; set; }

    /// <summary>
    /// The name of the entity
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the entity
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The date that the entity was created.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// The date that the entity was last updated.
    /// </summary>
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;
}
