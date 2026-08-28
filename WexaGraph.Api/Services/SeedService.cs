using Neo4j.Driver;

namespace WexaGraph.Api.Services
{
    public class SeedService
    {
        private readonly IDriver _driver;

        public SeedService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task SeedAsync()
        {
            const string cypher = """
        MERGE (angular:Technology { name: 'Angular' })
        SET angular.category = 'Frontend'

        MERGE (dotnet:Technology { name: '.NET' })
        SET dotnet.category = 'Backend'

        MERGE (typescript:Technology { name: 'TypeScript' })
        SET typescript.category = 'Language'

        MERGE (csharp:Technology { name: 'C#' })
        SET csharp.category = 'Language'

        MERGE (ai:Technology { name: 'Artificial Intelligence' })
        SET ai.category = 'AI'

        MERGE (banking:Domain { name: 'Banking' })

        MERGE (healthcare:Domain { name: 'Healthcare' })

        MERGE (bankingApi:Project { name: 'Banking API Platform' })
        SET bankingApi.description = 'Secure banking API platform'

        MERGE (healthcarePortal:Project { name: 'Healthcare Portal' })
        SET healthcarePortal.description = 'Patient healthcare management portal'

        MERGE (angular)-[:RELATED_TO]->(typescript)

        MERGE (dotnet)-[:RELATED_TO]->(csharp)

        MERGE (bankingApi)-[:USES]->(angular)
        MERGE (bankingApi)-[:USES]->(dotnet)
        MERGE (bankingApi)-[:USES]->(csharp)
        MERGE (bankingApi)-[:IN_DOMAIN]->(banking)

        MERGE (healthcarePortal)-[:USES]->(angular)
        MERGE (healthcarePortal)-[:USES]->(typescript)
        MERGE (healthcarePortal)-[:USES]->(ai)
        MERGE (healthcarePortal)-[:IN_DOMAIN]->(healthcare)
        """;

            await using var session = _driver.AsyncSession();

            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync(cypher);
                return true;
            });
        }
    }
}
