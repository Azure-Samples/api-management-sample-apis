// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using Todo.Data;
using Todo.RestApi.Services;

namespace Todo.RestApi.Extensions;

public static class Utils
{
    private const int DefaultBatchSize = 20;
    private const int MaxBatchSize = 100;


    /// <summary>
    /// Gets this request, but without the $skip and $top parameters.
    /// </summary>
    /// <returns></returns>
    public static string? GetBaseUri(HttpRequest? request)
    {
        if (request == null)
        {
            return null;
        }

        var baseUri = new UriBuilder(request.GetDisplayUrl());
        if (request.QueryString.HasValue)
        {
            var query = HttpUtility.ParseQueryString(request.QueryString.Value ?? string.Empty);
            query.Remove("$skip");
            query.Remove("$top");
            baseUri.Query = query.ToString();
        }
        return baseUri.ToString();
    }

    /// <summary>
    /// Gets the appropriate batch size.
    /// </summary>
    /// <param name="batchSize">The customer provided batch size.</param>
    /// <returns>The actual batch size to use.</returns>
    public static int GetBatchSize(int? batchSize)
    {
        if (batchSize == null || batchSize < 1)
        {
            return DefaultBatchSize;
        }
        else if (batchSize < MaxBatchSize)
        {
            return (int)batchSize;
        }
        else
        {
            return MaxBatchSize;
        }
    }

    /// <summary>
    /// Converts a string into a Guid, throwing <see cref="MalformedInputException"/> if the ID is invalid.
    /// </summary>
    /// <param name="id">The ID to transform.</param>
    /// <returns>The transformed ID.</returns>
    /// <exception cref="MalformedInputException">if the ID is malformed.</exception>
    public static Guid ParseGuid(string id)
    {
        if (Guid.TryParse(id, out Guid result) && result != Guid.Empty)
        {
            return result;
        }
        throw new MalformedInputException($"The ID \'{id}\' is not a valid GUID.");
    }

    /// <summary>
    /// Converts a State string into a TodoItemState.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public static bool ParseState(string? input, out TodoItemState state)
    {
        if (string.IsNullOrEmpty(input) || input.Equals("todo", StringComparison.OrdinalIgnoreCase))
        {
            state = TodoItemState.Todo;
            return true;
        }
        if (input.Equals("inprogress", StringComparison.OrdinalIgnoreCase) || input.Equals("in_progress", StringComparison.OrdinalIgnoreCase))
        {
            state = TodoItemState.InProgress;
            return true;
        }
        if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
        {
            state = TodoItemState.Done;
            return true;
        }
        state = TodoItemState.Todo;
        return false;
    }

    /// <summary>
    /// Determine if skip and top are valid.
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="batchSize"></param>
    /// <returns></returns>
    public static void ValidatePaging(int? skip, int? batchSize)
    {
        if (skip.HasValue && skip.Value < 0)
        {
            throw new MalformedInputException($"The value of '$skip' is invalid.");
        }
        if (batchSize.HasValue && (batchSize.Value < 1 || batchSize > MaxBatchSize))
        {
            throw new MalformedInputException($"The value of '$top' is invalid.");
        }
    }

}
