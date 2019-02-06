namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class StandardJob
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StandardJob()
        {
            StandardJobParts = new HashSet<StandardJobPart>();
        }

        public int StandardJobID { get; set; }

        [Required(ErrorMessage ="Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot be longer then 100 characters")]
        public string Description { get; set; }

        public decimal StandardHours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StandardJobPart> StandardJobParts { get; set; }
    }
}
