using System;

namespace Bookmarker.Models
{
    public abstract class ABaseEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
    }
}
