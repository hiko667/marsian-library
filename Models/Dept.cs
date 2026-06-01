using System.ComponentModel.DataAnnotations;       
using System.ComponentModel.DataAnnotations.Schema; 

namespace marsian_library.Models;

public class Dept
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("AddressId")]
    public int AddressId { get; set; }
    public virtual Address? Address { get; set; }

    [ForeignKey("DirectorId")]
    public int? DirectorId { get; set; }
    public virtual Emp? Director { get; set; }

    public ICollection<Emp> Emps { get; set; } = new List<Emp>();
}