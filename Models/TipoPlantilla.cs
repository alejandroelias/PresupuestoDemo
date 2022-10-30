using PresupuestoDemo.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PresupuestoDemo.Models
{
    public class TipoPlantilla : IValidatableObject
    {
        public int Id_TipoPlantilla { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Plantilla es requerido")]
        [Display(Name = "Tipo de plantilla")]
        [PrimeraLetraMayuscula]
        public string TipoDePlantilla { get; set; }

        [Required(ErrorMessage = "El campo Activa es requerido")]
        [Display(Name = "Activa")]
        public bool EsActiva { get; set; }

        public string UsuarioLog { get; set; }

        internal DateTime FechaModificacionLog { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Codigo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Codigo != null && Codigo.Length > 0)
            {
                var campoTamano3Letras = Codigo.ToString();

                if (campoTamano3Letras.Length > 3)
                {
                    yield return new ValidationResult("El codigo deber solo 3 letras", new[] { nameof(Codigo) });
                }

            }
        }
    }
}