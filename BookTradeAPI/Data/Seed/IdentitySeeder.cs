using BookTradeAPI.Models.Entities;
using BookTradeAPI.Utilities.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookTradeAPI.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roleNames = { "Admin", "Seller", "Member" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            string address = "VN";
            string phoneNumber = "0123456789";

            // Seed admin account
            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new User
                {
                    FullName = "Admin",
                    Address = address,
                    Email = adminEmail,
                    UserName = adminEmail,
                    PhoneNumber = phoneNumber,
                    AvatarUrl = "images/avatar.png"
                };
                string password = "Admin123@";
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, UserRoleConstant.ADMIN);
                }
            }

            // Seed seller account
            var sellerEmail = "seller@gmail.com";
            var sellerUser = await userManager.FindByEmailAsync(sellerEmail);
            if (sellerUser == null)
            {
                var seller = new User
                {
                    FullName = "Seller",
                    Address = address,
                    Email = sellerEmail,
                    UserName = sellerEmail,
                    PhoneNumber = phoneNumber,
                    AvatarUrl = "images/avatar.png"
                };
                string password = "Seller123@";
                var result = await userManager.CreateAsync(seller, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, UserRoleConstant.SELLER);
                }
            }

            // Seed member account
            var memberEmail = "member@gmail.com";
            var memberUser = await userManager.FindByEmailAsync(memberEmail);
            if (memberUser == null)
            {
                var member = new User
                {
                    FullName = "Member",
                    Address = address,
                    Email = memberEmail,
                    UserName = memberEmail,
                    PhoneNumber = phoneNumber,
                    AvatarUrl = "images/avatar.png"
                };
                string password = "Member123@";
                var result = await userManager.CreateAsync(member, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(member, UserRoleConstant.MEMBER);
                }
            }
        }
    }
}
