using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public class Specialty : IEntityBase
    {
        public Specialty()
        {
            Consultant = new HashSet<Consultant>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Consultant> Consultant { get; }
    }
}
