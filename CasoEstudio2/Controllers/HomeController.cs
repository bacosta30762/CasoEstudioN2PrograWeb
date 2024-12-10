using CasoEstudio2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly Caso2DbContext _context;

    public HomeController(Caso2DbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string nombreUsuario, string contraseña)
    {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        if (usuario != null && BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña))
        {
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol.Nombre);
            HttpContext.Session.SetString("UsuarioNombre", usuario.NombreUsuario);

            return RedirectToAction("Index");
        }

        ViewBag.Error = "Nombre de usuario o contraseña incorrectos.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult Register()
    {
        // Cargar los roles al mostrar el formulario de registro
        ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([Bind("NombreUsuario,NombreCompleto,Correo,Telefono,Contraseña,RolId")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            // Verificar que el nombre de usuario no esté repetido
            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario))
            {
                ModelState.AddModelError(string.Empty, "El nombre de usuario ya está en uso.");
                ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
                return View(usuario);
            }

            // Hash de la contraseña
            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registro exitoso. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
        return View(usuario);
    }
}
