using System;

namespace Models.Abstracts
{
    public abstract class ABaseEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
    }
}
