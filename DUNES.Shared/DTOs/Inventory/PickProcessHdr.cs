using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class PickProcessHdr
    {
        public int Id { get; set; }

        public int ConsignRequestID { get; set; }

        [MaxLength(100)]
        public string? Batchid { get; set; }

        [MaxLength(100)]
        public string? HeaderId { get; set; }

        [MaxLength(100)]
        public string? DeliveryId { get; set; }


        [MaxLength(240)]
        public string? Address1 { get; set; }

        [MaxLength(240)]
        public string? Address2 { get; set; }

        [MaxLength(240)]
        public string? Address3 { get; set; }

        [MaxLength(240)]
        public string? Address4 { get; set; }



        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? ZipCode { get; set; }

        public DateTime DateCreated { get; set; }

        [MaxLength(240)]
        public string? Carrier { get; set; }

        [MaxLength(240)]
        public string? ShipMethod { get; set; }

        public DateTime DateTimeProcessed { get; set; }

        public int OutConsReqsId { get; set; }

        public DateTime DateTimeConfirmed { get; set; }

        public DateTime DateTimeOnlineOrders { get; set; }

        public DateTime DateTimeError { get; set; }

        public string? ErrorMsg { get; set; }

        public string? OnlineOrdersErrorMsg { get; set; }

        public DateTime DateTimeOnlineOrdersError { get; set; }

        public int ShipOutConsReqsId { get; set; }

        public DateTime ShipDateTimeConfirmed { get; set; }

        public string? ShipErrorMsg { get; set; }

        public DateTime ShipDateTimeError { get; set; }
    }
}
