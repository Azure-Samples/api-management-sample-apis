// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Todo.Data;
using Todo.RestApi.Services;

var builder = WebApplication.CreateBuilder(args);

/******************************************************************************************
**
** Add services to the container
*/

/*
** Application Insights logging
*/
builder.Services.AddApplicationInsightsTelemetry();

/*
** CORS
*/
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

/*
** Entity Framework Core Setup.
*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null)
{
    throw new ApplicationException("DefaultConnection is not set");
}
builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

/*
** Automapper
*/
builder.Services.AddAutoMapper(typeof(MappingProfile));

/*
** The Data Service.
*/
builder.Services.AddScoped<ITodoDataService, TodoDataService>();

/*
** Controllers.
*/
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true));
});

/*
** Swagger / OpenAPI.
*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/******************************************************************************************
**
** Configure the HTTP Pipeline.
*/
var app = builder.Build();

/*
** Database Initialization
*/
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    if (context is IDatabaseInitializer initializer)
    {
        await initializer.InitializeDatabaseAsync();
    }
}

/*
** CORS
*/
app.UseCors();

/*
** Add Swagger support.
*/
app.UseSwagger();
app.UseSwaggerUI();

/*
** Controllers.
*/
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

/************************************************************************************************
**
** Run the application.
*/
app.Run();
