using System;
using System.Collections.Generic;

namespace Festival.DAL.Entity
{
    public record PerformanceEntity : EntityBase
    {
        public DateTime? StartPerformanceTime { get; set; }
        public DateTime? EndPerformanceTime { get; set; }
        public Guid BandId { get; set; }
        public BandEntity? Band { get; set; }
        public Guid StageId { get; set; }
        public StageEntity? Stage { get; set; }

        private sealed class PerformanceEntityEqualityComparer : IEqualityComparer<PerformanceEntity>
        {
            public bool Equals(PerformanceEntity? x, PerformanceEntity? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.StartPerformanceTime.Equals(y.StartPerformanceTime)
                       && x.EndPerformanceTime.Equals(y.EndPerformanceTime)
                       && x.BandId.Equals(y.BandId)
                       && x.StageId.Equals(y.StageId);
            }

            public int GetHashCode(PerformanceEntity obj)
                => HashCode.Combine(obj.StartPerformanceTime, obj.EndPerformanceTime, obj.BandId, obj.StageId);
        }

        public static IEqualityComparer<PerformanceEntity> PerformanceEntityComparer { get; } = new PerformanceEntityEqualityComparer();
    } 
}
