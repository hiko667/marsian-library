using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class BookGenre
{
    [Required]
    [ForeignKey("BookId")]
    public int BookId { get; set; }
    public Book? Book { get; set; }

    [Required]
    [ForeignKey("GenreId")]
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}