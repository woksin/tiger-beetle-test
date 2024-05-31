// // Copyright (c) woksin-org. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Proto.Cluster;
using Proto.Cluster.SingleNode;
using Proto.OpenTelemetry;
using Proto.Remote;
using TigerBeetle;

namespace TigerBeetleTest;

public static class Setup
{
    public static void ConfigureTracing(this WebApplicationBuilder builder) =>
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(b => b.AddService("TigerBeetleTest"))
            .WithTracing(b =>
            {
                b.AddProtoActorInstrumentation();
                b.AddAspNetCoreInstrumentation();
                b.AddOtlpExporter();
            });

    public static void ConfigureProtoActor(this WebApplicationBuilder builder)
    {
        builder.Services.AddProtoCluster("TestCluster",
            configureSystem: config => config.WithConfigureRootContext(context => context.WithTracing()),
            clusterProvider: new SingleNodeProvider(),
            identityLookup: new SingleNodeLookup(),
            configureRemote: config => config.WithProtoMessages(Messages.ProtosReflection.Descriptor));
    }
    
    public static void ConfigureTigerBeetle(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<TigerBeetleConfig>().BindConfiguration("TigerBeetle");
        builder.Services.AddSingleton<Client>(p =>
        {
            var config = p.GetRequiredService<IOptionsMonitor<TigerBeetleConfig>>().CurrentValue;
            return new Client(config.ClusterId, config.Replicas, config.ConcurrencyMax);
        });
    }
}
