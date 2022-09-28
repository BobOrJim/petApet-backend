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
                    Alias = "janbannanAlias",
                    PhoneNr = "12345678",
                    IsLoggedIn = false,
                    ProfilePictureUrl = "",
                    AuthId = Guid.Parse("322a8943-370e-4190-ac9f-4deccc700cb6"),
                    Adverts = new List<Advert>
                    {
                        new Advert
                        {
                            Name = "Boppe",
                            Age = 1,
                            Race = "Orc",
                            Sex = "Male",
                            Personallity = "Like to cuddle",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://media.istockphoto.com/photos/yorkshire-ter…n-white-background-picture-id1318666271?s=612x612"
                        },
                        new Advert
                        {
                            Name = "Blåöga",
                            Age = 2,
                            Race = "Goblin",
                            Sex = "Male",
                            Personallity = "Sleepy",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://media.istockphoto.com/photos/tricolor-bern…camera-and-panting-picture-id1346381655?s=612x612"
                        },
                        new Advert
                        {
                            Name = "Sture",
                            Age = 2,
                            Race = "Barn cat",
                            Sex = "Male",
                            Personallity = "Likes to watch tv",
                            RentPeriod = 5,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://www.felinemedicalclinic.com/wp-content/uploads/2020/11/feline-01.jpeg"
                        },
                        new Advert
                        {
                            Name = "Snuttis",
                            Age = 2,
                            Race = "Greyhound",
                            Sex = "Male",
                            Personallity = "Likes to watch tv",
                            RentPeriod = 5,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://i.etsystatic.com/7670702/r/il/58a8d5/1414317722/il_794xN.1414317722_771f.jpg"
                        },
                                                new Advert
                        {
                            Name = "Sköldis",
                            Age = 2,
                            Race = "Tortoise",
                            Sex = "Male",
                            Personallity = "Likes to watch tv",
                            RentPeriod = 5,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://i.imgur.com/hbRdlCz.jpg"
                        },
                    }
                },
                new User()
                {
                    Alias = "MrWhite",
                    PhoneNr = "88112233",
                    IsLoggedIn = false,
                    ProfilePictureUrl = "",
                    AuthId = Guid.Parse("7cdb1ac1-7c99-4ef3-a274-027a81b85efe"),
                    Adverts = new List<Advert>
                    {
                        new Advert
                        {
                            Name = "Silversjärna",
                            Age = 1,
                            Race = "Okänt",
                            Sex = "Male",
                            Personallity = "Like to eat grass",
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
                        },
                        new Advert
                        {
                            Name = "Kari",
                            Age = 1,
                            Race = "Okänt",
                            Sex = "Male",
                            Personallity = "Krispig",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://i.imgur.com/p37VN4W.jpeg"
                        },
                                                new Advert
                        {
                            Name = "Urkel",
                            Age = 1,
                            Race = "Terrier",
                            Sex = "Male",
                            Personallity = "Very smart",
                            RentPeriod = 2,
                            Grade = 1,
                            Review = "",
                            ImageUrls = @"https://media.istockphoto.com/photos/dog-with-pencil-at-the-office-picture-id667786852?k=20&m=667786852&s=612x612&w=0&h=WuNB1lGE3kq0ZtbJgetKGc5ytxoGY0Hn4CPreT_QEgM="
                        }
                    }
                },
            };
        }
    }
}



