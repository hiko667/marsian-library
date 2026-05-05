using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

// public class Books
// {
//     public int Id { get; set; }

//     [Required]
//     [Column(TypeName = "varchar(50)")]
//     [MaxLength(100)]
//     public string Name { get; set; }

//     [MaxLength(256)]
//     public string Bio { get; set; }

//     [MaxLength(50)]
//     public string FirstName { get; set; }

//     [MaxLength(50)]
//     public string LastName { get; set; }

//     // Klucz obcy - nullable
//     public int? TeamId { get; set; }

//     // WAŻNE: dodaj ? aby wskazać, że to opcjonalna relacja
//     [ForeignKey("TeamId")]
//     public virtual Team? Team { get; set; }  // Dodaj ? tutaj!

//     public ICollection<Movie> Movies { get; set; } = new List<Movie>();
//     public ICollection<Comic> Comics { get; set; } = new List<Comic>();
// }