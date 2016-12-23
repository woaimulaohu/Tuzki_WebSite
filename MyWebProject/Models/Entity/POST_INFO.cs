namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POST_INFO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int POST_ID { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime DATE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string TITLE { get; set; }

        public int? VIEW_COUNT { get; set; }

        public int? REPRODUCED_COUNT { get; set; }

        public int? PRAISE_COUNT { get; set; }

        public int? TAG_ID { get; set; }
    }
}
