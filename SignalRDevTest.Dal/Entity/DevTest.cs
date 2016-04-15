using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Entity
{
    [Serializable]
    [DataContract(IsReference = true)]
    [Table("DevTest")]
   public class DevTest : EntityBase
    {
        [Key]
        public int ID { get; set; }
        public string CampaignName { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public Nullable<int> Clicks { get; set; }
        public Nullable<int> Conversions { get; set; }
        public Nullable<int> Impressions { get; set; }
        public string AffiliateName { get; set; }


    }
}
