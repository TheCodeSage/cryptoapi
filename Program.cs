var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddSingleton<CreatureGenerator, CreatureGenerator>()
.AddSingleton<IRepository<MarketplaceCreature>, MarketplaceRepository>()
.AddGraphQLServer()
.AddQueryType(t => t.Name("Query"))
.AddType<MarketplaceQueryResolver>()
.AddMutationType(m => m.Name("Mutation"))
.AddType<ArenaMutationResolver>()
.AddType<CreatureGeneratorResolver>();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
  endpoints.MapGraphQL());

app.UseGraphQLPlayground();

await app.RunAsync();