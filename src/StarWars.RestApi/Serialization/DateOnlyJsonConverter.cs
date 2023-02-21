// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace StarWars.RestApi.Serialization;

/// <summary>
/// Serializer / Deserializer for DateOnly.
/// </summary>
/// <see href="https://github.com/dotnet/runtime/issues/53539"/>
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetDateTime(out var dt))
        {
            return DateOnly.FromDateTime(dt);
        }
        var value = reader.GetString();
        if (value == null)
        {
            return default;
        }
        var match = new Regex("^(\\d\\d\\d\\d)-(\\d\\d)-(\\d\\d)(T|\\s|\\z)").Match(value);
        return match.Success ? new DateOnly(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)) : default;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}

/// <summary>
/// Serializer / Deserializer for DateOnly?.
/// </summary>
/// <see href="https://github.com/dotnet/runtime/issues/53539"/>
public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetDateTime(out var dt))
        {
            return DateOnly.FromDateTime(dt);
        }
        var value = reader.GetString();
        if (value == null)
        {
            return default;
        }
        var match = new Regex("^(\\d\\d\\d\\d)-(\\d\\d)-(\\d\\d)(T|\\s|\\z)").Match(value);
        return match.Success
            ? new DateOnly(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value))
            : default;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.ToString("yyyy-MM-dd"));
}

/// <summary>
/// Extension methods to add DateOnly converters.
/// </summary>
public static class DateOnlyConverterExtensions
{
    public static void AddDateOnlyConverters(this JsonSerializerOptions options)
    {
        options.Converters.Add(new DateOnlyJsonConverter());
        options.Converters.Add(new NullableDateOnlyJsonConverter());
    }
}
