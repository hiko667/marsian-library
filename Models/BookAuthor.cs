using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class BookAuthor
{
    [Required]
    [ForeignKey("BookId")]
    public int BookId { get; set; }
    public Book? Book { get; set; }

    [Required]
    [ForeignKey("AuthorId")]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}