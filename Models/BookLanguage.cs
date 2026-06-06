using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class BookLanguage
{
    [Required]
    [ForeignKey("BookId")]
    public int BookId { get; set; }
    public Book? Book { get; set; }

    [Required]
    [ForeignKey("LanguageId")]
    public int LanguageId { get; set; }
    public Language? Language { get; set; }
}