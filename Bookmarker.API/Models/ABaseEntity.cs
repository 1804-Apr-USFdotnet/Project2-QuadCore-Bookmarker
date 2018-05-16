using System;

namespace Models
{
    public abstract class ABaseEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
    }
}
