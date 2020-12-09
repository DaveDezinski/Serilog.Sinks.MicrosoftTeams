// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageFact.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The message card fact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The message card fact.
    /// </summary>
    internal class MicrosoftTeamsMessageFact
    {
        /// <summary>
        /// Gets or sets the name of the card fact.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the card fact.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}