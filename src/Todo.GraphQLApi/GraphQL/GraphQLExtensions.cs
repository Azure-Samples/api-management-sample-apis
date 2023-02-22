// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using HotChocolate.Types.Pagination;
using Todo.GraphQLApi.GraphQL.Services;

namespace Todo.GraphQLApi.GraphQL;

public static class GraphQLExtensions
{
    /// <summary>
    /// Adds the appropriate GraphQL Services to the collection.
    /// </summary>
    /// <param name="services"></param>
    public static void AddGraphQLService(this IServiceCollection services)
    {
        var pagingOptions = new PagingOptions { MaxPageSize = 100, DefaultPageSize = 50 };

        services.AddGraphQLServer()
            .RegisterService<TodoDataService>()
            .AddTypes()
            .AddMutationConventions()
            .AddGlobalObjectIdentification()
            .SetPagingOptions(pagingOptions)
            .AddFiltering()
            .AddSorting();
    }
}
