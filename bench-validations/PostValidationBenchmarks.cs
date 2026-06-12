using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

using DADto = social_media_api_dataannotations_dotnet10.DTOs.CreatePostDto;
using FVDto = social_media_api_fluentvalidation_dotnet10.DTOs.CreatePostDto;
using FVValidator = social_media_api_fluentvalidation_dotnet10.Validators.CreatePostValidator;


namespace bench_validations
{
    // =================================================================
    // BENCHMARK
    // =================================================================

    /// <summary>
    /// To simulate that occurs inside each POST endpoint
    ///   DataAnnotations → [ApiController] call of function directly:
    ///     Validator.TryValidateObject(dto, context, results, validateAllProperties: true)
    ///
    ///   FluentValidation → endpoint calls manually:
    ///     validator.Validate(dto)
    ///
    /// [MemoryDiagnoser]  → Gen0/Gen1 GC and allocated bytes alocados for each operation.
    /// </summary>
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    [HideColumns("Error", "StdDev", "Median", "RatioSD")]
    public class PostValidationBenchmarks
    {
        // DTOs pré-criados no GlobalSetup para isolar o custo de
        // validação do custo de construção do objeto.
        private DADto _daValid = null!;
        private DADto _daInvalid = null!;
        private FVDto _fvValid = null!;
        private FVDto _fvInvalid = null!;

        // Validator singleton — to reply the production behaviour (DI singleton).
        private readonly FVValidator _fluentValidator = new();

        // Reuse list for each invalid scenario of DataAnnotation;
        private readonly List<ValidationResult> _daErrors = new(8);

        [GlobalSetup]
        public void Setup()
        {
            var validTags = new List<string> { "dotnet", "api", "csharp" };     // 3 tags — válido
            var invalidTags = new List<string> { "a", "b", "c", "d", "e", "f" }; // 6 tags — viola FV

            // Happy Path — All fields are valid 
            _daValid = new DADto { Title = "Post de performance em .NET 10", Body = "Comparison between DataAnnotations and FluentValidation.", UserId = 42, Views = 500, Tags = validTags };
            _fvValid = new FVDto { Title = "Post de performance em .NET 10", Body = "Comparison between DataAnnotations and FluentValidation.", UserId = 42, Views = 500, Tags = validTags };

            // Invalid Scenario — Multiple rule violations (mimics  malformed payload)
            _daInvalid = new DADto { Title = "", Body = "", UserId = 0, Views = 99999, Tags = invalidTags };
            _fvInvalid = new FVDto { Title = "", Body = "", UserId = 0, Views = 99999, Tags = invalidTags };
        }

        // -----------------------------------------------------------------
        // DATA ANNOTATIONS
        // It emulates the built-in logic that [ApiController] runs before reaching the action.
        // Nota: DataAnnotations não tem constraint nativa para tamanho de coleção
        // (Tags), portanto 6 tags NÃO geram erro aqui — assimetria intencional.
        // -----------------------------------------------------------------

        [Benchmark(Baseline = true, Description = "DA  | valid")]
        public bool DataAnnotations_Valid()
        {
            var ctx = new ValidationContext(_daValid);
            return Validator.TryValidateObject(_daValid, ctx, null, true);
        }

        [Benchmark(Description = "DA  | invalid")]
        public bool DataAnnotations_Invalid()
        {
            _daErrors.Clear();
            var ctx = new ValidationContext(_daInvalid);
            return Validator.TryValidateObject(_daInvalid, ctx, _daErrors, validateAllProperties: true);
        }

        // -----------------------------------------------------------------
        // FLUENTVALIDATION
        // To emulate the PostController.Create()
        // -----------------------------------------------------------------

        [Benchmark(Description = "FV  | valid")]
        public bool FluentValidation_Valid()
            => _fluentValidator.Validate(_fvValid).IsValid;

        [Benchmark(Description = "FV  | invalid")]
        public bool FluentValidation_Invalid()
            => _fluentValidator.Validate(_fvInvalid).IsValid;
    }

}
