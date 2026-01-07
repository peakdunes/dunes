using System.ComponentModel.DataAnnotations;

namespace DUNES.Shared.DTOs.Auth
{
    public class UserConfigurationCreateDto
    {
        // Recomendado: NO lo mandes desde UI; setéalo en API con el usuario logueado.
        [MaxLength(450)]
        public string? Userid { get; set; }

        [Required]
        [MaxLength(100)]
        public string Enviromentname { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Companydefault { get; set; }

        [Range(1, int.MaxValue)]
        public int Companyclientdefault { get; set; }

        public int Locationdefault { get; set; }
        public int Bindcr1default { get; set; }
        public int Wmsbin { get; set; }

        public int Divisiondefault { get; set; }

        public bool Isactive { get; set; }

        [MaxLength(1000)]
        public string? Binesdistribution { get; set; }

        public int Concepttransferdefault { get; set; }
        public int Transactiontransferdefault { get; set; }

        public bool AllowChangeSettings { get; set; }
        public bool Deleteonlymytran { get; set; }

        public bool Processonlymytran { get; set; }

        [MaxLength(450)]
        public string? Roleid { get; set; }

        public bool Isdepot { get; set; }
    }
}
