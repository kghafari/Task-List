using System;
using System.Collections.Generic;

namespace TaskList_Capstone.Models
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool Complete { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
