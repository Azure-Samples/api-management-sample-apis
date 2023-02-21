// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Serialization;
using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// A planet in the Star Wars universe.
/// </summary>
public class Planet : BaseModel, IEquatable<Planet>
{
    private const string modelType = "planet";

    /// <summary>
    /// Creates a new <see cref="Planet"/> object.
    /// </summary>
    /// <param name="planetId">The ID of the planet.</param>
    /// <param name="name">The name of this planet.</param>
    /// <param name="climate">The climate of this planet. Comma-seperated if diverse.</param>
    /// <param name="terrain">The terrain of this planet. Comma-seperated if diverse.</param>
    public Planet(int planetId, string name, string climate, string terrain) : base(modelType, planetId)
    {
        Name = name;
        Climate = climate;
        Terrain = terrain;
    }

    /// <summary>
    /// Creates a new <see cref="Planet"/> object.
    /// </summary>
    /// <param name="planetId">The ID of the planet.</param>
    /// <param name="name">The name of this planet.</param>
    /// <param name="diameter">The diameter of this planet in kilometers.</param>
    /// <param name="rotationPeriod">The number of standard hours it takes for this planet to complete a single rotation on its axis.</param>
    /// <param name="orbitalPeriod">The number of standard days it takes for this planet to complete a single orbit of its local star.</param>
    /// <param name="gravity">A number denoting the gravity of this planet. Where 1 is normal.</param>
    /// <param name="population">The average population of sentient beings inhabiting this planet.</param>
    /// <param name="climate">The climate of this planet. Comma-seperated if diverse.</param>
    /// <param name="terrain">The terrain of this planet. Comma-seperated if diverse.</param>
    /// <param name="surfaceWater">The percentage of the planet surface that is naturally occuring water or bodies of water.</param>
    public Planet(int planetId, string name, int? diameter, int? rotationPeriod, int? orbitalPeriod, double? gravity, long? population, string climate, string terrain, double? surfaceWater)
        : this(planetId, name, climate, terrain)
    {
        Diameter = diameter;
        RotationPeriod = rotationPeriod;
        OrbitalPeriod = orbitalPeriod;
        Gravity = gravity;
        Population = population;
        SurfaceWater = surfaceWater;
    }

    /// <summary>
    /// The name of this planet.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The diameter of this planet in kilometers.
    /// </summary>
    [JsonPropertyName("diameter")]
    public int? Diameter { get; set; }

    /// <summary>
    /// The number of standard hours it takes for this planet to complete a single rotation on its axis.
    /// </summary>
    [JsonPropertyName("rotation_period")]
    public int? RotationPeriod { get; set; }

    /// <summary>
    /// The number of standard days it takes for this planet to complete a single orbit of its local star.
    /// </summary>
    [JsonPropertyName("orbital_period")]
    public int? OrbitalPeriod { get; set; }

    /// <summary>
    /// A number denoting the gravity of this planet. Where 1 is normal.
    /// </summary>
    [JsonPropertyName("gravity")]
    public double? Gravity { get; set; }

    /// <summary>
    /// The average population of sentient beings inhabiting this planet.
    /// </summary>
    [JsonPropertyName("population")]
    public long? Population { get; set; }

    /// <summary>
    /// The climate of this planet. Comma-seperated if diverse.
    /// </summary>
    [JsonPropertyName("climate")]
    public string Climate { get; set; }

    /// <summary>
    /// The terrain of this planet. Comma-seperated if diverse.
    /// </summary>
    [JsonPropertyName("terrain")]
    public string Terrain { get; set; }

    /// <summary>
    /// The percentage of the planet surface that is naturally occuring water or bodies of water.
    /// </summary>
    [JsonPropertyName("surface_water")]
    public double? SurfaceWater { get; set; }

    /// <summary>
    /// An array of Film URL Resources that this planet has appeared in.
    /// </summary>
    [JsonPropertyName("films")]
    [JsonConverter(typeof(FilmListConverter))]
    public IList<Film> Films { get; set; } = new List<Film>();

    /// <summary>
    /// An array of People URL Resources that live on this planet.
    /// </summary>
    [JsonPropertyName("residents")]
    [JsonConverter(typeof(PersonListConverter))]
    public IList<Person> Residents { get; set; } = new List<Person>();

    #region IEquatable<Planet>
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(Planet? other)
        => other != null && other.Id == Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is Planet other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id, Name);
    #endregion
}
