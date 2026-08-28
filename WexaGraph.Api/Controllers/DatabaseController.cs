using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WexaGraph.Api.Services;

namespace WexaGraph.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly CognoDbService _cognoDbService;
        private readonly SeedService _seedService;


        public DatabaseController(CognoDbService cognoDbService, SeedService seedService)
        {
            _cognoDbService = cognoDbService;
            _seedService = seedService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var connected = await _cognoDbService.TestConnectionAsync();

                return Ok(new
                {
                    success = connected,
                    message = "CognoDB connection successful."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Unable to connect to CognoDB.",
                    error = ex.Message
                });
            }
        }
        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                await _seedService.SeedAsync();

                return Ok(new
                {
                    success = true,
                    message = "Database seeded successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Database seeding failed.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("projects-by-technology")]
        public async Task<IActionResult> GetProjectsByTechnology([FromQuery] string technology)
        {
            if (string.IsNullOrWhiteSpace(technology))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Technology is required."
                });
            }

            try
            {
                var projects =
                    await _cognoDbService
                        .GetProjectsByTechnologyAsync(technology);

                return Ok(new
                {
                    success = true,
                    technology,
                    projects
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch projects.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("technology-domains")]
        public async Task<IActionResult> GetTechnologyDomains([FromQuery] string technology)
        {
            if (string.IsNullOrWhiteSpace(technology))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Technology is required."
                });
            }

            try
            {
                var results =
                    await _cognoDbService
                        .GetTechnologyDomainsAsync(technology);

                return Ok(new
                {
                    success = true,
                    technology,
                    results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch technology domains.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations([FromQuery] string technology)
        {
            if (string.IsNullOrWhiteSpace(technology))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Technology is required."
                });
            }

            try
            {
                var results =
                    await _cognoDbService
                        .GetRecommendationsAsync(technology);

                return Ok(new
                {
                    success = true,
                    technology,
                    recommendations = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get recommendations.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("graph")]
        public async Task<IActionResult> GetGraph([FromQuery] string technology)
        {
            if (string.IsNullOrWhiteSpace(technology))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Technology is required."
                });
            }

            try
            {
                var graph =
                    await _cognoDbService
                        .GetGraphAsync(technology);

                return Ok(new
                {
                    success = true,
                    technology,
                    graph
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to load graph.",
                    error = ex.Message
                });
            }
        }
    }
}
