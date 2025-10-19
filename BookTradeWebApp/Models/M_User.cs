using BookTradeAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookTradeWebApp.Models
{
    public class M_User_Login
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [StringLength(255, ErrorMessage = "Email có độ dài tối đa 255 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
    public class M_Seller_Register
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên cửa hàng")]
        [StringLength(100, ErrorMessage = "Tên cửa hàng có độ dài tối đa 100 ký tự")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile? File { get; set; }

        public string? BannerUrl { get; set; }

        public byte Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(50, ErrorMessage = "Họ tên có độ dài tối đa 50 ký tự")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ có độ dài tối đa 255 ký tự")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [StringLength(255, ErrorMessage = "Email có độ dài tối đa 255 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(20, ErrorMessage = "Số điện thoại có độ dài tối đa 20 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = null!;
    }
}
