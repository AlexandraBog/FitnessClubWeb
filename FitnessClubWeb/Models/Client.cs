namespace FitnessClubWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Visitings = new HashSet<Visiting>();
        }

        public int ID { get; set; }

        [Required]
        public string FIO { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDay { get; set; }

        public int SubscriptionID { get; set; }

        [ForeignKey("SubscriptionID")]
        public virtual Subscription Subscription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visiting> Visitings { get; set; }
    }
}
