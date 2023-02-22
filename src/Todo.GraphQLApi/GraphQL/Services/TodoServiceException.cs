// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace Todo.GraphQLApi.GraphQL.Services;

/// <summary>
/// An exception thrown when there is an error with the <see cref="TodoDataService"/>.
/// </summary>
public class TodoServiceException : Exception
{
    public TodoServiceException()
    {
    }

    public TodoServiceException(string? message) : base(message)
    {
    }

    public TodoServiceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TodoServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
