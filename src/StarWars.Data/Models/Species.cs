// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Serialization;
using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// A species within the Star Wars Universe
/// </summary>
public class Species : BaseModel
{
    private const string modelType = "species";

    /// <summary>
    /// Creates a new <see cref="Species"/> record.
    /// </summary>
    /// <param name="speciesId">The ID of the species.</param>
    /// <param name="name">The name of this species.</param>
    /// <param name="classification">The classification of this species.</param>
    /// <param name="designation">The designation of this species.</param>
    /// <param name="averageHeight">The average height of this person in centimeters.</param>
    /// <param name="averageLifespan">The average lifespan of this species in years.</param>
    /// <param name="hairColors">A comma-seperated string of common hair colors for this species, none if this species does not typically have hair.</param>
    /// <param name="skinColors">A comma-seperated string of common skin colors for this species</param>
    /// <param name="eyeColors">A comma-seperated string of common eye colors for this species</param>
    /// <param name="language">The language commonly spoken by this species.</param>
    public Species(int speciesId, string name, string classification, string designation, string averageHeight, string averageLifespan, string hairColors, string skinColors, string eyeColors, string language)
        : base(modelType, speciesId)
    {
        Name = name;
        Classification = classification;
        Designation = designation;
        AverageHeight = averageHeight;
        AverageLifespan = averageLifespan;
        HairColors = hairColors;
        SkinColors = skinColors;
        EyeColors = eyeColors;
        Language = language;
    }

    /// <summary>
    /// The name of this species.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The classification of this species.
    /// </summary>
    [JsonPropertyName("classification")]
    public string Classification { get; set; }

    /// <summary>
    /// The designation of this species.
    /// </summary>
    [JsonPropertyName("designation")]
    public string Designation { get; set; }

    /// <summary>
    /// The average height of this species in centimeters.
    /// </summary>
    [JsonPropertyName("average_height")]
    public string AverageHeight { get; set; }

    /// <summary>
    /// The average lifespan of this species in years
    /// </summary>
    [JsonPropertyName("average_lifespan")]
    public string AverageLifespan { get; set; }

    /// <summary>
    /// A comma-separated string of common hair colors for this species.
    /// </summary>
    [JsonPropertyName("hair_colors")]
    public string HairColors { get; set; }

    /// <summary>
    /// A comma-separated string of common skin colors for this species.
    /// </summary>
    [JsonPropertyName("skin_colors")]
    public string SkinColors { get; set; }

    /// <summary>
    /// A comma-separated string of common eye colors for this species.
    /// </summary>
    [JsonPropertyName("eye_colors")]
    public string EyeColors { get; set; }

    /// <summary>
    /// The language spoken by this species.
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; set; }

    /// <summary>
    /// The URL of the planet resource that this species inhavits.
    /// </summary>
    [JsonPropertyName("homeworld")]
    [JsonConverter(typeof(PlanetConverter))]
    public Planet? Homeworld { get; set; }

    /// <summary>
    /// The list of film resources that this person has been in.
    /// </summary>
    [JsonPropertyName("films")]
    [JsonConverter(typeof(FilmListConverter))]
    public IList<Film> Films { get; set; } = new List<Film>();

    #region IEquatable<Species>
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(Species? other)
        => other != null && other.Id == Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is Species other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id, Name);
    #endregion
}
