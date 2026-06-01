using System.ComponentModel.DataAnnotations;        // Dla [Required] i [MaxLength]
using System.ComponentModel.DataAnnotations.Schema; // Dla [Column]

namespace marsian_library.Models;

public class Emp
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("AddressId")]
    public int AddressId { get; set; }
    public virtual Address? Address { get; set; }

    [ForeignKey("DeptId")]
    public int? DeptId { get; set; }
    public virtual Dept? Dept { get; set; }

    [Required]
    [ForeignKey("JobId")]
    public int JobId { get; set; }
    public virtual Job? Job { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
}