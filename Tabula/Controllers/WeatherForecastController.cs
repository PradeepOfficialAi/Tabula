using Microsoft.AspNetCore.Mvc;
using Tabula.Detectors;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace Tabula.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IReadOnlyList<IReadOnlyList<Cell>> Get()
        {
            using (PdfDocument document = PdfDocument.Open("Resources/SBI.pdf", new ParsingOptions() { ClipPaths = true }))
            {
                ObjectExtractor oe = new ObjectExtractor(document);
                PageArea page = oe.Extract(1);

                IExtractionAlgorithm ea = new BasicExtractionAlgorithm();
                List<Table> tables = ea.Extract(page);
                var table = tables[tables.Count - 1];
                var rows = table.Rows;
                return rows;
            }
        }
    }
}