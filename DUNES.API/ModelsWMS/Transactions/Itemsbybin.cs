using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Transactions;

public partial class Itemsbybin
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string Idcompanyclient { get; set; } = null!;

    public int BinesId { get; set; }

    public string Itemid { get; set; } = null!;
}
