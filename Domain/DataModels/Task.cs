using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DataModels
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? AssignedToId { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        [ForeignKey("AssignedToId")]
        public Member Member { get; set; }
    }
}
