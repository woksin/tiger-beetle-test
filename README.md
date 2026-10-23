# tiger-beetle-test

2024 experiment: poking at [TigerBeetle](https://tigerbeetle.com), the financial-transactions database, from .NET 8, with [Proto.Actor](https://proto.actor) clustering alongside.

It is an ASP.NET minimal API with Swagger at `/`:

| Endpoint | What it does |
| --- | --- |
| `POST /accounts` | Creates 1,000 random accounts in TigerBeetle in one batch |
| `GET /accounts?ids=...` | Looks up accounts by id |
| `GET /accounts/{id}` | Looks up one account |

Proto.Actor is set up as a single-node cluster with OpenTelemetry tracing and Serilog logging to Seq. The `AccountsGrain` defined in `Protos.proto` is a stub that throws `NotImplementedException`. The project was never finished.

## Run it

Needs the .NET 8 SDK and Docker. Configure the TigerBeetle replica addresses in a `TigerBeetle` section of `appsettings.json` (`Replicas`, `ClusterId`, `ConcurrencyMax`); the committed file has none.

```sh
cd environment
docker compose up -d        # RabbitMQ, Jaeger and Seq
# start a TigerBeetle replica yourself, then:
cd .. && dotnet run
```

`environment/dev.sh single|cluster` starts local TigerBeetle replicas, but it expects a `tigerbeetle` binary at `../infra/tigerbeetle` that is not in this repo. I did not run any of this again when writing this README.

No LICENSE file.
