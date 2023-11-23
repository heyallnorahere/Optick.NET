using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Optick.NET.RedistBuilder
{
    // code basically stolen from my own project
    // sowwy
    public static class Utilities
    {
        public static string GenerateBuildRuntimeIdentifier()
        {
            string platform;
            bool isOSX = false;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "win";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "osx";
                isOSX = true;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux";
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            var arch = RuntimeInformation.OSArchitecture.ToString();
            return isOSX ? platform : $"{platform}-{arch.ToLower()}";
        }

        public static string GetSharedLibraryName(string name)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"{name}.dll";
            }
            else
            {
                return $"lib{name}.{(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "dylib" : "so")}";
            }
        }

        private static async Task RelayTextAsync(TextReader input, params Action<string>[] callbacks)
        {
            while (true)
            {
                var line = await input.ReadLineAsync();
                if (line is null)
                {
                    return;
                }

                foreach (var callback in callbacks)
                {
                    callback.Invoke(line);
                }
            }
        }

        public static int RunCommand(string command, string? cwd = null, Action<string>? onLine = null, bool dryRun = false)
        {
            Console.WriteLine($">{command}");
            if (dryRun)
            {
                return 0;
            }

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = isWindows ? "cmd.exe" : "/bin/bash",
                    UseShellExecute = false,
                    WorkingDirectory = cwd ?? Environment.CurrentDirectory,
                    RedirectStandardOutput = onLine != null,
                    RedirectStandardError = onLine != null
                }
            };

            process.StartInfo.ArgumentList.Add(isWindows ? "/c" : "-c");
            process.StartInfo.ArgumentList.Add(command);
            process.Start();

            var tasks = new List<Task>();
            if (onLine != null)
            {
                var readers = new TextReader[]
                {
                    process.StandardOutput,
                    process.StandardError
                };

                foreach (var reader in readers)
                {
                    tasks.Add(Task.Run(async () => await RelayTextAsync(reader, Console.WriteLine, onLine)));
                }
            }

            tasks.Add(process.WaitForExitAsync());
            Task.WhenAll(tasks).Wait(); // :)

            return process.ExitCode;
        }
    }
}