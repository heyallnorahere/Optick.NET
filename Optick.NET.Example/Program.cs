using System;
using System.Collections.Generic;
using System.Threading;

namespace Optick.NET.Example
{
    internal static class Program
    {
        private static readonly IReadOnlyDictionary<ConsoleKey, Action> sKeyEvents;
        private static bool sShouldQuit;

        static Program()
        {
            sShouldQuit = false;
            sKeyEvents = new Dictionary<ConsoleKey, Action>
            {
                [ConsoleKey.Q] = () =>
                {
                    using var quitEvent = Optick.Event("Quit");
                    sShouldQuit = true;
                },
                [ConsoleKey.T] = () =>
                {
                    using var timeEvent = Optick.Event("TimeRequested");
                    Console.WriteLine(DateTime.Now);
                }
            };
        }

        private static bool OnStateChanged(State state)
        {
            // nothing here currently
            return true;
        }

        public static void Main(string[] args)
        {
            Optick.SetStateChangedCallback(OnStateChanged);

            using (var app = new OptickApp("ExampleApp"))
            {
                const string threadName = "MainThread";
                using var scope = new ThreadScope(threadName);

                ulong frameIndex = 0;
                while (!sShouldQuit)
                {
                    using var frameEvent = Optick.Frame(threadName);

                    using (var updateEvent = Optick.Category("Update", Category.GameLogic))
                    {
                        Console.WriteLine($"Frame {frameIndex++}");
                        Thread.Sleep(1000 / 600);
                    }

                    while (!Console.IsInputRedirected && Console.KeyAvailable)
                    {
                        using var keyPressedEvent = Optick.Category("Key pressed", Category.Input);

                        var key = Console.ReadKey(true);
                        if (sKeyEvents.TryGetValue(key.Key, out Action? handler))
                        {
                            handler.Invoke();
                        }
                        else
                        {
                            Console.WriteLine($"Unrecognized key: {key.Key}");
                        }
                    }
                }
            }

            Optick.Shutdown();
        }
    }
}