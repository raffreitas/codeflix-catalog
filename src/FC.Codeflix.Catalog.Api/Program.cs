using FC.Codeflix.Catalog.Api.Categories;
using FC.Codeflix.Catalog.Api.Filters;
using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddUseCases()
    .AddElasticSearch(builder.Configuration)
    .AddRepositories()
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<CategoryQueries>()
    .AddTypeExtension<CategoryMutations>()
    .AddErrorFilter<GraphQlErrorFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGraphQL();

app.MapControllers();

await app.RunAsync();