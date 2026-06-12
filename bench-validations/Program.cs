
// =================================================================
// BENCHMARK: DataAnnotations vs FluentValidation  +  Pipeline HTTP
//
// Micro-benchmark (Only logic will be evaluate, nanosegundos):
//   DADto      → social_media_api_dataannotations_dotnet10.DTOs.CreatePostDto
//   FVDto      → social_media_api_fluentvalidation_dotnet10.DTOs.CreatePostDto
//   FVValidator→ social_media_api_fluentvalidation_dotnet10.Validators.CreatePostValidator
//
// Macro-benchmark (The ASP.NET Core pipeline will execute from memory, microssegundos):
//   Net7Program  → minimal_api_net7_dataannotations.Program  (alias MinApi7)
//   Net8Program  → minimal_api_net8_dataannotations.Program  (alias MinApi8)
//   Net10Program → minimal_api_net10_dataannotation.Program  (alias MinApi10)
// =================================================================

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc.Testing;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;

using bench_validations;


var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
using var fileOut = new StreamWriter($"benchmark_{timestamp}.out", append: false, Encoding.UTF8);
var tee = new TeeWriter(Console.Out, fileOut);
Console.SetOut(tee);

BenchmarkRunner.Run<PostValidationBenchmarks>();
BenchmarkRunner.Run<EndpointPipelineBenchmarks>();
