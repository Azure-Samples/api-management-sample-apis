// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using AutoMapper;

namespace Todo.RestApi.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Data.TodoItem, DTO.TodoItem>();
        CreateMap<Data.TodoList, DTO.TodoList>();
    }
}
