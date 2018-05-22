using System;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public abstract class ABaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Modified { get; set; }
    }
}
