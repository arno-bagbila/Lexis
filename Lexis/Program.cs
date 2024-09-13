using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using MongoDB.Driver;
using System.Reflection;
using GraphQL;
using LexisApi.GraphQL;
using LexisApi.GraphQL.Queries;
using LexisApi.GraphQL.Types;
using Serilog;
using LexisApi.Infrastructure.Middlewares.CustomExceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
});

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connStr = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
    new MongoClient(connStr));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LexisQuery>();

//GraphQL
builder.Services.AddGraphQL(options =>
{
    options.AddSchema<AppSchema>();
    options.AddSystemTextJson();
});
builder.Services.AddSingleton<ObjectIdGraphType>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

//GraphQL
app.UseGraphQL();
app.UseGraphQLAltair();


app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
