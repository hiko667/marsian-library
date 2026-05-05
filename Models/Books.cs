using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    [MaxLength(256)]
    public string Title { get; set; }

    [Required]
    [MaxLength(13)]
    [MinLength(10)]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "ISBN może zawierać tylko cyfry")]
    public string Isbn { get; set; }

    public int PublisherId { get; set; }

    // WAŻNE: dodaj ? aby wskazać, że to opcjonalna relacja
    [ForeignKey("PublisherId")]
    public virtual Publisher Publisher { get; set; }  // Dodaj ? tutaj!

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Author> Authors { get; set; } = new List<Author>();
}