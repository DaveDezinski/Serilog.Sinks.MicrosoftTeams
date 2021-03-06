﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageCard.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The teams message card.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    using System.Collections.Generic;

    using System.Text.Json.Serialization;

    /// <summary>
    /// The teams message card.
    /// </summary>
    internal class MicrosoftTeamsMessageCard
    {
        /// <summary>
        /// Gets the type of the card.
        /// </summary>
        [JsonPropertyName("@type")]
        public string Type { get; } = "MessageCard";

        /// <summary>
        /// Gets the context of the card.
        /// </summary>
        [JsonPropertyName("@context")]
        public string Context { get; } = "http://schema.org/extensions";

        /// <summary>
        /// Gets or sets the title of the card.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text of the card.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the theme color of the card.
        /// </summary>
        [JsonPropertyName("themeColor")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the sections of the card.
        /// </summary>
        [JsonPropertyName("sections")]
        public IList<MicrosoftTeamsMessageSection> Sections { get; set; }

        /// <summary>
        /// Gets or sets the potential action buttons.
        /// </summary>
        [JsonPropertyName("potentialAction")]
        public IList<MicrosoftTeamsMessageAction> PotentialActions { get; set; }
    }
}