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
        public int POST_ID { get; set; }

        public DateTime DATE { get; set; }

        [Required]
        [StringLength(50)]
        public string MAIN_TITLE { get; set; }

        public int? VIEW_COUNT { get; set; }

        public int? REPRODUCED_COUNT { get; set; }

        public int? PRAISE_COUNT { get; set; }

        [StringLength(50)]
        public string TAG_ID { get; set; }

        [StringLength(100)]
        public string SECOND_TITLE { get; set; }

        public bool IS_TOP { get; set; }
    }
}
