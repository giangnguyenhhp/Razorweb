using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP12_RazorPage_EntityFramework.Models;
// [Table("Post")]
public class Article
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(255)]
    [Required]
    [Column]
    public string Title { get; set; }
    
    [DataType(DataType.Date)]
    [Required]
    public DateTimeOffset Created { get; set; }
    
    
    public string Content { get; set; }
}