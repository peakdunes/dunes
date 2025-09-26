using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Masters
{

    /// <summary>
    /// DTO table WmsCompanyclient
    /// </summary>
    public class WmsCompanyclientDto
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; } = string.Empty;
    }
}
