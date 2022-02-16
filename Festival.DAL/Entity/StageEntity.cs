using System;
using System.Collections.Generic;
using Nemesis.Essentials.Design;

namespace Festival.DAL.Entity
{
    public record StageEntity : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? PhotoURL { get; set; }
        public string? ShortDescription { get; set; }
        public ICollection<PerformanceEntity>? Performances { get; set; } = new ValueCollection<PerformanceEntity>(PerformanceEntity.PerformanceEntityComparer);
    }
}
