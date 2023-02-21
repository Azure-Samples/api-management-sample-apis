// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Todo.RestApi.Services;

namespace Todo.RestApi.Extensions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is MalformedInputException)
        {
            context.Result = new BadRequestResult();
        }

        if (context.Exception is EntityExistsException entity_exists)
        {
            context.Result = entity_exists.Entity != null ? new ConflictObjectResult(entity_exists.Entity) : new ConflictResult();
        }

        if (context.Exception is EntityMissingException)
        {
            context.Result = new NotFoundResult();
        }
        
    }
}
