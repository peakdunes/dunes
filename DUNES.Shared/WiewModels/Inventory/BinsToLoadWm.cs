using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.WiewModels.Inventory
{

    /// <summary>
    /// Use to load all bines distribution for a ANS process
    /// </summary>
    public class BinsToLoadWm
    {

        public int Id { get; set; }

        public int inventorytype { get; set; }

        [MaxLength(50)]
        public string partnumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string tagname { get; set; } = string.Empty;

        [MaxLength(100)]
        public string typename { get; set; } = string.Empty;

        public int qty { get; set; }

        public int lineid { get; set; }

        public int statusid { get; set; }

        public int binid { get; set; }

        [MaxLength(100)]
        public string statusname { get; set; } = string.Empty;
    }
}
