using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Response
{
    public class MRes_User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}
