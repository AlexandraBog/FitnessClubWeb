namespace FitnessClubWeb.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
  

    [Table("Visiting")]
    public partial class Visiting
    {
        /// <summary>
        /// класс, который хранит информацию о посещени€х
        /// </summary>
        public int ID { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? FinishTime { get; set; }

        public int ClientID { get; set; }

        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }
    }
}
