// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageActionTarget.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The Microsoft Teams message action target class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The Microsoft Teams message action target class.
    /// </summary>
    public class MicrosoftTeamsMessageActionTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsMessageActionTarget"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="operatingSystem">The operating system.</param>
        public MicrosoftTeamsMessageActionTarget(string uri, string operatingSystem = "default")
        {
            this.OperatingSystem = operatingSystem;
            this.Uri = uri;
        }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the operating system.
        /// </summary>
        [JsonPropertyName("os")]
        public string OperatingSystem { get; set; }
    }
}