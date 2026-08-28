namespace WexaGraph.Api.Services
{
    public interface ICognoDbService
    {
        Task<bool> TestConnectionAsync();

        Task<List<string>> GetProjectsByTechnologyAsync(
            string technologyName);

        Task<List<object>> GetTechnologyDomainsAsync(
            string technologyName);

        Task<List<object>> GetRecommendationsAsync(
            string technologyName);

        Task<List<object>> GetGraphAsync(
            string technologyName);
    }
}