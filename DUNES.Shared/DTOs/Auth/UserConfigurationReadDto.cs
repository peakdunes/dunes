using System;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Read DTO for UI (includes display names for related entities).
    /// </summary>
    public class UserConfigurationReadDto
    {
        public int Id { get; set; }
        public string? Userid { get; set; }

        public string? Enviromentname { get; set; }
        public bool Isactive { get; set; }
        public DateTime Datecreated { get; set; }

        // Company
        public int Companydefault { get; set; }
        public string? CompanyName { get; set; }

        // Company Client
        public int Companyclientdefault { get; set; }
        public string? CompanyClientName { get; set; }

        // Location (optional display)
        public int Locationdefault { get; set; }
        public string? LocationName { get; set; }

        // Bin (optional display)
        public int Wmsbin { get; set; }
        public string? WmsBinName { get; set; }

        // Division (if you later map it to a catalog)
        public int Divisiondefault { get; set; }
        public string? DivisionName { get; set; }

        // Role (optional display)
        public string? Roleid { get; set; }
        public string? RoleName { get; set; }

        public bool Isdepot { get; set; }

        public string? Binesdistribution { get; set; }

        public int Concepttransferdefault { get; set; }
        public int Transactiontransferdefault { get; set; }

        public bool AllowChangeSettings { get; set; }
        public bool Deleteonlymytran { get; set; }
        public bool Processonlymytran { get; set; }
    }
}
