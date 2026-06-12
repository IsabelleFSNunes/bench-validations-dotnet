extern alias MinApi7;
extern alias MinApi8;
extern alias MinApi10; 

using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

using Net7Program = MinApi7::Program;
using Net8Program = MinApi8::Program;
using Net10Program = MinApi10::Program;

namespace bench_validations
{
    // =================================================================
    // BENCHMARK — PIPELINE HTTP (WebApplicationFactory)
    // Benchmarks the ASP.NET Core pipeline overhead in-memory (.NET 7, 8, and 10),
    // bypassing the actual TCP port. Scale: microseconds.
    // =================================================================

    [Config(typeof(IntegrationBenchmarkConfig))]
    [MemoryDiagnoser]
    [HideColumns("Error", "StdDev", "Median", "RatioSD")]
    public class EndpointPipelineBenchmarks
    {
        private WebApplicationFactory<Net7Program> _factory7 = null!;
        private WebApplicationFactory<Net8Program> _factory8 = null!;
        private WebApplicationFactory<Net10Program> _factory10 = null!;
        private HttpClient _client7 = null!;
        private HttpClient _client8 = null!;
        private HttpClient _client10 = null!;

        // Record local para não conflitar com os DTOs dos projetos referenciados
        private record PostPayload(string Title, string Body, int UserId, int Views, List<string>? Tags = null);

        private static readonly PostPayload ValidPayload = new("Benchmark Title", "Body content", 1, 100);
        private static readonly PostPayload InvalidPayload = new("", "", 0, 99999);

        private static WebApplicationFactoryClientOptions NoRedirect =>
            new() { AllowAutoRedirect = false };

        [GlobalSetup]
        public void Setup()
        {
            _factory7  = new WebApplicationFactory<Net7Program>()
                .WithWebHostBuilder(b => b.UseEnvironment("Testing"));
            _factory8  = new WebApplicationFactory<Net8Program>()
                .WithWebHostBuilder(b => b.UseEnvironment("Testing"));
            _factory10 = new WebApplicationFactory<Net10Program>()
                .WithWebHostBuilder(b => b.UseEnvironment("Testing"));
            _client7   = _factory7.CreateClient(NoRedirect);
            _client8   = _factory8.CreateClient(NoRedirect);
            _client10  = _factory10.CreateClient(NoRedirect);
        }

        [Benchmark(Baseline = true, Description = "Net7  | Valid ")]
        public Task<HttpResponseMessage> Net7_Valid()
            => _client7.PostAsJsonAsync("/api/posts", ValidPayload);

        [Benchmark(Description = "Net7  | Invalid ")]
        public Task<HttpResponseMessage> Net7_Invalid()
            => _client7.PostAsJsonAsync("/api/posts", InvalidPayload);

        [Benchmark(Description = "Net8  | Valid ")]
        public Task<HttpResponseMessage> Net8_Valid()
            => _client8.PostAsJsonAsync("/api/posts", ValidPayload);

        [Benchmark(Description = "Net8  | Invalid ")]
        public Task<HttpResponseMessage> Net8_Invalid()
            => _client8.PostAsJsonAsync("/api/posts", InvalidPayload);

        [Benchmark(Description = "Net10 | Valid ")]
        public Task<HttpResponseMessage> Net10_Valid()
            => _client10.PostAsJsonAsync("/api/posts", ValidPayload);

        [Benchmark(Description = "Net10 | Invalid ")]
        public Task<HttpResponseMessage> Net10_Invalid()
            => _client10.PostAsJsonAsync("/api/posts", InvalidPayload);

        [GlobalCleanup]
        public void Cleanup()
        {
            _client7.Dispose(); _factory7.Dispose();
            _client8.Dispose(); _factory8.Dispose();
            _client10.Dispose(); _factory10.Dispose();
        }
    }
}
