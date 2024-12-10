using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasoEstudio2.Models;

namespace CasoEstudio2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Caso2DbContext _context;

        public UsuariosController(Caso2DbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            // Ocultar contraseñas: no incluirlas en la consulta
            var usuarios = await _context.Usuarios
                .Select(u => new
                {
                    u.Id,
                    u.NombreUsuario,
                    u.NombreCompleto,
                    u.Correo,
                    u.Telefono,
                    u.Rol
                }).ToListAsync();

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Select(u => new
                {
                    u.Id,
                    u.NombreUsuario,
                    u.NombreCompleto,
                    u.Correo,
                    u.Telefono,
                    u.Rol
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(new[] { "Administrador", "Organizador", "Usuario" });
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreUsuario,NombreCompleto,Correo,Telefono,Contraseña,Rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Validar rol ingresado
                if (!new[] { "Administrador", "Organizador", "Usuario" }.Contains(usuario.Rol))
                {
                    ModelState.AddModelError("Rol", "El rol especificado no es válido.");
                    ViewData["Roles"] = new SelectList(new[] { "Administrador", "Organizador", "Usuario" });
                    return View(usuario);
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roles"] = new SelectList(new[] { "Administrador", "Organizador", "Usuario" });
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = new SelectList(new[] { "Administrador", "Organizador", "Usuario" }, usuario.Rol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,NombreCompleto,Correo,Telefono,Rol")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roles"] = new SelectList(new[] { "Administrador", "Organizador", "Usuario" }, usuario.Rol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
