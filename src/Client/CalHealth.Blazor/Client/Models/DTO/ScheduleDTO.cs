using System;

namespace CalHealth.Blazor.Client.Models
{
    public class ScheduleDTO
    {
        public ScheduleDTO()
        {
            ConsultantId = 0;
            TimeSlotId = 0;
            Date = DateTime.Today;
        }
        
        public int ConsultantId;
        public int TimeSlotId { get; set; }
        public DateTime Date { get; set; }
        
        public string ConsultantIdProxy
        {
            get => ConsultantId.ToString();
            set
            {
                if (int.TryParse(value, out var i))
                {
                    ConsultantId = i;
                }
                else
                {
                    {
                        throw new ArgumentException(nameof(value));
                    }
                }
            } 
        }
    }
}