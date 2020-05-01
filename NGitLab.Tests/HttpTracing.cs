using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;

namespace NGitLab.Tests
{
    /// <summary>
    /// Allows to trace the http calls using the hack described on stackoverflow:
    /// http://stackoverflow.com/questions/1049442/system-net-httpwebrequest-tracing-without-using-files-or-app-config
    /// </summary>
    public static class HttpTracing
    {
        public static void Enable()
        {
            InitializeHttp();
            Logging.HttpTraceSource.Listeners.Clear();

            var listener = new MyListener { Filter = new EventTypeFilter(SourceLevels.All) };

            Logging.HttpTraceSource.Switch.Level = SourceLevels.Information;
            Logging.HttpTraceSource.Listeners.Add(listener);

            Logging.WebTraceSource.Switch.Level = SourceLevels.Information;
            Logging.WebTraceSource.Listeners.Add(listener);
            Logging.IsEnabled = true;
        }

        /// <summary>
        /// Force the initialization of the http module.
        /// </summary>
        private static void InitializeHttp()
        {
            if (!Logging.Initialized)
            {
                var uri = new Uri("http://localhost");
                WebRequest.Create(uri);
                var waitForInitializationThread = new Thread(() =>
                {
                    while (!Logging.Initialized)
                    {
                        Thread.Sleep(100);
                    }
                });

                waitForInitializationThread.Start();
                waitForInitializationThread.Join();
            }
        }

        /// <summary>
        /// Hides the reflection hack.
        /// </summary>
        private static class Logging
        {
            private static Type LoggingType { get; } = typeof(WebRequest).Assembly.GetType("System.Net.Logging");

            private static FieldInfo EnabledField => LoggingType.GetField("s_LoggingEnabled", BindingFlags.NonPublic | BindingFlags.Static);

            private static FieldInfo InitializedField => LoggingType.GetField("s_LoggingInitialized", BindingFlags.NonPublic | BindingFlags.Static);

            public static bool IsEnabled
            {
                get => (bool)EnabledField.GetValue(null);
                set => EnabledField.SetValue(null, value);
            }

            public static bool Initialized
            {
                get => (bool)InitializedField.GetValue(null);
                set => InitializedField.SetValue(null, value);
            }

            public static TraceSource WebTraceSource => GetTraceSource("s_WebTraceSource");

            public static TraceSource HttpTraceSource => GetTraceSource("s_HttpListenerTraceSource");

            public static TraceSource SocketTraceSource => GetTraceSource("s_SocketsTraceSource");

            public static TraceSource CacheTraceSource => GetTraceSource("s_CacheTraceSource");

            private static TraceSource GetTraceSource(string fieldName)
                => (TraceSource)LoggingType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        }

        /// <summary>
        /// Logs in TeaBox output pad.
        /// </summary>
        private sealed class MyListener : TraceListener
        {
            public override void Write(string message)
            {
                Console.WriteLine(message);
                Trace.WriteLine(message);
            }

            public override void WriteLine(string message)
            {
                Console.WriteLine(message);
                Trace.WriteLine(message);
            }
        }
    }
}
