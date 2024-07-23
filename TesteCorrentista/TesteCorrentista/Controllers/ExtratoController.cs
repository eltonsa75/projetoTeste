using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using TesteCorrentista.Models;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SeuProjeto.Data;

namespace TesteCorrentista.Controllers
{
    public class ExtratoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _pdfConverter;

        public ExtratoController(ApplicationDbContext context, IConverter pdfConverter)
        {
            _context = context;
            _pdfConverter = pdfConverter;
        }

        public async Task<IActionResult> Index(int dias = 5)
        {
            var dataLimite = DateTime.Now.AddDays(-dias);
            var transacoes = await _context.Transacoes
                .Where(t => t.Data >= dataLimite)
                .ToListAsync();

            return View(transacoes);
        }

        public async Task<IActionResult> ExportarPdf(int dias = 5)
        {
            var dataLimite = DateTime.Now.AddDays(-dias);
            var transacoes = await _context.Transacoes
                .Where(t => t.Data >= dataLimite)
                .ToListAsync();

            var htmlContent = await RenderViewToStringAsync("Index", transacoes);

            var pdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = {
                    DocumentTitle = "Extrato",
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            var pdfBytes = _pdfConverter.Convert(pdfDocument);

            return File(pdfBytes, "application/pdf", "extrato.pdf");
        }

        private async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.GetView("", viewName, false);

                if (viewResult.Success == false)
                {
                    throw new ArgumentNullException("A view with the name " + viewName + " could not be found");
                }

                var viewContext = new ViewContext
                {
                    ViewData = ViewData,
                    Writer = writer,
                    HttpContext = HttpContext,
                    RouteData = RouteData,
                    TempData = TempData,
                    View = viewResult.View
                };

                await viewResult.View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
