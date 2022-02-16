using System;
using Festival.DAL.Entity;

namespace Festival.BL.Models
{
    public record PerformanceListModel : ModelBase
    {
        public string? BandName { get; set; }
        public string? StageName { get; set; }
        public Genres? Genre { get; set; }
        public DateTime? StartPerformanceTime { get; set; }
        public DateTime? EndPerformanceTime { get; set; }
    }
}
