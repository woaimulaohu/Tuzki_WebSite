namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VISIT_IP
    {
        [Required]
        [StringLength(30)]
        public string IP_ADDRESS { get; set; }

        public DateTime SESSION_START_TIME { get; set; }

        public int ID { get; set; }
    }
}
