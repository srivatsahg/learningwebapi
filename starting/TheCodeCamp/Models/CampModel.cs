using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCodeCamp.Models
{
    public class CampModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Moniker { get; set; } //Surrogate key 

        [Required]
        public DateTime EventDate { get; set; } = DateTime.MinValue;

        [Required]
        [Range(1,30)]
        public int Length { get; set; } = 1;

        public ICollection<TalkModel> Talks { get; set; }

        //Using the location 
        public string Venue { get; set; }

        //Automapper maps the location details automatically using the prefix Location
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
    }
}

