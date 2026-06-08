// =================================================================
// BENCHMARK: DataAnnotations vs FluentValidation
// Espelha exatamente os projetos:
//   - social-media-api-dataannotations-dotnet10
//   - social-media-api-mvc-dotnet10 (namespace: social_media_api_fluentvalidation_dotnet10)
//
// Execução:  dotnet run -c Release
// =================================================================

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

BenchmarkRunner.Run<PostValidationBenchmarks>();


// =================================================================
// DTO — DataAnnotations
// Cópia fiel de: social-media-api-dataannotations-dotnet10/DTOs/CreatePostDto.cs
// =================================================================
public sealed class CreatePostDtoAnnotations
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [MaxLength(199, ErrorMessage = "Título não pode exceder 199 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Conteúdo é obrigatório")]
    public string Body { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "UserId deve estar entre 1 e 100")]
    public int UserId { get; set; }

    [Range(0, 10000, ErrorMessage = "Views deve estar entre 0 e 10000")]
    public int Views { get; set; }

    // NOTA: Tags não tem constraint em DataAnnotations no projeto original.
    // DataAnnotations não oferece attribute nativo para tamanho de coleção.
    public List<string> Tags { get; set; } = [];
}


// =================================================================
// DTO — FluentValidation
// Cópia fiel de: social-media-api-mvc-dotnet10/DTOs/CreatePostDto.cs
// (POCO sem attributes — regras ficam no Validator)
// =================================================================
public sealed class CreatePostDtoFluent
{
    public string Title { get; set; } = string.Empty;
    public string Body  { get; set; } = string.Empty;
    public int UserId   { get; set; }
    public int Views    { get; set; }
    public List<string> Tags { get; set; } = [];
}


// =================================================================
// VALIDATOR — FluentValidation
// Cópia fiel de: social-media-api-mvc-dotnet10/Validators/CreatePostValidator.cs
// =================================================================
public sealed class CreatePostValidator : AbstractValidator<CreatePostDtoFluent>
{
    public CreatePostValidator()
    {
        const int maxCaracteresTitle = 199;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Titulo é obrigatório")
            .MaximumLength(maxCaracteresTitle)
            .WithMessage($"Título não pode exceder {maxCaracteresTitle} caracteres");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Conteúdo é obrigatório");

        RuleFor(x => x.UserId)
            .InclusiveBetween(1, 100).WithMessage("UserId deve estar entre 1 e 100");

        RuleFor(x => x.Views)
            .InclusiveBetween(0, 10000).WithMessage("Views deve estar entre 0 e 10000");

        RuleFor(x => x.Tags)
            .Must(t => t.Count <= 5).WithMessage("Máximo de 5 tags permitidas");
    }
}


// =================================================================
// BENCHMARK
// =================================================================

/// <summary>
/// Simula o que ocorre dentro de cada endpoint POST:
///
///   DataAnnotations → [ApiController] chama internamente:
///     Validator.TryValidateObject(dto, context, results, validateAllProperties: true)
///
///   FluentValidation → endpoint chama manualmente:
///     validator.Validate(dto)
///
/// [MemoryDiagnoser]  → coleta Gen0/Gen1 GC e bytes alocados por operação.
///
/// HardwareCounters   → requer execução como Administrador no Windows
///                      e CPU com suporte a PMU. Descomente o atributo
///                      abaixo para activar (pode causar erro sem privilégio):
///
///   [HardwareCounters(HardwareCounter.BranchMispredictions,
///                     HardwareCounter.CacheMisses)]
/// </summary>
[Config(typeof(BenchmarkConfig))]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Median", "RatioSD")]
public class PostValidationBenchmarks
{
    // --- DTOs pré-criados ---
    // Instanciados uma única vez no GlobalSetup para isolar o custo de
    // validação do custo de construção do objeto.
    private CreatePostDtoAnnotations _daValid    = null!;
    private CreatePostDtoAnnotations _daInvalid  = null!;
    private CreatePostDtoFluent      _fvValid    = null!;
    private CreatePostDtoFluent      _fvInvalid  = null!;

    // Validator singleton — replica o comportamento de produção (DI singleton).
    // Não mede custo de construção (reflection + compilação de lambdas do FV).
    private readonly CreatePostValidator _fluentValidator = new();

    // Lista reutilizada para o cenário inválido do DataAnnotations.
    // Evita que a alocação da List<ValidationResult> distorça a medição.
    private readonly List<ValidationResult> _daErrors = new(8);

    [GlobalSetup]
    public void Setup()
    {
        var validTags   = new List<string> { "dotnet", "api", "csharp" };        // 3 tags — válido
        var invalidTags = new List<string> { "a", "b", "c", "d", "e", "f" };    // 6 tags — viola FV

        // Cenário válido — todos os campos dentro das restrições
        _daValid = new CreatePostDtoAnnotations
        {
            Title  = "Post de performance em .NET 10",
            Body   = "Comparando DataAnnotations com FluentValidation.",
            UserId = 42,
            Views  = 500,
            Tags   = validTags
        };
        _fvValid = new CreatePostDtoFluent
        {
            Title  = "Post de performance em .NET 10",
            Body   = "Comparando DataAnnotations com FluentValidation.",
            UserId = 42,
            Views  = 500,
            Tags   = validTags
        };

        // Cenário inválido — múltiplas regras violadas em simultâneo
        // (simula um payload mal formado enviado ao endpoint)
        _daInvalid = new CreatePostDtoAnnotations
        {
            Title  = "",             // viola [Required]
            Body   = "",             // viola [Required]
            UserId = 0,              // viola [Range(1,100)]
            Views  = 99999,          // viola [Range(0,10000)]
            Tags   = invalidTags     // sem constraint DA → não gera erro
        };
        _fvInvalid = new CreatePostDtoFluent
        {
            Title  = "",             // viola NotEmpty
            Body   = "",             // viola NotEmpty
            UserId = 0,              // viola InclusiveBetween(1,100)
            Views  = 99999,          // viola InclusiveBetween(0,10000)
            Tags   = invalidTags     // viola Must(t => t.Count <= 5)
        };
    }

    // -----------------------------------------------------------------
    // DATA ANNOTATIONS
    // Simula o que [ApiController] faz internamente antes de chamar o action.
    // -----------------------------------------------------------------

    [Benchmark(Baseline = true, Description = "DA  | válido")]
    public bool DataAnnotations_Valid()
    {
        var ctx = new ValidationContext(_daValid);
        return Validator.TryValidateObject(_daValid, ctx, null, true);
    }

    [Benchmark(Description = "DA  | inválido")]
    public bool DataAnnotations_Invalid()
    {
        _daErrors.Clear();
        var ctx = new ValidationContext(_daInvalid);
        return Validator.TryValidateObject(_daInvalid, ctx, _daErrors, validateAllProperties: true);
    }

    // -----------------------------------------------------------------
    // FLUENTVALIDATION
    // Simula a chamada manual feita dentro do PostsController.Create().
    // -----------------------------------------------------------------

    [Benchmark(Description = "FV  | válido")]
    public bool FluentValidation_Valid()
        => _fluentValidator.Validate(_fvValid).IsValid;

    [Benchmark(Description = "FV  | inválido")]
    public bool FluentValidation_Invalid()
        => _fluentValidator.Validate(_fvInvalid).IsValid;
}


// =================================================================
// CONFIG
// =================================================================

/// <summary>
/// WarmupCount(3)  → 3 iterações de aquecimento (JIT, branch prediction).
/// IterationCount(15) → 15 iterações de medição para média estável.
///
/// Para um relatório final de produção, troque por Job.LongRun
/// (mais iterações, mais confiança estatística).
/// </summary>
public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.Default
            .WithWarmupCount(3)
            .WithIterationCount(15));
    }
}
