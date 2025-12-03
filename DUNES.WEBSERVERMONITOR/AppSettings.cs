using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.WEBSERVERMONITOR
{
    public class AppSettings
    {
        public string LogDir { get; set; } = string.Empty;
        public string OutDir { get; set; } = string.Empty;

        public string ApiBaseUrl { get; set; } = string.Empty;
        public string EndpointPath { get; set; } = string.Empty;
        public string DefaultSource { get; set; } = "DBK-WEB";

        public bool HasHeader { get; set; } = false;

        /// <summary>
        /// 0 = hoy, 1 = ayer, 2 = anteayer, etc.
        /// Se ignora si DateText tiene valor.
        /// </summary>
        public int UseDaysBack { get; set; } = 0;

        /// <summary>
        /// "hoy", "ayer", "anteayer" o fechas tipo "2025-11-10".
        /// Si está vacío, se usa UseDaysBack.
        /// </summary>
        public string? DateText { get; set; }
    }
}
