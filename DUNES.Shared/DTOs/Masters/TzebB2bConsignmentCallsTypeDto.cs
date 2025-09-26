using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Masters
{
    public class TzebB2bConsignmentCallsTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Attr1 { get; set; }

        public string? Attr2 { get; set; }

       
    }
}
