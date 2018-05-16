using System;

namespace Bookmarker.Models
{
    public abstract class ABaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
