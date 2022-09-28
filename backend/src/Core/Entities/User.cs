using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(200)]
        public string Alias { get; set; } = "";
        
        [MaxLength(20)]
        public string PhoneNr { get; set; } = "";
        
        public bool IsLoggedIn { get; set; } = false;

        [MaxLength(200)]
        public string ProfilePictureUrl { get; set; } = "";
        

        public Guid AuthId { get; set; } //FK till AuthEntity, ligger dock utanför EF


        public List<Advert>? Adverts { get; set; } = new(); //EF use this to create a relation between User and Advert
    }
}

/*
  
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = "";
 **/