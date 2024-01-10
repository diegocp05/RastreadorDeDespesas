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
    public class TransacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Construtor que recebe uma instância do contexto do banco de dados.
        public TransacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ação para listar todas as transações.
        public async Task<IActionResult> Index()
        {
            // Recupera todas as transações, incluindo informações sobre suas categorias.
            var applicationDbContext = _context.Transacoes.Include(t => t.Categoria);
            return View(await applicationDbContext.ToListAsync());
        }

        // Ação para adicionar ou editar uma transação.
        public IActionResult AddOuEditar(int id = 0)
        {
            // Povoar a lista de categorias antes de exibir a view.
            PovoarCategorias();

            if (id == 0)
                return View(new Transacao());
            else
                return View(_context.Transacoes.Find(id));
        }

        // Ação para lidar com o envio do formulário de adição ou edição de uma transação.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOuEditar([Bind("TransacaoId,CategoriaId,Quantidade,Nota,Data")] Transacao transacao)
        {
            if (ModelState.IsValid)
            {
                // Verifica se a transação já existe no banco de dados.
                if (transacao.TransacaoId == 0)
                    _context.Add(transacao);
                else
                    _context.Update(transacao);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se o modelo não for válido, povoar a lista de categorias novamente e retornar à view.
            PovoarCategorias();
            return View(transacao);
        }

        // Ação para lidar com a exclusão de uma transação.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Verifica se o conjunto de entidades 'ApplicationDbContext.Transacoes' é nulo.
            if (_context.Transacoes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transacoes' is null.");
            }

            // Busca a transação pelo ID e a remove se existir.
            var transacao = await _context.Transacoes.FindAsync(id);
            if (transacao != null)
            {
                _context.Transacoes.Remove(transacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método não relacionado à ação para povoar a lista de categorias antes de exibir a view.
        [NonAction]
        public void PovoarCategorias()
        {
            // Recupera todas as categorias do banco de dados.
            var CategoriaCollection = _context.Categorias.ToList();

            // Adiciona uma categoria padrão para escolha ao início da lista.
            Categoria DefaultCategoria = new Categoria() { CategoriaId = 0, Titulo = "Escolha a Categoria" };
            CategoriaCollection.Insert(0, DefaultCategoria);

            // Define a lista de categorias disponíveis na ViewBag.
            ViewBag.Categorias = CategoriaCollection;
        }
    }
}
