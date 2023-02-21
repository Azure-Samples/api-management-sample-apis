// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Serialization;
using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// A Star Wars Film
/// </summary>
public class Film : BaseModel, IEquatable<Film>
{
    private const string modelType = "film";

    /// <summary>
    /// Creates a complete episode with all information.
    /// </summary>
    /// <param name="episodeId"></param>
    /// <param name="title"></param>
    /// <param name="openingCrawl"></param>
    /// <param name="director"></param>
    /// <param name="producer"></param>
    /// <param name="releaseDate"></param>
    public Film(int episodeId, string title, string openingCrawl, string director, string producer, DateOnly releaseDate) : base(modelType, episodeId)
    {
        Title = title;
        OpeningCrawl = openingCrawl;
        Director = director;
        Producer = producer;
        ReleaseDate = releaseDate;
    }

    /// <summary>
    /// The title of this film.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The opening crawl text at the beginning of this film.
    /// </summary>
    [JsonPropertyName("opening_crawl")]
    public string OpeningCrawl { get; set; } = string.Empty;

    /// <summary>
    /// The director of this film.
    /// </summary>
    [JsonPropertyName("director")]
    public string Director { get; set; } = string.Empty;

    /// <summary>
    /// The producer(s) of this film.
    /// </summary>
    [JsonPropertyName("producer")]
    public string Producer { get; set; } = string.Empty;

    /// <summary>
    /// The release date at original creator country.
    /// </summary>
    [JsonPropertyName("release_date")]
    public DateOnly ReleaseDate { get; set; } = DateOnly.MinValue;

    /// <summary>
    /// The list of characters features within this film.
    /// </summary>
    [JsonPropertyName("characters")]
    [JsonConverter(typeof(PersonListConverter))]
    public IList<Person> Characters { get; set; } = new List<Person>();

    /// <summary>
    /// The list of planets featured within this film.
    /// </summary>
    [JsonPropertyName("planets")]
    [JsonConverter(typeof(PlanetListConverter))]
    public IList<Planet> Planets { get; set; } = new List<Planet>();

    /// <summary>
    /// The list of species featured within this film.
    /// </summary>
    [JsonPropertyName("species")]
    [JsonConverter(typeof(SpeciesListConverter))]
    public IList<Species> Species { get; set; } = new List<Species>();

    /// <summary>
    /// The list of starships featured within this film.
    /// </summary>
    [JsonPropertyName("starships")]
    [JsonConverter(typeof(StarshipListConverter))]
    public IList<Starship> Starships { get; set; } = new List<Starship>();

    /// <summary>
    /// The list of vehicles featured within this film.
    /// </summary>
    [JsonPropertyName("vehicles")]
    [JsonConverter(typeof(VehicleListConverter))]
    public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    #region IEquatable<Film>
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(Film? other)
        => other != null && other.Id == Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is Film other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id, Title);
    #endregion
}
