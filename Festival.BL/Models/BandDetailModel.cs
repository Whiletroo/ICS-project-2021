using System;
using System.Collections.Generic;
using Nemesis.Essentials.Design;
using Festival.DAL.Entity;

namespace Festival.BL.Models
{
    public record BandDetailModel: ModelBase
    {
        public string Name { get; set; } = null!;
        public string? PhotoURL { get; set; }
        public Genres Genre { get; set; }
        public Countries OriginCountry { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }

        public ICollection<PerformanceDetailModel> Performances { get; set; } = new ValueCollection<PerformanceDetailModel>(equalityComparer: PerformanceDetailModel.PerformanceDetailModelComparer);
    }
}
