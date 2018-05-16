using System;

namespace Models.Abstracts
{
    public abstract class ABaseEntity
    {
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
    }
}
