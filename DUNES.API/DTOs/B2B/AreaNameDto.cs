using System.ComponentModel.DataAnnotations;

namespace DUNES.API.DTOs.B2B
{

    /// <summary>
    /// Area Name
    /// </summary>
    public class AreaNameDto
    {
        /// <summary>
        /// area name
        /// </summary>
        [Key]
        public string area { get; set; } = string.Empty;
    }
}
