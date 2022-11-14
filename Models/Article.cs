using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP12_RazorPage_EntityFramework.Models;

// [Table("Post")]
public class Article
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(255,MinimumLength = 5,ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
    [Required(ErrorMessage = "{0} phải nhập")]
    [Column]
    [DisplayName("Tiêu đề Blog")]
    public string Title { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "{0} phải nhập")]
    [DisplayName("Ngày tạo")]
    public DateTimeOffset Created { get; set; }

    [DisplayName("Nội dung")]
    public string? Content { get; set; }
    
}