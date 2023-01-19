﻿namespace WebApiAutores.Filtros
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo 1.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            //throw new NotImplementedException();
            Escribir("Proceso iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            timer.Dispose();
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void Escribir(string mensaje)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
