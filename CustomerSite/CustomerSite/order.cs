namespace CustomerSite
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("order")]
    public partial class order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int order_pk { get; set; }

        public int customer_pk { get; set; }

        public int product_pk { get; set; }

        public int order_quantity { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual customer customer { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual product product { get; set; }
    }
}
