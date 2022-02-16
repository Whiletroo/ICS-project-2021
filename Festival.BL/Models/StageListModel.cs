using System;

namespace Festival.BL.Models
{
    public record StageListModel : ModelBase
    {
        public string Name { get; set; } = null!;
    }
}
