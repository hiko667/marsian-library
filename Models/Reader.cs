using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Reader
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("AddressId")]
    public int AddressId { get; set; }
    public virtual Address? Address { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}