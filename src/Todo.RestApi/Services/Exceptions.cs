// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace Todo.RestApi.Services;

/// <summary>
/// The core exception for the <see cref="ITodoDataService"/> service.
/// </summary>
public class DataServiceException : Exception
{
    public DataServiceException()
    {
    }

    public DataServiceException(string? message) : base(message)
    {
    }

    public DataServiceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DataServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

/// <summary>
/// Exception that is thrown when the input from the user is mal-formed.
/// </summary>
public class MalformedInputException : DataServiceException
{
    public MalformedInputException()
    {
    }

    public MalformedInputException(string? message) : base(message)
    {
    }

    public MalformedInputException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected MalformedInputException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

/// <summary>
/// Exception that is thrown when an entity is expected, but does not exist.
/// </summary>
public class EntityMissingException : DataServiceException
{
    public EntityMissingException()
    {
    }

    public EntityMissingException(string? message) : base(message)
    {
    }

    public EntityMissingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

/// <summary>
/// Exception that is thrown when an entity does not exist, but is expected.
/// </summary>
public class EntityExistsException : DataServiceException
{
    public EntityExistsException()
    {
    }

    public EntityExistsException(string? message) : base(message)
    {
    }

    public EntityExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// Can be optionally set to the entity that exists.
    /// </summary>
    public object? Entity { get; set; } = null;
}
