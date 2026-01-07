using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.Models.Configuration;

public partial class UserConfiguration
{
    public int Id { get; set; }

    [MaxLength(450)]
    public string? Userid { get; set; }

    [MaxLength(100)]
    public string? Enviromentname { get; set; }

    public int Companydefault { get; set; }

    public int Companyclientdefault { get; set; }

    public int Locationdefault { get; set; }

    public int Bindcr1default { get; set; }

    public int Wmsbin { get; set; }

    public int Divisiondefault { get; set; }

    public bool Isactive { get; set; }

    public string? Binesdistribution { get; set; }

    public int Concepttransferdefault { get; set; }

    public int Transactiontransferdefault { get; set; }

    public bool AllowChangeSettings { get; set; }

    public bool Deleteonlymytran { get; set; }

    public DateTime Datecreated { get; set; }

    public bool Processonlymytran { get; set; }

    public string? Roleid { get; set; }

    public bool Isdepot { get; set; }
}
