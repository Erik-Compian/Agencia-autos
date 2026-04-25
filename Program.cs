using Microsoft.EntityFrameworkCore;
// 1. Agregamos la librería de Autenticación
using Microsoft.AspNetCore.Authentication.Cookies;
using AgenciaAutosMVC.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AgenciaAutosContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// --- INICIO: CONFIGURACIÓN DE LOGIN Y SESIÓN ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Si alguien intenta entrar a una página prohibida, lo mandamos aquí:
        options.LoginPath = "/Acceso/Index";
        // Tiempo de vida de la sesión (ej. 30 minutos de inactividad)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });
// --- FIN DE LA CONFIGURACIÓN ---

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 2. IMPORTANTE: Activar la Autenticación ANTES de la Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();