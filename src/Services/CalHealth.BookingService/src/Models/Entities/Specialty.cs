using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Specialty : IEntityBase
    {
        public Specialty()
        {
            Consultant = new HashSet<Consultant>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Consultant> Consultant { get; set; }
    }
}
