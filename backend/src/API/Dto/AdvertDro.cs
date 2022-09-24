using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dto
{
    public class AdvertDro
    {

        [MaxLength(200)]
        public string UserId { get; set; } = ""; //EF use this to create a relation between User and Advert

        [MaxLength(200)]
        public string Name { get; set; } = "";

        [Range(0, 120, ErrorMessage = "Thats not a valid age")]
        public int Age { get; set; } = 0;

        [MaxLength(200)]
        public string Race { get; set; } = "";

        [MaxLength(200)]
        public string Sex { get; set; } = "";

        [MaxLength(200)]
        public string Personallity { get; set; } = "";

        [Range(0, 120, ErrorMessage = "Thats not a valid age")]
        public int RentPeriod { get; set; } = 0;

        [Range(1, 5, ErrorMessage = "Thats not a valid age")]
        public int Grade { get; set; } = 1;

        [MaxLength(200)]
        public string Review { get; set; } = "";
        
        [MaxLength(200)]
        public string ImageUrls { get; set; } = "";


    }
}
