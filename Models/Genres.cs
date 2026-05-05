using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}