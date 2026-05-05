using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Borrow
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("BookCopyId")]
    public int BookCopyId { get; set; }
    public virtual BookCopy? BookCopy { get; set; }

    // [Required] - odkomentuj jeśli Reader istnieje
    // [ForeignKey("ReaderId")]
    // public int ReaderId { get; set; }
    // public virtual Reader? Reader { get; set; }

    [Required]
    public DateTime BorrowDate { get; set; }  

    [Required]
    public DateTime ExpectedReturnDate { get; set; }  

    public DateTime? ReturnDate { get; set; }  

    [Required]
    [Range(0, 3)]
    public int TimesExtended { get; set; }
}