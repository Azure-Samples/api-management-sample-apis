// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Serialization;
using System.Text.Json.Serialization;

namespace StarWars.Data.Models;

/// <summary>
/// A base set of features for a vehicle.
/// </summary>
public abstract class BaseVehicle : BaseModel
{
    /// <summary>
    /// Creates a new <see cref="VehicleBase"/> object.
    /// </summary>
    /// <param name="modelType">The model type.</param>
    /// <param name="id">The ID of the vehicle.</param>
    /// <param name="name">The name of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="manufacturer">The manufacturer of the vehicle.</param>
    /// <param name="consumables">The maximum length of time that this starship can provide consumables for its entire crew without having to resupply.</param>
    protected BaseVehicle(string modelType, int id, string name, string model, string manufacturer, string consumables)
        : base(modelType, id)
    {
        Name = name;
        Model = model;
        Manufacturer = manufacturer;
        Consumables = consumables;
    }

    /// <summary>
    /// The name of the vehicle.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The model or official name of this vehicle.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    /// The manufacturer of this vehicle.
    /// </summary>
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }

    /// <summary>
    /// The cost of this starship new, in galactic credits.
    /// </summary>
    [JsonPropertyName("cost_in_credits")]
    public long? CostInCredits { get; set; }

    /// <summary>
    /// The length of this starship in meters.
    /// </summary>
    [JsonPropertyName("length")]
    public double? Length { get; set; }

    /// <summary>
    /// The maximum speed of this starship in atmosphere. n/a if this starship is incapable of atmosphering flight.
    /// </summary>
    [JsonPropertyName("max_atmosphering_speed")]
    public int? MaxAtmospheringSpeed { get; set; }

    /// <summary>
    /// The number of personnel needed to run or pilot this starship.
    /// </summary>
    [JsonPropertyName("crew")]
    public int? Crew { get; set; }

    /// <summary>
    /// The number of non-essential people this starship can transport.
    /// </summary>
    [JsonPropertyName("passengers")]
    public int? Passengers { get; set; }

    /// <summary>
    /// The maximum number of kilograms that this starship can transport.
    /// </summary>
    [JsonPropertyName("cargo_capacity")]
    public long? CargoCapacity { get; set; }

    /// <summary>
    /// The maximum length of time that this starship can provide consumables for its entire crew without having to resupply.
    /// </summary>
    [JsonPropertyName("consumables")]
    public string Consumables { get; set; }

    /// <summary>
    /// A list of Person URL Resources that this starship has been piloted by.
    /// </summary>
    [JsonPropertyName("pilots")]
    [JsonConverter(typeof(PersonListConverter))]
    public IList<Person> Pilots { get; set; } = new List<Person>();

    /// <summary>
    /// A list of Film resources that this starship has appeared in.
    /// </summary>
    [JsonPropertyName("films")]
    [JsonConverter(typeof(FilmListConverter))]
    public IList<Film> Films { get; set; } = new List<Film>();
}
