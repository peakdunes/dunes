using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Admin
{
    /// <summary>
    /// Menu table entity (legacy structure: level1..level5).
    /// </summary>
    /// 
    [Table("Menu")]
    public class Menu
    {
        /// <summary>
        /// database id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// code
        /// </summary>
        [MaxLength(10)]
        public string? Code { get; set; }

        /// <summary>
        /// level1
        /// </summary>
        [MaxLength(100)]
        public string? Level1 { get; set; }
        /// <summary>
        /// level1
        /// </summary>
        [MaxLength(100)]
        public string? Level2 { get; set; }
        /// <summary>
        /// level1
        /// </summary>
        [MaxLength(100)]
        public string? Level3 { get; set; }
        /// <summary>
        /// level1
        /// </summary>
        [MaxLength(100)]
        public string? Level4 { get; set; }
        /// <summary>
        /// level1
        /// </summary>
        [MaxLength(100)]
        public string? Level5 { get; set; }

        /// <summary>
        /// Legacy roles column (comma separated). We will stop using it once permissions tables are ready.
        /// </summary>
        [MaxLength(500)]
        public string? Roles { get; set; }

        /// <summary>
        /// is active
        /// </summary>
        public bool Active { get; set; }


        /// <summary>
        /// utility
        /// </summary>
        [MaxLength(500)]
        public string? Utility { get; set; }

        /// <summary>
        /// controller action
        /// </summary>
        [MaxLength(100)]
        public string? Action { get; set; }

        /// <summary>
        /// controller name
        /// </summary>
        [MaxLength(100)]
        public string? Controller { get; set; }

        /// <summary>
        /// Sort/order column.
        /// NOTE: SQL column name is [order] (reserved word), mapped via Fluent API below.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// menu title
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        public string? Title { get; set; }
    }
}
