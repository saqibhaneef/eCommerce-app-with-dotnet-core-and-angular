﻿using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Bob",
                    Email = "bob@mailinator.com",
                    UserName = "bob@mailinator.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bob",
                        Street = "10 Street",
                        City = "New town",
                        State = "NY",
                        ZipCode = "786",
                    }

                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}
