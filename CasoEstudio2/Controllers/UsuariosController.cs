using CasoEstudio2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var caso2DbContext = _context.Usuarios.Include(u => u.Rol);
        return View(await caso2DbContext.ToListAsync());
    }

    // GET: Usuarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
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
        ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
        return View();
    }

    // POST: Usuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NombreUsuario,NombreCompleto,Correo,Telefono,Contraseña,RolId")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            // Hash de la contraseña
            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
        return View(usuario);
    }

    // GET: Usuarios/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = _context.Usuarios.Find(id);
        if (usuario == null)
        {
            return NotFound();
        }

        // Cargar los roles para el campo select
        ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
        return View(usuario);
    }



    // POST: Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,NombreCompleto,Correo,Telefono,Contraseña,RolId")] Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Si la contraseña ha sido modificada, aplicamos el hash
                var usuarioExistente = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (usuarioExistente != null && usuarioExistente.Contraseña != usuario.Contraseña)
                {
                    usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
                }

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
        ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
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
            .Include(u => u.Rol)
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
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}
