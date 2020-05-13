using System;
using System.Collections.Generic;

namespace ThetaOnlineStore.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Status { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
