namespace MyWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MENU")]
    public partial class MENU
    {
        [Key]
        public int MENU_ID { get; set; }

        public int? MENU_SERIAL { get; set; }

        [StringLength(50)]
        public string MENU_NAME { get; set; }

        [StringLength(50)]
        public string GRID_PAGE_NAME { get; set; }

        [StringLength(50)]
        public string MENU_ICON { get; set; }
    }
}
