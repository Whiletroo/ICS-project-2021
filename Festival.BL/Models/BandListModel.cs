using System;
using Festival.DAL.Entity;

namespace Festival.BL.Models
{
    public record BandListModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public Genres Genre { get; set; }
        public string? ShortDescription { get; set; }
    }
}
