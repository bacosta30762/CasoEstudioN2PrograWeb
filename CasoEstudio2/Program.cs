using CasoEstudio2.Models;
using CasoEstudio2.Seed;
using CasoEstudio2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<Caso2DbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddSession();
builder.Services.AddScoped<IDashboardService, DashboardService>();


var app = builder.Build();

// Endpoint: GET /api/events (Lista de eventos)
app.MapGet("/api/events", async (IEventoService eventoService) =>
{
    var eventos = await eventoService.ObtenerEventosAsync();
    return Results.Ok(eventos.Select(e => new
    {
        e.Id,
        e.Titulo,
        e.Descripcion,
        e.Fecha,
        e.Hora,
        e.Duracion,
        e.Ubicacion,
        e.CupoMaximo,
        e.CategoriaId
    }));
});

// Endpoint: GET /api/events/{id} (Detalles de un evento específico)
app.MapGet("/api/events/{id:int}", async (int id, IEventoService eventoService) =>
{
    var evento = await eventoService.ObtenerEventoPorIdAsync(id);
    if (evento == null)
    {
        return Results.NotFound(new { message = "Evento no encontrado" });
    }
    return Results.Ok(evento);
});

using (var serviceScope = app.Services.CreateScope())
{
    var seeder = serviceScope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
