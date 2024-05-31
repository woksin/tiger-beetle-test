using Microsoft.AspNetCore.Mvc;
using Proto.Cluster;
using Serilog;
using TigerBeetle;
using TigerBeetleTest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((context, config) => config
    .ReadFrom.Configuration(context.GetRequiredService<IConfiguration>()));

builder.Services.AddHealthChecks().AddCheck<ClusterHealthCheck>("proto", tags: ["ready", "live"]);
builder.ConfigureTracing();
builder.ConfigureProtoActor();
builder.ConfigureTigerBeetle();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapPost("/accounts", ([FromServices] Client client) =>
{
    
    return client.CreateAccountsAsync(Enumerable.Range(0, 1000).Select(_ => new Account()
    {
        Id = Guid.NewGuid().ToUInt128(),
        Code = 718,
        Flags = AccountFlags.None,
        Ledger = 1,
        UserData32 = 100,
        UserData64 = 1000,
        UserData128 = Guid.NewGuid().ToUInt128()
    }).ToArray());
});

app.MapGet("/accounts", ([FromServices] Client client, [FromQuery]UInt128[] ids) => client.LookupAccountsAsync(new ReadOnlyMemory<UInt128>(ids)));
app.MapGet("/accounts/{id}", ([FromServices] Client client, [FromRoute]UInt128 id) => client.LookupAccountAsync(id));

app.Run();
