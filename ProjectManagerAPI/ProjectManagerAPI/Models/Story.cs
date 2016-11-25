using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class Story
    {
        public int ID { get; set; }
        public string StoryName { get; set; }
        public int Theme { get; set; }
        public int Actor { get; set; }
        public string IWantTo { get; set; }
        public string SoThat { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }
        public string Estimate { get; set; }
        public decimal TimeEstimate { get; set; }
        public decimal PercentageCompletion { get; set; }
        public int Status { get; set; }
        public int ProjectID { get; set; }
        public bool Active { get; set; }

    }
}