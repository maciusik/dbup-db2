using DbUp.Tests.Common;

namespace DbUp.Db2.Tests;

public class NoPublicApiChanges : NoPublicApiChangesBase
{
    public NoPublicApiChanges()
        : base(typeof(NewProviderExtensions).Assembly)
    {
    }
}
