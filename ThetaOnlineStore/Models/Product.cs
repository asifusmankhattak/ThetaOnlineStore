using System;
using System.Collections.Generic;

namespace ThetaOnlineStore.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int? CurrentStock { get; set; }
        public double? CostPrice { get; set; }
        public double? SalePrice { get; set; }
        public string Images { get; set; }
        public string ProductCode { get; set; }
        public string Status { get; set; }
        public string OpeningStock { get; set; }
        public DateTime? OpeningDate { get; set; }
        public string ProductFeatures { get; set; }
        public string CreatedBy { get; set; }
    }
}
