using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeuProjeto.Data;
using SeuProjeto.Models;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configuração do DbContext com uma string de conexão do appsettings.json
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        // Adiciona os serviços MVC
        services.AddControllersWithViews();

        // Adicione outras configurações de serviços aqui, se necessário
        // Exemplo: services.AddScoped<IAlgumaInterface, AlgumaClasse>();
    });

var app = builder.Build();

// Configuração do pipeline de solicitação HTTP
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
