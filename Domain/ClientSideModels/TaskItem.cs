using System;

namespace Domain.ClientSideModels
{
    public class TaskItem
    {
        public Guid referenceId { get; set; }
        public Guid memberId { get; set; }
        public string taskName { get; set; }
        public bool isComplete { get; set; }
    }
}
