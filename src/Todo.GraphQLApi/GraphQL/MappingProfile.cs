// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using AutoMapper;

namespace Todo.GraphQLApi.GraphQL;

/// <summary>
/// The <see cref="AutoMapper"/> profile for mapping the database models
/// to the GraphQL DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Data.TodoItem, DTO.TodoItem>();
        CreateMap<Data.TodoList, DTO.TodoList>();
    }
}
