using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_User
    {
        public class MReq_User_Login
        {
            [Required(ErrorMessage = "Vui lòng nhập email")]
            [StringLength(255, ErrorMessage = "Email có độ dài tối đa 255 ký tự")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; } = null!;

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
            public string Password { get; set; } = null!;
        }
        public class MReq_User_Register
        {
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
            public string Password { get; set; } = null!;

            [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
            [StringLength(20, ErrorMessage = "Số điện thoại có độ dài tối đa 20 ký tự")]
            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            public string PhoneNumber { get; set; } = null!;
        }
    }
}
