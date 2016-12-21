namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POST_CONTENT
    {
        [Key]
        public int POST_ID { get; set; }

        [Column("POST_CONTENT", TypeName = "text")]
        [Required]
        public string POST_CONTENT1 { get; set; }
    }
}
