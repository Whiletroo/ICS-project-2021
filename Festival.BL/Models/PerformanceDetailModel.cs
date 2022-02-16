using System;
using System.Collections.Generic;
using Festival.DAL.Entity;

namespace Festival.BL.Models
{
    public record PerformanceDetailModel : ModelBase
    {
        public Guid BandId { get; set; }
        public string? BandName { get; set; }

        public Guid StageId { get; set; }
        public string? StageName { get; set; }

        public DateTime? StartPerformanceTime { get; set; }
        public DateTime? EndPerformanceTime { get; set; }

        private sealed class PerformanceDetailModelEqualityComparer : IEqualityComparer<PerformanceDetailModel>
        {
            public bool Equals(PerformanceDetailModel? x, PerformanceDetailModel? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id)
                    && x.StartPerformanceTime.Equals(y.StartPerformanceTime)
                    && x.EndPerformanceTime.Equals(y.EndPerformanceTime)
                    && x.BandId.Equals(y.BandId)
                    && x.StageId.Equals(y.StageId);
            }

            public int GetHashCode(PerformanceDetailModel obj)
            {
                return HashCode.Combine(obj.Id, obj.StartPerformanceTime, obj.EndPerformanceTime, obj.BandId, obj.StageId);
            }
        }

        public static IEqualityComparer<PerformanceDetailModel> PerformanceDetailModelComparer { get; } = new PerformanceDetailModelEqualityComparer();
    }
}
