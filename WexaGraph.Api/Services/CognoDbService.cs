using Neo4j.Driver;

namespace WexaGraph.Api.Services
{
    public class CognoDbService
    {
        private readonly IDriver _driver;

        public CognoDbService(IConfiguration configuration)
        {
            var uri = configuration["COGNODB_URI"];
            var username = configuration["COGNODB_USERNAME"];
            var password = configuration["COGNODB_PASSWORD"];

            if (string.IsNullOrWhiteSpace(uri))
                throw new InvalidOperationException("COGNODB_URI is not configured.");

            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidOperationException("COGNODB_USERNAME is not configured.");

            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("COGNODB_PASSWORD is not configured.");

            _driver = GraphDatabase.Driver(
                uri,
                AuthTokens.Basic(username, password));
        }

        public async Task<bool> TestConnectionAsync()
        {
            await using var session = _driver.AsyncSession();

            var result = await session.RunAsync(
                "RETURN 1 AS result");

            var record = await result.SingleAsync();

            return record["result"].As<int>() == 1;
        }
        public async Task<List<string>> GetProjectsByTechnologyAsync(string technologyName)
        {
            const string cypher = """
        MATCH (project:Project)-[:USES]->(technology:Technology)
        WHERE technology.name = $technologyName
        RETURN project.name AS projectName
        ORDER BY project.name
        """;

            await using var session = _driver.AsyncSession();

            var result = await session.RunAsync(
                cypher,
                new { technologyName });

            var records = await result.ToListAsync();

            return records
                .Select(record => record["projectName"].As<string>())
                .ToList();
        }
        public async Task<List<object>> GetTechnologyDomainsAsync(string technologyName)
        {
            const string cypher = """
        MATCH (technology:Technology)<-[:USES]-(project:Project)-[:IN_DOMAIN]->(domain:Domain)
        WHERE technology.name = $technologyName
        RETURN
            project.name AS projectName,
            domain.name AS domainName
        ORDER BY domain.name, project.name
        """;

            await using var session = _driver.AsyncSession();

            var result = await session.RunAsync(
                cypher,
                new { technologyName });

            var records = await result.ToListAsync();

            return records
                .Select(record => (object)new
                {
                    ProjectName = record["projectName"].As<string>(),
                    DomainName = record["domainName"].As<string>()
                })
                .ToList();
        }
        public async Task<List<object>> GetRecommendationsAsync(string technologyName)
        {
            const string cypher = """
        MATCH (technology:Technology)-[:RELATED_TO]->(related:Technology)
        WHERE technology.name = $technologyName

        OPTIONAL MATCH (project:Project)-[:USES]->(related)

        RETURN
            related.name AS technologyName,
            related.category AS category,
            collect(DISTINCT project.name) AS projects
        ORDER BY related.name
        """;

            await using var session = _driver.AsyncSession();

            var result = await session.RunAsync(
                cypher,
                new { technologyName });

            var records = await result.ToListAsync();

            return records
                .Select(record => (object)new
                {
                    TechnologyName =
                        record["technologyName"].As<string>(),

                    Category =
                        record["category"].As<string>(),

                    Projects =
                        record["projects"]
                            .As<List<object>>()
                            .Select(x => x.As<string>())
                            .ToList()
                })
                .ToList();
        }
        public async Task<List<object>> GetGraphAsync(string technologyName)
        {
            const string cypher = """
        MATCH (technology:Technology)
        WHERE technology.name = $technologyName

        OPTIONAL MATCH (technology)-[:RELATED_TO]->(related:Technology)

        OPTIONAL MATCH (project:Project)-[:USES]->(technology)

        OPTIONAL MATCH (project)-[:IN_DOMAIN]->(domain:Domain)

        RETURN
            technology.name AS technologyName,
            related.name AS relatedTechnology,
            project.name AS projectName,
            domain.name AS domainName
        """;

            await using var session = _driver.AsyncSession();

            var result = await session.RunAsync(
                cypher,
                new { technologyName });

            var records = await result.ToListAsync();

            return records.Select(record => (object)new
            {
                TechnologyName =
                    record["technologyName"].As<string>(),

                RelatedTechnology =
                    record["relatedTechnology"].As<string?>(),

                ProjectName =
                    record["projectName"].As<string?>(),

                DomainName =
                    record["domainName"].As<string?>()
            }).ToList();
        }
    }
}
