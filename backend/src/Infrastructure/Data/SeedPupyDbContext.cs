using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Data
{
    public class SeedPupyDbContext
    {

        public static async Task SeedAsync(PupyDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.User.AnyAsync())
            {
                await context.User.AddRangeAsync(GetPreconfiguredUsers());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Alias = "MrGreen",
                    Email = "jon@gmail.com",
                    PhoneNr = "12345678",
                    IsLoggedIn = false,
                    ProfilePictureUrl = "",
                    Adverts = new List<Advert>
                    {
                        new Advert
                        {
                            Name = "Gurgulath Dwarfkilla",
                            Age = 1,
                            Race = "Orc",
                            Sex = "Male",
                            Personallity = "Very agressive. Dislike short people",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://media.istockphoto.com/photos/yorkshire-ter…n-white-background-picture-id1318666271?s=612x612"
                        },
                        new Advert
                        {
                            Name = "Snitch Gutstabah",
                            Age = 2,
                            Race = "Goblin",
                            Sex = "Male",
                            Personallity = "Will stab you in the back",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://media.istockphoto.com/photos/tricolor-bern…camera-and-panting-picture-id1346381655?s=612x612"
                        }
                    }
                },
                new User()
                {
                    Alias = "MrWhite",
                    Email = "White@gmail.com",
                    PhoneNr = "88112233",
                    IsLoggedIn = false,
                    ProfilePictureUrl = "",
                    Adverts = new List<Advert>
                    {
                        new Advert
                        {
                            Name = "Morgul eyepoker",
                            Age = 1,
                            Race = "Snotling",
                            Sex = "Male",
                            Personallity = "Likes to poke people in they eye",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://cdn.pixabay.com/photo/2017/09/25/13/12/cocker-spaniel-2785074_960_720.jpg"
                        },
                        new Advert
                        {
                            Name = "Throgg",
                            Age = 2,
                            Race = "River Troll",
                            Sex = "Male",
                            Personallity = "Will squeeze and suck you dry, but not in a good way",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://cdn.pixabay.com/photo/2016/12/13/05/15/puppy-1903313_640.jpg"
                        }
                    }
                },
                new User()
                {
                    Alias = "ReadyOnlyDude",
                    Email = "dude@gmail.com",
                    PhoneNr = "88112233",
                    IsLoggedIn = false,
                    ProfilePictureUrl = "",
                },
                new User()
                {
                    Alias = "LikeToRentSomething",
                    Email = "dude@gmail.com",
                    PhoneNr = "88112233",
                    IsLoggedIn = false,
                    ProfilePictureUrl = "",
                }
            };
        }
    }
}



