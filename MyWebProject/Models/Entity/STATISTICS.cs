namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STATISTICS
    {
        [Key]
        [StringLength(50)]
        public string KEY_NAME { get; set; }

        public int VALUE { get; set; }
    }
}
