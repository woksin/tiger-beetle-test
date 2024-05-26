using Microsoft.AspNetCore.Mvc;
using TigerBeetle;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Client>(p => new Client(UInt128.Zero, [p.GetRequiredService<IConfiguration>().GetConnectionString("TigerBeetle")!]));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");


app.MapPost("/accounts", ([FromServices] Client client) => client.CreateAccountsAsync(new []
{
    new Account
    {
        Id = 1,
        Code = 718,
        Flags = AccountFlags.None,
        Ledger = 1,
        UserData32 = 100,
        UserData64 = 1000,
        UserData128 = Guid.NewGuid().ToUInt128()
    }
}));

app.Run();

