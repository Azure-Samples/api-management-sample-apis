// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

public class Starship : BaseVehicle, IEquatable<Starship>
{
    private const string modelType = "starship";

    /// <summary>
    /// Create a new <see cref="Starship"/> record.
    /// </summary>
    /// <param name="starshipId">The ID of the starship.</param>
    /// <param name="name">The name of the starship.</param>
    /// <param name="starshipClass">The class of this starship.</param>
    /// <param name="model">The model or official name of this starship.</param>
    /// <param name="manufacturer">The manufacturer of this starship. Comma seperated if more than one.</param>
    /// <param name="length">The length of this starship in meters.</param>
    /// <param name="consumables">The maximum length of time that this starship can provide consumables for its entire crew without having to resupply.</param>
    public Starship(int starshipId, string name, string starshipClass, string model, string manufacturer, double length, string consumables)
        : base(modelType, starshipId, name, model, manufacturer, consumables)
    {
        Length = length;
        StarshipClass = starshipClass;
    }

    /// <summary>
    /// Create a new <see cref="Starship"/> record.
    /// </summary>
    /// <param name="starshipId">The ID of the starship.</param>
    /// <param name="name">The name of the starship.</param>
    /// <param name="starshipClass">The class of this starship.</param>
    /// <param name="model">The model or official name of this starship.</param>
    /// <param name="manufacturer">The manufacturer of this starship. Comma seperated if more than one.</param>
    /// <param name="cost">The cost of this starship new, in galactic credits.</param>
    /// <param name="length">The length of this starship in meters.</param>
    /// <param name="speed">The maximum speed of this starship in atmosphere. n/a if this starship is incapable of atmosphering flight.</param>
    /// <param name="crew">The number of personnel needed to run or pilot this starship.</param>
    /// <param name="passengers">The number of non-essential people this starship can transport.</param>
    /// <param name="cargoCapacity">The maximum number of kilograms that this starship can transport.</param>
    /// <param name="consumables">The maximum length of time that this starship can provide consumables for its entire crew without having to resupply.</param>
    /// <param name="hyperdriveRating">The class of this starships hyperdrive.</param>
    /// <param name="mglt">The Maximum number of Megalights this starship can travel in a standard hour.</param>
    public Starship(int starshipId, string name, string starshipClass, string model, string manufacturer, long? cost, double length,
        int? speed, int? crew, int? passengers, long? cargoCapacity, string consumables, double? hyperdriveRating, int? mglt)
        : this(starshipId, name, starshipClass, model, manufacturer, length, consumables)
    {
        CostInCredits = cost;
        MaxAtmospheringSpeed = speed;
        Crew = crew;
        Passengers = passengers;
        CargoCapacity = cargoCapacity;
        HyperdriveRating = hyperdriveRating;
        MegalightsPerHour = mglt;
    }

    /// <summary>
    /// The class of this starship, such as Starfighter or Deep Space Mobile Battlestation.
    /// </summary>
    [JsonPropertyName("starship_class")]
    public string StarshipClass { get; set; }

    /// <summary>
    /// The class of this starships hyperdrive.
    /// </summary>
    [JsonPropertyName("hyperdrive_rating")]
    public double? HyperdriveRating { get; set; }

    /// <summary>
    /// The Maximum number of Megalights this starship can travel in a standard hour. A Megalight is a standard unit of distance and
    /// has never been defined before within the Star Wars universe.  This figure is only really useful for measuring the difference
    /// in speed of starships. We can assume it is similar to AU, the distance between our Sun (Sol) and Earth.
    /// </summary>
    [JsonPropertyName("MGLT")]
    public int? MegalightsPerHour { get; set; }

    #region IEquatable<Starship>
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(Starship? other)
        => other != null && other.Id == Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is Starship other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id, Name);
    #endregion
}
