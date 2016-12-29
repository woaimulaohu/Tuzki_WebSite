namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONFIG")]
    public partial class CONFIG
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string KEY_NAME { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string VALUE { get; set; }
    }
}
