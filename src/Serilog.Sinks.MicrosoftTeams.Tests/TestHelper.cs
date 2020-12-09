// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A helper class for the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Net.Http.Server;

    using System.Text.Json;
    
    using Serilog.Events;

    /// <summary>
    /// A helper class for the tests.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class TestHelper
    {
        /// <summary>
        /// The test web hook URL.
        /// </summary>
        private const string TestWebHook = "http://localhost:1234/webhook";

        /// <summary>
        /// Adds a timeout to the functionality.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="taskToComplete">The task to run.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        private static async Task<T> WithTimeout<T>(this Task<T> taskToComplete, TimeSpan timeSpan)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var delayTask = Task.Delay(timeSpan, timeoutCancellationTokenSource.Token);
            var completedTask = await Task.WhenAny(taskToComplete, delayTask).ConfigureAwait(false);

            if (completedTask == delayTask)
            {
                throw new TimeoutException($"WithTimeout has timed out after {timeSpan}.");
            }

            timeoutCancellationTokenSource.Cancel();
            return await taskToComplete.ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="omitPropertiesSection">A value indicating whether the properties should be omitted or not.</param>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLogger(bool omitPropertiesSection = false)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", omitPropertiesSection: omitPropertiesSection))
                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Creates the logger with buttons..
        /// </summary>
        /// <param name="buttons">´The buttons to output</param>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLoggerWithButtons(IEnumerable<MicrosoftTeamsSinkOptionsButton> buttons)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", buttons: buttons, omitPropertiesSection: true))
                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Captures the requests.
        /// </summary>
        /// <param name="count">The counter variable.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        public static async Task<IList<JsonElement>> CaptureRequestsAsync(int count)
        {
            var settings = new WebListenerSettings();
            settings.UrlPrefixes.Add(TestWebHook);

            var result = new List<JsonElement>();
            using var listener = new WebListener(settings);
            listener.Start();

            while (count-- > 0)
            {
                using var requestContext = await listener.AcceptAsync().WithTimeout(TimeSpan.FromSeconds(6)).ConfigureAwait(false);
                var body = ReadBodyStream(requestContext.Request.Body);
                result.Add(body);
                requestContext.Response.StatusCode = 204;
            }

            return result;
        }

        /// <summary>
        /// Reads the body stream.
        /// </summary>
        /// <param name="stream">The body stream.</param>
        /// <returns>A <see cref="JsonElement"/> from the body stream.</returns>
        private static JsonElement ReadBodyStream(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<JsonElement>(json);
        }

        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="renderedMessage">The rendered message.</param>
        /// <param name="logEventLevel">The log event level.</param>
        /// <param name="color">The color.</param>
        /// <param name="counter">The counter.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <returns>A <see cref="JsonElement"/> from the message.</returns>
        public static string CreateMessage(string template, string renderedMessage, LogEventLevel logEventLevel,
            string color, int counter, string occurredOn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(ms))
                {

                    writer.WriteStartObject();
                    writer.WriteString("@type", "MessageCard");
                    writer.WriteString("@context", "http://schema.org/extensions");
                    writer.WriteString("title", "Integration Tests");
                    writer.WriteString("text", renderedMessage);
                    writer.WriteString("themeColor", color);
                    writer.WriteStartArray("sections");
                    writer.WriteStartObject();
                    writer.WriteString("title", "Properties");
                    writer.WriteStartArray("facts");
                    writer.WriteStartObject();
                    writer.WriteString("name", "Level");
                    writer.WriteString("value", logEventLevel.ToString());
                    writer.WriteEndObject();
                    writer.WriteStartObject();
                    writer.WriteString("name", "MessageTemplate");
                    writer.WriteString("value", template);
                    writer.WriteEndObject();
                    writer.WriteStartObject();
                    writer.WriteString("name", "counter");
                    writer.WriteString("value", counter.ToString());
                    writer.WriteEndObject();
                    writer.WriteStartObject();
                    writer.WriteString("name", "Occurred on");
                    writer.WriteString("value", occurredOn);
                    writer.WriteEndObject();
                    writer.WriteEndArray();  // end facts
                    writer.WriteEndObject();
                    writer.WriteEndArray();  // end sections
                    writer.WriteEndObject();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="renderedMessage">The rendered message.</param>
        /// <param name="color">The color.</param>
        /// <returns>A new <see cref="JsonElement"/> representing the message.</returns>
        public static string CreateMessage(string renderedMessage, string color)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(ms))
                {
                    writer.WriteStartObject();
                    writer.WriteString("@type", "MessageCard");
                    writer.WriteString("@context", "http://schema.org/extensions");
                    writer.WriteString("title", "Integration Tests");
                    writer.WriteString("text", renderedMessage);
                    writer.WriteString("themeColor", color);
                    writer.WriteEndObject();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Creates a message with buttons.
        /// </summary>
        /// <param name="renderedMessage">The rendered message.</param>
        /// <param name="color">The color.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>A new <see cref="JsonElement"/> representing the message.</returns>
        public static string CreateMessageWithButton(string renderedMessage, string color, IEnumerable<MicrosoftTeamsSinkOptionsButton> buttons)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(ms))
                {
                    writer.WriteStartObject();
                    writer.WriteString("@type", "MessageCard");
                    writer.WriteString("@context", "http://schema.org/extensions");
                    writer.WriteString("title", "Integration Tests");
                    writer.WriteString("text", renderedMessage);
                    writer.WriteString("themeColor", color);
                    writer.WriteStartArray("potentialAction");
                    foreach (var microsoftTeamsSinkOptionsButton in buttons)
                    {
                        writer.WriteStartObject();
                        writer.WriteString("@type", "OpenUri");
                        writer.WriteString("name", microsoftTeamsSinkOptionsButton.Name);
                        writer.WriteStartArray("targets");
                        writer.WriteStartObject();
                        writer.WriteString("uri", microsoftTeamsSinkOptionsButton.Uri);
                        writer.WriteString("os", "default");
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}