using BouncyHsm.Nuget.Example;
using FluentAssertions;
using Monet.BouncyHsm;
using Net.Pkcs11Interop.Common;
using Xunit.Abstractions;

namespace BouncyHsm.Nuget.ExampleIntegrationTests;


[Collection(IntegrationTestsCollectionDefinition.CollectionName)]
public class CryptoServiceTests(IntegrationTestsFixture fixture, ITestOutputHelper output)
{
    private readonly IntegrationTestsFixture fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
    private readonly ITestOutputHelper output = output ?? throw new ArgumentNullException(nameof(output));

    [Fact]
    public void ShouldGetSlotList()
    {
        // Arrange
        var libraryPath = BouncyHsmExtensions.GetPkcs11LibraryPath();
        var connectionString = fixture.GetConnectionString();
        output.WriteLine($"BouncyHsm PKCS11 library path: {libraryPath}");
        output.WriteLine($"BouncyHsm connection string: {connectionString}");
        var cryptoService = new CryptoServiceBuilder()
            .WithPkcs11Library(libraryPath)
            .Build();
        cryptoService.InitializePkcs11Library();

        // Act        
        var slotList = cryptoService.GetSlotList(SlotsType.WithOrWithoutTokenPresent);

        // Assert            
        slotList.Should().NotBeNullOrEmpty();
    }
}

internal class CryptoServiceBuilder
{
    private string? libraryPath;

    public CryptoServiceBuilder()
    {
        libraryPath = null;
    }

    public CryptoServiceBuilder WithPkcs11Library(string libraryPath)
    {
        this.libraryPath = libraryPath;
        return this;
    }

    public CryptoService Build()
    {
        if (libraryPath == null)
            throw new InvalidOperationException($"PKCS11 library path is not set, use the {nameof(WithPkcs11Library)} method to set the library path");

        return new CryptoService(libraryPath);
    }
}