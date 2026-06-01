using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(10)]
    [MaxLength(13)]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "ISBN może zawierać tylko cyfry")]
    public string Isbn { get; set; } = string.Empty;

    [Required]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString();


    [Required]
    [ForeignKey("PublisherId")]
    public int PublisherId { get; set; }
    public virtual Publisher? Publisher { get; set; }

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();

}