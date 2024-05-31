using Proto;
using TigerBeetle;
using TigerBeetleTest.Messages;

namespace TigerBeetleTest;

public class TigerBeetleConfig
{
    public string[] Replicas { get; set; }
    public UInt128 ClusterId { get; set; } = UInt128.Zero;
    public int ConcurrencyMax { get; set; } = 256;
}

public class AccountsGrain(IContext context, Client client, string identity)
    : AccountsGrainBase(context)
{
    readonly Client _client = client;
    readonly string _identity = identity;

    public override Task<AddAccountsResponse> AddAccounts(AddAccountsRequest request)
        => throw new NotImplementedException();
}
