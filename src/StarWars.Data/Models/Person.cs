// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Serialization;
using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// The gender of a character in the film.
/// </summary>
public enum Gender
{
    Unknown,
    None,
    Hermaphrodite,
    Male,
    Female
}

/// <summary>
/// A person in the Star Wars universe.
/// </summary>
public class Person : BaseModel, IEquatable<Person>
{
    private const string modelType = "person";

    /// <summary>
    /// Creates a new <see cref="Person"/> record.
    /// </summary>
    /// <param name="personId">The ID of the person.</param>
    /// <param name="name">The name of this person.</param>
    /// <param name="hairColor">The hair color of this person.</param>
    /// <param name="skinColor">The skin color of this person.</param>
    /// <param name="eyeColor">The eye color of this person.</param>
    /// <param name="gender">The gender of this person (if known).</param>
    public Person(int personId, string name, string hairColor, string skinColor, string eyeColor, Gender gender) : base(modelType, personId)
    {
        Name = name;
        HairColor = hairColor;
        SkinColor = skinColor;
        EyeColor = eyeColor;
        Gender = gender;
    }

    /// <summary>
    /// Creates a new <see cref="Person"/> record.
    /// </summary>
    /// <param name="personId">The ID of the person.</param>
    /// <param name="name">The name of this person.</param>
    /// <param name="height">The height of this person in meters.</param>
    /// <param name="mass">The mass of this person in kilograms.</param>
    /// <param name="hairColor">The hair color of this person.</param>
    /// <param name="skinColor">The skin color of this person.</param>
    /// <param name="eyeColor">The eye color of this person.</param>
    /// <param name="birthYear">The birth year of this person (relative to the Battle of Yavin), or <c>null</c> if not known</param>
    /// <param name="gender">The gender of this person (if known).</param>
    public Person(int personId, string name, int? height, int? mass, string hairColor, string skinColor, string eyeColor, int? birthYear, Gender gender) 
        : this(personId, name, hairColor, skinColor, eyeColor, gender)
    {
        Height = height;
        Mass = mass;
        BirthYear = birthYear;
    }

    /// <summary>
    /// The name of the person.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The height of the person in meters.
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <summary>
    /// The mass of the person in kilograms.
    /// </summary>
    [JsonPropertyName("mass")]
    public int? Mass { get; set; }

    /// <summary>
    /// The hair color of the person.
    /// </summary>
    [JsonPropertyName("hair_color")]
    public string HairColor { get; set; } = string.Empty;

    /// <summary>
    /// The skin color of the person.
    /// </summary>
    [JsonPropertyName("skin_color")]
    public string SkinColor { get; set; } = string.Empty;

    /// <summary>
    /// The eye color of the person.
    /// </summary>
    [JsonPropertyName("eye_color")]
    public string EyeColor { get; set; } = string.Empty;

    /// <summary>
    /// The birth year of the person. relative to the Battle of Yavin
    /// </summary>
    [JsonPropertyName("birth_year")]
    public int? BirthYear { get; set; }

    /// <summary>
    /// The gender of the person (if known).
    /// </summary>
    [JsonPropertyName("gender")]
    public Gender Gender { get; set; } = Gender.Unknown;

    /// <summary>
    /// The URL of the planet resource that this person was born on.
    /// </summary>
    [JsonPropertyName("homeworld")]
    [JsonConverter(typeof(PlanetConverter))]
    public Planet? Homeworld { get; set; }

    /// <summary>
    /// The URL of the species resource that this person is.
    /// </summary>
    [JsonPropertyName("species")]
    [JsonConverter(typeof(SpeciesConverter))]
    public Species? Species { get; set; }

    /// <summary>
    /// The list of film resources that this person has been in.
    /// </summary>
    [JsonPropertyName("films")]
    [JsonConverter(typeof(FilmListConverter))]
    public IList<Film> Films { get; set; } = new List<Film>();

    /// <summary>
    /// The list of starship resources that this person has piloted.
    /// </summary>
    [JsonPropertyName("starships")]
    [JsonConverter(typeof(StarshipListConverter))]
    public IList<Starship> Starships { get; set; } = new List<Starship>();

    /// <summary>
    /// The list of vehicle resources that this person has piloted.
    /// </summary>
    [JsonPropertyName("vehicles")]
    [JsonConverter(typeof(VehicleListConverter))]
    public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    #region IEquatable<Person>
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(Person? other)
        => other != null && other.Id == Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is Person other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id, Name);
    #endregion
}
