using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class BookCopy
{
    public int Id { get; set; }


    [Required]
    [ForeignKey("BookId")]
    public int BookId { get; set; }
    public virtual Book Book { get; set; }

    // [Required]
    // [ForeignKey("DeptId")]
    // public int DeptId { get; set; }
    // public virtual Dept Dept { get; set; }


    [Required]
    [ForeignKey("BookStateId")]
    public int BookStateId { get; set; }
    public virtual BookState BookState { get; set; }
}