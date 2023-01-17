namespace WebApiAutores.MiddleWares
{
    public class LoguearRespuestaHTTPMiddleware
    {
        public readonly RequestDelegate siguiente;
        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente)
        {
            this.siguiente = siguiente;
        }

        // Invoke o InvokeAsync
        public async Task InvokeAsync(HttpContext contexto)
        {

        }
    }
}
