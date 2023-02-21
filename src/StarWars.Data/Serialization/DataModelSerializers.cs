// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWars.Data.Serialization;

public static class DataModelHelpers
{
    public static void WriteBaseModelValue(this Utf8JsonWriter writer, BaseModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("id");
        writer.WriteNumberValue(value.Id);
        writer.WriteEndObject();
    }

    public static void WriteBaseModelArray(this Utf8JsonWriter writer, IEnumerable<BaseModel> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteBaseModelValue(item, options);
        }
        writer.WriteEndArray();
    }
}

public class FilmListConverter : JsonConverter<IList<Film>>
{
    public override IList<Film>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IList<Film> value, JsonSerializerOptions options)
        => writer.WriteBaseModelArray(value, options);
}

public class PersonListConverter : JsonConverter<IList<Person>>
{
    public override IList<Person>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IList<Person> value, JsonSerializerOptions options)
        => writer.WriteBaseModelArray(value, options);
}

public class PlanetConverter : JsonConverter<Planet>
{
    public override Planet? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, Planet value, JsonSerializerOptions options)
        => writer.WriteBaseModelValue(value, options);
}

public class PlanetListConverter : JsonConverter<IList<Planet>>
{
    public override IList<Planet>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IList<Planet> value, JsonSerializerOptions options)
        => writer.WriteBaseModelArray(value, options);
}

public class SpeciesConverter : JsonConverter<Species>
{
    public override Species? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, Species value, JsonSerializerOptions options)
        => writer.WriteBaseModelValue(value, options);
}

public class SpeciesListConverter : JsonConverter<IList<Species>>
{
    public override IList<Species>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IList<Species> value, JsonSerializerOptions options)
        => writer.WriteBaseModelArray(value, options);
}

public class StarshipListConverter : JsonConverter<IList<Starship>>
{
    public override IList<Starship>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IList<Starship> value, JsonSerializerOptions options)
        => writer.WriteBaseModelArray(value, options);
}

public class VehicleListConverter : JsonConverter<IList<Vehicle>>
{
    public override IList<Vehicle>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IList<Vehicle> value, JsonSerializerOptions options)
        => writer.WriteBaseModelArray(value, options);
}

