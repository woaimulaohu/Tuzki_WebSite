namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class COMMENTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMMENTS()
        {
            COMMENTS1 = new HashSet<COMMENTS>();
        }

        [Key]
        public int COMMENTS_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DATE { get; set; }

        [StringLength(10)]
        public string NICK_NAME { get; set; }

        [Required]
        [StringLength(500)]
        public string TEXT { get; set; }

        [StringLength(100)]
        public string EMAIL { get; set; }

        public int POST_ID { get; set; }

        public int? BEFOR_COMMENTS_ID { get; set; }

        [Column(TypeName = "text")]
        public string AVATAR_URL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENTS> COMMENTS1 { get; set; }

        public virtual COMMENTS COMMENTS2 { get; set; }
    }
}
