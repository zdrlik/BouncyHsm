using Monet.BouncyHsm;
using Testcontainers.BouncyHsm;

namespace BouncyHsm.Nuget.ExampleIntegrationTests;

public class IntegrationTestsFixture : IAsyncLifetime
{
    private readonly BouncyHsmContainer bouncyHsmContainer = new BouncyHsmBuilder()
        .WithImage(BouncyHsmBuilder.BouncyHsmImage)
        .Build();

    public async Task InitializeAsync()
    {
        await StartBouncyHsm();
    }

    public async Task DisposeAsync()
    {
        await DisposeBouncyHsm();
    }

    public string GetConnectionString()
    {
        return bouncyHsmContainer.GetConnectionString();
    }

    async Task StartBouncyHsm()
    {
        await bouncyHsmContainer.StartAsync();
        await bouncyHsmContainer.CreateTestSlot();

        var connectionString = GetConnectionString();
        BouncyHsmExtensions.SetConnectionStringViaFile(connectionString);
    }

    async Task DisposeBouncyHsm()
    {
        BouncyHsmExtensions.RemoveBouncyHsmConnectionString();

        var (Stdout, Stderr) = await bouncyHsmContainer.GetLogsAsync();
        Console.WriteLine("*** BOUNCY HSM LOGS ***");
        Console.WriteLine(Stdout);
        Console.WriteLine(Stderr);
        Console.WriteLine("*** BOUNCY HSM LOGS END ***");
        await bouncyHsmContainer.DisposeAsync();
    }
}
