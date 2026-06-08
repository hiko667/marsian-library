using System.ComponentModel.DataAnnotations;       
using System.ComponentModel.DataAnnotations.Schema; 

namespace marsian_library.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    
    public ICollection<Book> Books { get; set; } = new List<Book>();
}