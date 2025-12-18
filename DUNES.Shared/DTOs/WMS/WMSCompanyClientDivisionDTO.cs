using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{

    /// <summary>
    /// dto for CompanyClientDivision model
    /// </summary>
    public class WMSCompanyClientDivisionDTO
    {
        /// <summary>
        /// internal id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Division Name
        /// </summary>
        public string? DivisionName { get; set; }

        /// <summary>
        /// Company client Id
        /// </summary>
        public int Idcompanyclient { get; set; }


        /// <summary>
        /// Active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
