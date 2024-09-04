namespace BouncyHsm.Nuget.ExampleIntegrationTests;

[CollectionDefinition(CollectionName, DisableParallelization = false)]
public class IntegrationTestsCollectionDefinition : ICollectionFixture<IntegrationTestsFixture>
{
    public const string CollectionName = "Integration Tests";
}
