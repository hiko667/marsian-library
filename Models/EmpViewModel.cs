using System.ComponentModel.DataAnnotations;

namespace marsian_library.Models
{
    public class EmpViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public int AddressId { get; set; }

        [Display(Name = "Dział")]
        public int? DeptId { get; set; }

        [Required]
        [Display(Name = "Stanowisko")]
        public int JobId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła muszą być identyczne.")]
        [Display(Name = "Potwierdź hasło")]
        public string? ConfirmPassword { get; set; }

        public string? CurrentEmail { get; set; }
    }
}
