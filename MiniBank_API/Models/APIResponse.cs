using System.Net;

namespace MiniBank_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public string ErrorMessage { get; set; }
        public List<string> ExceptionMessages { get; set; }
        public Object Resultado { get; set; }


        public Object Editado() => Resultado = new { message = "Editado" };
        public Object Eliminado() => Resultado = new { message = "Eliminado" };

        public string ErrorNotFound(int id) => this.ErrorMessage = $"El Id={id} no existe en la base de datos";
        public string ErrorIdCero() => this.ErrorMessage = $"El Id debe ser mayor que cero (0)";
        public string ErrorDifId(int id, Object Id) => this.ErrorMessage = $"El Id={id} de la URL no coincide con el id={Id} del cuerpo de la solicitud";
        public string ErrorJson() => this.ErrorMessage = $"Json nulo o inválido";
    }
}