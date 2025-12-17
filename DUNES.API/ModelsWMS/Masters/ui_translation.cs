using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// multilanguage 
    /// </summary>
    public class ui_translation
    {
        /// <summary>
        /// id
        /// </summary>
       public int Id { get; set; }
        /// <summary>
        /// language
        /// </summary>
        [MaxLength(5)]
        public required string Lang { get; set; }

        /// <summary>
        /// key
        /// </summary>
        [MaxLength(200)]
        public required string TKey { get; set; }


        /// <summary>
        /// text by language
        /// </summary>
        [MaxLength(2000)]
        public required string TValue { get; set; }  
    }
}
