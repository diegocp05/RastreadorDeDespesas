using Expanse_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Expanse_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            // Definindo o intervalo de datas para os últimos 7 dias.
            DateTime DataInicio = DateTime.Today.AddDays(-6);
            DateTime DataFim = DateTime.Today;

            // Obtendo transações para o intervalo de datas especificado, incluindo informações da categoria.
            List<Transacao> TransacoesSelecionadas = await _context.Transacoes
                .Include(x => x.Categoria)
                .Where(y => y.Data >= DataInicio && y.Data <= DataFim)
                .ToListAsync();

            // Calculando e configurando as variáveis ViewBag para TotalIncome, TotalExpense e Balanco.
            int TotalIncome = TransacoesSelecionadas
                .Where(i => i.Categoria.Tipo == "Income")
                .Sum(j => j.Quantidade);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            int TotalExpense = TransacoesSelecionadas
                .Where(i => i.Categoria.Tipo == "Expense")
                .Sum(j => j.Quantidade);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            int Balanco = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balanco = String.Format(culture, "{0:C0}", Balanco);

            // Configurando os dados para o gráfico Doughnut (Expense By Category).
            ViewBag.DoughnutChartData = TransacoesSelecionadas
                .Where(i => i.Categoria.Tipo == "Expense")
                .GroupBy(j => j.Categoria.CategoriaId)
                .Select(k => new
                {
                    categoriaTituloComIcone = k.First().Categoria.Icone + " " + k.First().Categoria.Titulo,
                    quantidade = k.Sum(j => j.Quantidade),
                    quantidadeFormatada = k.Sum(j => j.Quantidade).ToString("C0"),
                })
                .ToList();

            // Configurando os dados para o gráfico Spline (TotalIncome Vs Expense).
            // Income
            List<SplineChartData> IncomeSummary = TransacoesSelecionadas
                .Where(i => i.Categoria.Tipo == "Income")
                .GroupBy(j => j.Data)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Data.ToString("dd-MMM"),
                    income = k.Sum(l => l.Quantidade)
                })
                .ToList();

            // Expense
            List<SplineChartData> ExpenseSummary = TransacoesSelecionadas
                .Where(i => i.Categoria.Tipo == "Expense")
                .GroupBy(j => j.Data)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Data.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Quantidade)
                })
                .ToList();

            // Combinando dados de Income e Expense para os últimos 7 dias.
            string[] ultimos7Dias = Enumerable.Range(0, 7)
                .Select(i => DataInicio.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in ultimos7Dias
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };

            // Configurando as transações mais recentes para serem exibidas no dashboard.
            ViewBag.RecentTransactions = await _context.Transacoes
                .Include(i => i.Categoria)
                .OrderByDescending(j => j.Data)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }

    // Classe auxiliar para armazenar dados usados no gráfico Spline.
    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
