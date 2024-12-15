using CasoEstudio2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CasoEstudio2.Services;

public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;
    private readonly Caso2DbContext _context;

    public UsuarioController(IUsuarioService usuarioService, Caso2DbContext context)
    {
        _usuarioService = usuarioService;
        _context = context;
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            usuario.RolId = 3;
            await _usuarioService.CrearUsuario(usuario);
            return RedirectToAction("Index", "Home");
        }
        return View(usuario);
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string correo, string password)
    {
        var usuario = await _usuarioService.AutenticarUsuario(correo, password);
        if (usuario != null)
        {
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("RolUsuario", usuario.Rol.Nombre);
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Error = "Correo o contraseña incorrectos.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> GestionarRoles()
    {
        var usuarios = await _usuarioService.ObtenerTodosUsuarios();
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> AsignarRol(int usuarioId, int rolId)
    {
        await _usuarioService.AsignarRol(usuarioId, rolId);
        return RedirectToAction("GestionarRoles");
    }
}

