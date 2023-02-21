// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// The common properties for all the models.
/// </summary>
public abstract class BaseModel
{
    /// <summary>
    /// Creates a new model, based on a type and ID.
    /// </summary>
    /// <param name="modelType">The type of the model.</param>
    /// <param name="id">The ID of the model.</param>
    protected BaseModel(string modelType, int id)
    {
        ModelType = modelType;
        Id = id;
    }

    /// <summary>
    /// The model type (film, character, etc.)
    /// </summary>
    [JsonIgnore]
    public string ModelType { get; }

    /// <summary>
    /// The ID of the model.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; }
}
