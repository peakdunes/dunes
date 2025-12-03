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

        public string? CompanyId { get; set; }

        public string? Name { get; set; }

        public int Idcountry { get; set; }

        public int Idstate { get; set; }

        public int Idcity { get; set; }

        public string? Zipcode { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public bool Active { get; set; }
    }
}
