using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Masters
{
    public class TzebB2bMasterPartDefinitionDto
    {
        public int Id { get; set; }

        public string PartNo { get; set; } = null!;

        public string PartDsc { get; set; } = null!;

        public string Serialized { get; set; } = null!;

        public bool Repairable { get; set; }
           

    }
}
