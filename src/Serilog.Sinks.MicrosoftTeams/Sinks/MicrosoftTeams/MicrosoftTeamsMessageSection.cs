﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageSection.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The message section.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    using System.Collections.Generic;

    using System.Text.Json.Serialization;

    /// <summary>
    /// The message section.
    /// </summary>
    internal class MicrosoftTeamsMessageSection
    {
        /// <summary>
        /// Gets or sets message section title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets message section facts.
        /// </summary>
        [JsonPropertyName("facts")]
        public IList<MicrosoftTeamsMessageFact> Facts { get; set; }
    }
}