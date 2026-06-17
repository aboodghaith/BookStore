using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class SeedService
    {
        private readonly UserManager<User> _user;
        private readonly RoleManager<IdentityRole> _role;
        private readonly IConfiguration _config;

        public SeedService(UserManager<User> user, RoleManager<IdentityRole> role, IConfiguration config)
        {
            this._user = user;
            this._role = role;
            this._config = config;
        }

        public async Task SeedAdmin()
        {
            string roleName = "Admin";
            await SeedRole(roleName);

            string AdminEmail = _config["SeedAdmin:Email"]!;
            string AdminPassword = _config["SeedAdmin:Password"]!;

            var existingAdmin = await _user.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == AdminEmail);
                                                    
        
            if (existingAdmin == null)
            {
                var NewAdmin = new User { UserName = AdminEmail, Email = AdminEmail , FirstName = "Abdelrahman" 
                    , LastName = "Hassan" , Address= "N" , PhoneNumber = "01098803496"};
                var result = await _user.CreateAsync(NewAdmin, AdminPassword);
                if (result.Succeeded)
                {
                    await _user.AddToRoleAsync(NewAdmin, roleName);
                }
                else
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }


            }
        }

        public async Task SeedRoles()
        {
            await SeedRole("User");
        }
        public async Task SeedRole(string roleName)
        {
            if (!await _role.RoleExistsAsync(roleName))
            {
                var result = await _role.CreateAsync(new IdentityRole { Name = roleName });

            }
        }
    }
}
