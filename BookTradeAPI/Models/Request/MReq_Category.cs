using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(50, ErrorMessage = "Tên danh mục có độ dài tối đa 50 ký tự")]
        public string Name { get; set; } = null!;
    }
}
