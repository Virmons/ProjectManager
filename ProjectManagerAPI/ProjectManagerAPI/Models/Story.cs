using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class Story
    {
        public int? ID { get; set; }
        public string StoryName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Theme { get; set; }
        public string Actor { get; set; }
        public string IWantTo { get; set; }
        public string SoThat { get; set; }
        public string Notes { get; set; }
        public int? Priority { get; set; }
        public string Estimate { get; set; }
        public decimal TimeEstimate { get; set; }
        public decimal? PercentageCompletion { get; set; }
        public int? Status { get; set; }
        public string CreatedBy { get; set; }
        public int? ProjectID { get; set; }
        public bool? Active { get; set; }
        public DateTime? LastEdited { get; set; }
        public string LastEditedBy { get; set; }
        public List<Task> Tasks { get; set; }

    }
}