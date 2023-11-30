using System;
using System.Collections.Generic;
using System.Threading;

namespace Optick.NET.Example
{
    internal static class Program
    {
        private static readonly IReadOnlyDictionary<ConsoleKey, Action> sKeyEvents;
        private static bool sShouldQuit;
        private static ulong sFrameIndex;

        static Program()
        {
            sShouldQuit = false;
            sFrameIndex = 0;

            sKeyEvents = new Dictionary<ConsoleKey, Action>
            {
                [ConsoleKey.Q] = () =>
                {
                    using var quitEvent = OptickMacros.Event("Quit");
                    sShouldQuit = true;
                },
                [ConsoleKey.T] = () =>
                {
                    using var timeEvent = OptickMacros.Event("TimeRequested");
                    Console.WriteLine(DateTime.Now);
                }
            };
        }

        private static bool OnStateChanged(State state)
        {
            switch (state)
            {
                case State.STOP_CAPTURE:
                    OptickImports.AttachSummary("Last frame", sFrameIndex.ToString());
                    break;
            }

            return true;
        }

        private static void Print()
        {
            using var printEvent = OptickMacros.Event();
            OptickMacros.Tag("Frame index", sFrameIndex);

            Console.WriteLine($"Frame {sFrameIndex++}");
        }

        public static void Main(string[] args)
        {
            OptickImports.SetStateChangedCallback(OnStateChanged);

#if DEBUG
            OptickMacros.IsDebug = true;
#endif

            using (var app = new OptickApp("ExampleApp"))
            {
                const string threadName = "MainThread";
                using var scope = new ThreadScope(threadName);

                while (!sShouldQuit)
                {
                    using var frameEvent = OptickMacros.Frame(threadName);

                    using (OptickMacros.Category("Update", Category.GameLogic))
                    {
                        double start = OptickImports.GetHighPrecisionTime();
                        Print();
                        double end = OptickImports.GetHighPrecisionTime();

                        using (OptickMacros.Category("Wait", Category.Wait))
                        {
                            double duration = start - end;
                            double frequency = OptickImports.GetHighPrecisionFrequency();
                            double sleepDuration = (frequency / 60) - duration;

                            double sleep = sleepDuration * 1000 / frequency;
                            Thread.Sleep((int)Math.Max(sleep, 0));
                        }
                    }

                    while (!Console.IsInputRedirected && Console.KeyAvailable)
                    {
                        using var keyPressedEvent = OptickMacros.Category("Key pressed", Category.Input);

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

                OptickImports.Update();
            }

            OptickImports.Shutdown();
        }
    }
}