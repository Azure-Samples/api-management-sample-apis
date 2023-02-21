// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using StarWars.Data;
using StarWars.RestApi.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
** Add the data model to the service builder so it can be accessed everywhere.
*/
builder.Services.AddSingleton<StarWarsData>();

/*
** Add all the controllers to the service builder.
*/
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.AddDateOnlyConverters());

/*
** Add the Swagger generator.
*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/************************************************************************************************
**
** HTTP PIPELINE BUILDER
*/
var app = builder.Build();

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
