using System;
using System.IO;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace build
{
    class Program
    {
        private const string ArtifactsDir = "artifacts";
        private const string Clean = "clean";
        private const string Build = "build";
        private const string Test = "test";
        private const string Pack = "pack";
        private const string Publish = "publish";

        static void Main(string[] args)
        {
            Target(Clean, () =>
            {
                if (Directory.Exists(ArtifactsDir))
                {
                    Directory.Delete(ArtifactsDir, true);
                }
                Directory.CreateDirectory(ArtifactsDir);
            });

            Target(Build, () => Run("dotnet", "build HttpOverrides.sln -c Release -p:ContinuousIntegrationBuild=true"));

            Target(
                Test,
                DependsOn(Build),
                () => Run("dotnet", $"test src/HttpOverrides.Tests/HttpOverrides.Tests.csproj -c Release -r {ArtifactsDir} --no-build -l trx;LogFileName=HttpOverrides.Tests.xml --verbosity=normal"));

            Target(
                Pack,
                DependsOn(Build),
                () => Run("dotnet", $"pack src/HttpOverrides/HttpOverrides.csproj -c Release -o {ArtifactsDir} --no-build"));

            Target(Publish, DependsOn(Pack), () =>
            {
                var packagesToPush = Directory.GetFiles(ArtifactsDir, "*.nupkg", SearchOption.TopDirectoryOnly);
                Console.WriteLine($"Found packages to publish: {string.Join("; ", packagesToPush)}");

                var apiKey = Environment.GetEnvironmentVariable("FEEDZ_PROXYKIT_API_KEY");
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    Console.WriteLine($"Feedz API Key available ({apiKey.Substring(0,5)}). Pushing packages to Feedz...");
                    foreach (var packageToPush in packagesToPush)
                    {
                        Run("dotnet", $"nuget push {packageToPush} -s https://f.feedz.io/dh/oss-ci/nuget/index.json --api-key {apiKey} --skip-duplicate", noEcho: true);
                    }
                }
            });

            Target("default", DependsOn(Clean, Test, Publish));

            RunTargetsAndExit(args);
        }
    }
}
