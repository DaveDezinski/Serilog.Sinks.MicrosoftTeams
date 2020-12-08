﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkOptionsButton.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A class to handle the Microsoft Teams options buttons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// A class to handle the Microsoft Teams options buttons.
    /// </summary>
    public class MicrosoftTeamsSinkOptionsButton
    {
        /// <summary>
        /// Gets or sets the link display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the link URI.
        /// </summary>
        public string Uri { get; set; }
    }
}
