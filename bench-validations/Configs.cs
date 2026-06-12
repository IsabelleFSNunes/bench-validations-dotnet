using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace bench_validations
{

    // =================================================================
    // CONFIG
    // =================================================================

    /// <summary>
    /// WarmupCount(3): 3 warmup cycles (JIT, branch prediction).
    /// IterationCount(15): 15 measurement cycles for statistical stability.
    ///
    /// Use Job.LongRun for production-level reporting.
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

    public sealed class IntegrationBenchmarkConfig : ManualConfig
    {
        public IntegrationBenchmarkConfig()
        {
            AddJob(Job.Default.WithWarmupCount(2).WithIterationCount(10));
        }
    }

}
