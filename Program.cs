using BancaApi.Data;
using BancaApi.Domain.Interface;
using BancaApi.Services;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddDbContext<BancaDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IClienteServices, ClienteService>();
        builder.Services.AddScoped<IContadorCuentas, ContadorCuentaService>();
        builder.Services.AddScoped<ICuentaServices, CuentaService>();
        builder.Services.AddScoped<ITransaccionService, TransaccionService>();

        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();

    }
}
