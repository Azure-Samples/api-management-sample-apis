// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// A vehicle.
/// </summary>
public class Vehicle : BaseVehicle, IEquatable<Vehicle>
{
    private const string modelType = "vehicle";

    /// <summary>
    /// Creates a new <see cref="Vehicle"/> object.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle.</param>
    /// <param name="name">The name of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="vehicleClass">The class of this vehicle.</param>
    /// <param name="manufacturer">The manufacturer of the vehicle.</param>
    /// <param name="consumables">The maximum length of time that this starship can provide consumables for its entire crew without having to resupply.</param>
    public Vehicle(int vehicleId, string name, string model, string vehicleClass, string manufacturer, string consumables)
        : base(modelType, vehicleId, name, model, manufacturer, consumables)
    {
        VehicleClass = vehicleClass;
    }

    /// <summary>
    /// Creates a new <see cref="Vehicle"/> object.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle.</param>
    /// <param name="name">The name of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="vehicleClass">The class of this vehicle.</param>
    /// <param name="manufacturer">The manufacturer of the vehicle.</param>
    /// <param name="cost">The cost of this starship new, in galactic credits.</param>
    /// <param name="length">The length of this starship in meters.</param>
    /// <param name="crew">The number of personnel needed to run or pilot this starship.</param>
    /// <param name="passengers">The number of non-essential people this starship can transport.</param>
    /// <param name="speed">The maximum speed of this starship in atmosphere. n/a if this starship is incapable of atmosphering flight.</param>
    /// <param name="cargoCapacity">The maximum number of kilograms that this starship can transport.</param>
    /// <param name="consumables">The maximum length of time that this starship can provide consumables for its entire crew without having to resupply.</param>
    public Vehicle(int vehicleId, string name, string model, string vehicleClass, string manufacturer, long? cost, double? length, int? crew, int? passengers, int? speed, long? cargoCapacity, string consumables)
        : this(vehicleId, name, model, vehicleClass, manufacturer, consumables)
    {
        CostInCredits = cost;
        Length = length;
        Crew = crew;
        Passengers = passengers;
        MaxAtmospheringSpeed = speed;
        CargoCapacity = cargoCapacity;
    }

    /// <summary>
    /// The class of this vehicle, such as Wheeled.
    /// </summary>
    [JsonPropertyName("vehicle_class")]
    public string VehicleClass { get; set; }

    #region IEquatable<Vehicle>
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(Vehicle? other)
        => other != null && other.Id == Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is Vehicle other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id, Name);
    #endregion
}