using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRDevTest.Web.Models
{
    public class DevTestModel
    {
        [Required]
        [Display(Name = "Campaign Name")]
        public string CampaignName { get; set; }
        
        [Display(Name = "Campaign Date")]
        public Nullable<DateTime> Date { get; set; }
        
        [Display(Name = "Campaign Clicks")]
        public Nullable<int> Clicks { get; set; }
      
        [Display(Name = "Campaign Conversion")]
        public Nullable<int> Conversions { get; set; }
      
        [Display(Name = "Campaign Impression")]
        public Nullable<int> Impressions { get; set; }
      
        [Display(Name = "Affilaite Name")]
        public string AffiliateName { get; set; }
    }
}