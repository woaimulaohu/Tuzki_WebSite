namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_INFO
    {
        [Required]
        [StringLength(100)]
        public string SESSION_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string NICK_NAME { get; set; }

        [Required]
        [StringLength(200)]
        public string AVATAR_URL { get; set; }

        [Key]
        public int USER_ID { get; set; }

        public int USER_AUTH { get; set; }

        public DateTime EXPIRE_TIME { get; set; }
    }
}
