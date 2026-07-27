using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.ViewModels
{
    public class ProductoCreateViewModel
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 9999.99, ErrorMessage = "El precio debe ser un valor positivo (mínimo 0.01).")]
        public decimal Precio { get; set; }
    }
}