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
}
