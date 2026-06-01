using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marsian_library.Models;

public class Address
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Building { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? Apartment { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string ZipCode { get; set; } = string.Empty;

    public ICollection<Dept> Depts { get; set; } = new List<Dept>();
    public ICollection<Emp> Emps { get; set; } = new List<Emp>();
    public ICollection<Reader> Readers { get; set; } = new List<Reader>();

    [NotMapped]
    public string FullAddress => $"{Street} {Building}{(string.IsNullOrEmpty(Apartment) ? "" : $"/{Apartment}")}, {ZipCode} {City}";
}