using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Optick.NET.RedistBuilder.Commands
{
    internal struct CMakeCacheValue
    {
        public string Value { get; set; }

        public static implicit operator CMakeCacheValue(string value) => new CMakeCacheValue
        {
            Value = value.Replace("\\", "\\\\")
        };

        public static implicit operator CMakeCacheValue(bool value) => new CMakeCacheValue
        {
            Value = value ? "ON" : "OFF"
        };
    }

    [RegisteredCommand("build-artifact")]
    internal sealed class BuildArtifact : ICommand
    {
        private static readonly Dictionary<string, CMakeCacheValue> sCMakeOptions;
        static BuildArtifact()
        {
            sCMakeOptions = new Dictionary<string, CMakeCacheValue>
            {
                ["OPTICK_USE_VULKAN"] = true,
                ["OPTICK_USE_D3D12"] = RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
                ["OPTICK_BUILD_CONSOLE_SAMPLE"] = false,
            };
        }

        private static void BuildOptick(string sourceDirectory, string buildDirectory)
        {
            string cmakeCommand = $"cmake \"{sourceDirectory.Replace("\\", "\\\\")}\" -B \"{buildDirectory.Replace("\\", "\\\\")}\"";
            string buildCommand = $"cmake --build \"{buildDirectory.Replace("\\", "\\\\")}\" --config Release -j 8";

            foreach (var cacheKey in sCMakeOptions.Keys)
            {
                var value = sCMakeOptions[cacheKey].Value;
                cmakeCommand += $" -D{cacheKey}=\"{value}\"";
            }

            int exitCode = Utilities.RunCommand(cmakeCommand, cwd: Environment.CurrentDirectory);
            if (exitCode != 0)
            {
                throw new InvalidOperationException($"CMake configure command exited with code: {exitCode}");
            }

            exitCode = Utilities.RunCommand(buildCommand, cwd: Environment.CurrentDirectory);
            if (exitCode != 0)
            {
                throw new InvalidOperationException($"CMake build command exited with code: {exitCode}");
            }
        }

        private static void CreateArtifact(string buildDirectory)
        {
            string libraryName = Utilities.GetSharedLibraryName("OptickCore");
            string libraryPath = Path.Join(buildDirectory, libraryName);

            string artifactDirectory = Path.Join(Environment.CurrentDirectory, "artifacts");
            Directory.CreateDirectory(artifactDirectory);

            string rid = Utilities.RuntimeIdentifier;
            string zipPath = Path.Join(artifactDirectory, $"artifact-{rid}.zip");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            var entry = archive.CreateEntry($"runtimes/{rid}/native/{libraryName}");

            using var output = entry.Open();
            using var input = new FileStream(libraryPath, FileMode.Open, FileAccess.Read);

            var buffer = new byte[256];
            while (true)
            {
                int countRead = input.Read(buffer, 0, buffer.Length);
                if (countRead <= 0)
                {
                    break;
                }

                output.Write(buffer, 0, countRead);
            }
        }

        public void Invoke(string[] args)
        {
            string sourceDirectory = args.Length > 0 ? args[0] : Path.Join(Environment.CurrentDirectory, "optick");
            if (!Directory.Exists(sourceDirectory))
            {
                throw new DirectoryNotFoundException($"No such directory: {sourceDirectory}");
            }

            string buildDirectory = Path.Join(sourceDirectory, "build");
            BuildOptick(sourceDirectory, buildDirectory);
            CreateArtifact(buildDirectory);

            Console.WriteLine("Successfully built artifact!");
        }
    }
}