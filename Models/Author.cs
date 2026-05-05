using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [Column(TypeName = "varchar(50)")]
    [MaxLength(100)]
    public string Lastname { get; set; }

}