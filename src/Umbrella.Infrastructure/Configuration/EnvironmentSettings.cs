using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Configuration
{
    /// <summary>
    /// Simple class to map Ennvironment definition inside appSettings.json file
    /// </summary>
    public class EnvironmentSettings
    {
        /// <summary>
        /// Code of Environment
        /// </summary>
        /// <value></value>
        public string Code { get; set; }
        /// <summary>
        /// Display name of environment
        /// </summary>
        /// <value></value>
        public string DisplayName { get; set; }
        /// <summary>
        /// ID of hosting cloud project
        /// </summary>
        /// <value></value>
        public string? CloudProjectId { get; set; }
        /// <summary>
        /// TRUE if it is deployed on clound, FLASE for on premise
        /// </summary>
        /// <value></value>
        public bool IsDeployedOnCloud { get; set; }

        /// <summary>
        /// Default Constr
        /// </summary>
        public EnvironmentSettings()
        {
            this.Code = "localhost";
            this.DisplayName = "Localhost";
            this.CloudProjectId = "";
            this.IsDeployedOnCloud = false;
        }
    }
}