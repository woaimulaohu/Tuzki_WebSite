namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TAG_INFO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TAG_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string TAG_NAME { get; set; }

        public int? COUNT { get; set; }
    }
}
