using ContactDatabase.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAndModel
{
    public class Seeder
    {
        static string baseDir = Directory.GetCurrentDirectory();
        public async static Task Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ContactDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
            if (!context.Users.Any())
            {
                List<string> roles = new List<string>() { "Admin", "Regular" };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var path = File.ReadAllText(FilePath(baseDir, "JsonFile/Users.json"));

                var users = JsonConvert.DeserializeObject<List<User>>(path);

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Fatai@01");
                    if (user == users[0])
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "Regular");
                    }
                }
                await context.Users.AddRangeAsync(users);
            }
            

        }

        static string FilePath(string folderName, string fileName)
        {
            return Path.Combine(folderName, fileName);
        }
    }
}
