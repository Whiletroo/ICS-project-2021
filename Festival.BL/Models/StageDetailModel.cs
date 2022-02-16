using System;
using System.Collections.Generic;
using Nemesis.Essentials.Design;

namespace Festival.BL.Models
{
    public record StageDetailModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public string? PhotoURL { get; set; }
        public string? ShortDescription { get; set; }

        public ICollection<PerformanceDetailModel> Performances { get; set; } = new ValueCollection<PerformanceDetailModel>(equalityComparer: PerformanceDetailModel.PerformanceDetailModelComparer);
    }
}
