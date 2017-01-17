namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class COMMENTS
    {
        [Key]
        public int COMMENTS_ID { get; set; }

        public DateTime DATE { get; set; }

        [StringLength(30)]
        public string NICK_NAME { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string TEXT { get; set; }

        [StringLength(100)]
        public string EMAIL { get; set; }

        public int POST_ID { get; set; }

        public int? BEFOR_COMMENTS_ID { get; set; }

        [Column(TypeName = "text")]
        public string AVATAR_URL { get; set; }
    }
}
