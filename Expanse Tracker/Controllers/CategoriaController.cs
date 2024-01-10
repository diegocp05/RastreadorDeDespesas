using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expanse_Tracker.Models;

namespace Expanse_Tracker.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            // Retorna a lista de categorias se não for nula, senão, retorna um problema indicando que o conjunto de entidades está nulo.
            return _context.Categorias != null ?
                   View(await _context.Categorias.ToListAsync()) :
                   Problem("Entity set 'ApplicationDbContext.Categorias' is null.");
        }

        // GET: Categoria/AddOrEdit
        public IActionResult AddOuEditar(int id = 0)
        {
            // Retorna uma visão para adicionar ou editar uma categoria com base no ID.
            if (id == 0)
                return View(new Categoria());
            else
                return View(_context.Categorias.Find(id));
        }

        // POST: Categoria/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOuEditar([Bind("CategoriaId,Titulo,Icone,Tipo")] Categoria categoria)
        {
            // Adiciona ou atualiza a categoria no banco de dados e redireciona para a página de índice.
            if (ModelState.IsValid)
            {
                if (categoria.CategoriaId == 0)
                    _context.Add(categoria);
                else
                    _context.Update(categoria);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Remove a categoria do banco de dados e redireciona para a página de índice.
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categorias' is null.");
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
