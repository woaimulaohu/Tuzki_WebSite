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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [StringLength(50)]
        public string KEY_NAME { get; set; }

        [Required]
        [StringLength(50)]
        public string VALUE { get; set; }
    }
}
