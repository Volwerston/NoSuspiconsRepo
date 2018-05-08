using System;
using System.Collections.Generic;
using System.Linq;
using Film.Data;
using Film.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Film
{
    public static class DbInit
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                
            context.Database.EnsureCreated();

            if(context.Users.Any())
            {
                return;
            }

            var roles = new List<IdentityRole>{
                new IdentityRole{
                    Id = Guid.NewGuid().ToString(),
                    Name = "admin",
                    NormalizedName = "admin"
                },
                new IdentityRole{
                    Id = Guid.NewGuid().ToString(),
                    Name = "moderator",
                    NormalizedName = "moderator"
                },
                new IdentityRole{
                    Id = Guid.NewGuid().ToString(),
                    Name = "user",
                    NormalizedName = "user"
                }
            };

            context.Roles.AddRange(roles);

            var admin = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "admin",
                Age = 20,
                City = "Lviv"
            };

            context.Users.Add(admin);

            var userRoleAdmin = new IdentityUserRole<string>
            {
                UserId = admin.Id,
                RoleId = roles[0].Id
            };

            var userRoleModerator = new IdentityUserRole<string>
            {
                UserId = admin.Id,
                RoleId = roles[1].Id
            };

            context.UserRoles.AddRange(new List<IdentityUserRole<string>>{userRoleAdmin, userRoleModerator});

            
            var genres = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Комедія"
                },
                new Category
                {
                    Id = 2,
                    Name = "Триллер"
                },
                new Category
                {
                    Id = 3,
                    Name = "Містика"
                },
                new Category
                {
                    Id = 4,
                    Name = "Хоррор"
                },
                new Category
                {
                    Id = 5,
                    Name = "Документальний"
                }
            };

            context.Categories.AddRange(genres);

            var film = new Movie
            {
                Id = 1,
                Name = "Best Movie 4",
                Categories = new List<CategoryFilm>
                {
                    new CategoryFilm
                    {
                        FilmId = 1,
                        CategoryId = 4,
                    },
                    new CategoryFilm
                    {
                        FilmId = 1,
                        CategoryId = 1,
                    }
                }
            };

            context.Films.Add(film);

            context.SaveChanges();
            }
        }
    }
}