using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Masters
{
    /// <summary>
    /// mvcgeneralparameter dto
    /// </summary>
    public class MvcGeneralParametersDto
    {
        public int Id { get; set; }

        public int ParameterNumber { get; set; }

        public string? ParameterArea { get; set; }

        public string? ParameterDescription { get; set; }

        public string? ParameterValue { get; set; }
    }
}
