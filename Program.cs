using cryptoapi.CreatureGeneration;
using cryptoapi.Data;
using cryptoapi.Data.Interfaces;
using cryptoapi.Data.Models;
using cryptoapi.Data.Repositories;
using cryptoapi.Data.Resolvers;
using cryptoapi.Data.Resolvers.Mutation;

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

#if DEBUG
#pragma warning disable CS4014 
Task.Run(() => MockData.generateMockCreatures());
#pragma warning restore CS4014
#endif

await app.RunAsync();
