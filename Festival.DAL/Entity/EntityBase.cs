using System;

namespace Festival.DAL.Entity
{
    public abstract record EntityBase
    {
        public Guid Id { get; init; }
    }
}
