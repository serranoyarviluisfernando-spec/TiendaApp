using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TallerMecanico.Data;
using TallerMecanico.Interfaces;
using TallerMecanico.Models;
using TallerMecanico.Services;
using TallerMecanico.ViewModels;

namespace TallerMecanico.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesApiController : ControllerBase
    {
        private readonly IRepository<Producto> _repository;
        private readonly ReportService _reportService;

        public ReportesApiController(IRepository<Producto> repository, ReportService reportService)
        {
            _repository = repository;
            _reportService = reportService;
            QuestPDF.Settings.License = LicenseType.Community; // Licencia gratuita
        }

        // 1. Descargar Excel
        [HttpGet("descargar-excel")]
        public async Task<IActionResult> DescargarExcel()
        {
            var productos = await _repository.GetAllAsync();
            var dtos = productos.Select(p => new ProductoDto(p.Id, p.Nombre, p.Precio, 10)).ToList();

            var archivoExcel = _reportService.GenerarExcelProductos(dtos);
            return File(archivoExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Inventario.xlsx");
        }

        // 2. Descargar PDF
        [HttpGet("descargar-pdf")]
        public async Task<IActionResult> DescargarPdf()
        {
            var productos = await _repository.GetAllAsync();

            var pdfData = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Reporte de Inventario - Taller Mecánico")
                        .FontSize(18).SemiBold().FontColor("#2572A9");

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().BorderBottom(1).Padding(5).Text("Producto").Bold();
                            header.Cell().BorderBottom(1).Padding(5).Text("Precio").Bold();
                        });

                        foreach (var item in productos)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor("#DDD").Padding(5).Text(item.Nombre);
                            table.Cell().BorderBottom(0.5f).BorderColor("#DDD").Padding(5).Text($"${item.Precio:F2}");
                        }
                    });
                });
            }).GeneratePdf();

            return File(pdfData, "application/pdf", "Reporte_Inventario.pdf");
        }

        // 3. Endpoint de datos para Chart.js
        [HttpGet("datos-grafico")]
        public async Task<IActionResult> GetDatosGrafico()
        {
            var productos = await _repository.GetAllAsync();

            // Mapeo simple de productos y precios para la gráfica
            var datos = productos.Select(p => new
            {
                Etiqueta = p.Nombre,
                Valor = p.Precio
            });

            return Ok(datos);
        }
    }
}