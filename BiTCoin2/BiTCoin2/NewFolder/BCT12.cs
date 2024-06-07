using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BiTCoin2.NewFolder
{
    public class BCT12
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public double LastPrices { get; set; }

        [ForeignKey("Source")] // Specify the foreign key relationship
        public int SourceID { get; set; }

        public DateTime timestamp { get; set; }
    }
}
