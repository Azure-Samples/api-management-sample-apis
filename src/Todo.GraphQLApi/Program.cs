// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.GraphQLApi.GraphQL;

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
builder.Services.AddPooledDbContextFactory<TodoDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

/*
** GraphQL Services
*/
builder.Services.AddGraphQLService();
    
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
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TodoDbContext>>();
    var context = contextFactory.CreateDbContext();
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
** Controllers.
*/
app.UseHttpsRedirection();
app.MapGraphQL();

/*
** Redirect index page to GraphQL console
*/
app.MapGet("/", (HttpResponse response) => response.Redirect("/graphql"));

/************************************************************************************************
**
** Run the application.
*/
app.Run();

