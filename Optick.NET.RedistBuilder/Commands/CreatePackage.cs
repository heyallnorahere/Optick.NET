using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Optick.NET.RedistBuilder.Commands
{
    [RegisteredCommand("create-package")]
    internal sealed class CreatePackage : ICommand
    {
        private static void ExtractAllArchivesInDirectory(string artifactsDirectory, string outputDirectory)
        {
            var buffer = new byte[256];
            var artifacts = Directory.GetFiles(artifactsDirectory, "artifact-*.zip");

            if (artifacts.Length == 0)
            {
                throw new InvalidOperationException("No archives found to extract!");
            }

            foreach (var artifact in artifacts)
            {
                using var archive = ZipFile.Open(artifact, ZipArchiveMode.Read);
                foreach (var entry in archive.Entries)
                {
                    var entryPath = entry.FullName;

                    var directoryPath = Path.GetDirectoryName(entryPath);
                    if (directoryPath is not null)
                    {
                        Directory.CreateDirectory(Path.Join(outputDirectory, directoryPath));
                    }

                    string resultPath = Path.Join(outputDirectory, entryPath);
                    using var outputStream = new FileStream(resultPath, FileMode.Create, FileAccess.Write);
                    using var inputStream = entry.Open();

                    while (true)
                    {
                        int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead <= 0)
                        {
                            break;
                        }

                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public void Invoke(string[] args)
        {
            string artifactsDirectory = Path.Join(Environment.CurrentDirectory, "artifacts");
            string packageDirectory = Path.Join(artifactsDirectory, "package");

            if (Directory.Exists(packageDirectory))
            {
                Directory.Delete(packageDirectory, true);
            }

            Directory.CreateDirectory(packageDirectory);
            ExtractAllArchivesInDirectory(artifactsDirectory, packageDirectory);

            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version ?? new Version(1, 0, 0, 0);

            var builder = new PackageBuilder
            {
                Id = "Optick.NET.Redist",
                Version = new NuGetVersion(version),
                Description = "Optick redistributable binaries for specific platforms",
                Readme = "README.md",
                Repository = new RepositoryMetadata
                {
                    Type = "git",
                    Url = "https://github.com/yodasoda1219/Optick.NET" // github will associate the package with this repository
                }
            };

            using (var readmeStream = assembly.GetManifestResourceStream("Optick.NET.RedistBuilder.Resources.README.md"))
            {
                if (readmeStream is null)
                {
                    throw new FileNotFoundException("Failed to find embedded README resource!");
                }

                string outputReadmePath = Path.Join(packageDirectory, "README.md");
                using var outputStream = new FileStream(outputReadmePath, FileMode.Create, FileAccess.Write);

                var buffer = new byte[256];
                while (true)
                {
                    int bytesRead = readmeStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        break;
                    }

                    outputStream.Write(buffer, 0, bytesRead);
                }
            }

            builder.Authors.Add("Nora Beda");
            builder.AddFiles(packageDirectory, "runtimes/**", "runtimes");
            builder.AddFiles(packageDirectory, "README.md", string.Empty);

            string packageName = $"{builder.Id}.nupkg";
            string packagePath = Path.Join(artifactsDirectory, packageName);

            using (var outputStream = new FileStream(packagePath, FileMode.Create, FileAccess.Write))
            {
                builder.Save(outputStream);
            }

            Console.WriteLine("Successfully built package!");
        }
    }
}