using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.DTOs
{
    public class CreateToDoItemDto
    {
        [Required(ErrorMessage = "Se requiere un valor para el campo Nombre")]
        [StringLength(10,ErrorMessage="El campo {0} debe tener {1} o menos caracteres")]
        public string Name {  get; set; }=string.Empty;

        [Range(18,65 ,ErrorMessage= "El campo {0} debe estar entre {1} y {2} años")]
        public int Edad { get; set; }
    }
}
