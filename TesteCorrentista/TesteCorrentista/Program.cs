using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeuProjeto.Data;
using SeuProjeto.Models;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configura��o do DbContext com uma string de conex�o do appsettings.json
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        // Adiciona os servi�os MVC
        services.AddControllersWithViews();

        // Adicione outras configura��es de servi�os aqui, se necess�rio
        // Exemplo: services.AddScoped<IAlgumaInterface, AlgumaClasse>();
    });

var app = builder.Build();

// Configura��o do pipeline de solicita��o HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
