using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Copy
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("BookId")]
    public int BookId { get; set; }
    public virtual Book? Book { get; set; }

    [Required]
    [ForeignKey("DeptId")]
    public int DeptId { get; set; }
    public virtual Dept? Dept { get; set; }


    [Required]
    [ForeignKey("StateId")]
    public int StateId { get; set; }
    public virtual State? State { get; set; }
}