using AzureAppService_Blink.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Diagnostics;

namespace AzureAppService_Blink.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ExportToPDF()
        {
            MemoryStream stream = null;
            try
            {
                //Initialize HTML to PDF converter. 
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
                BlinkConverterSettings settings = new BlinkConverterSettings();
                //Set command line arguments to run without the sandbox.
                settings.CommandLineArguments.Add("--no-sandbox");
                settings.CommandLineArguments.Add("--disable-setuid-sandbox");
                //Set Blink viewport size.
                settings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);
                //Assign Blink settings to the HTML converter.
                htmlConverter.ConverterSettings = settings;
                //Convert URL to PDF document.
                PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
                //Create memory stream.
                stream = new MemoryStream();
                //Save the document to memory stream. 
                document.Save(stream);
            }
            catch (Exception ex)
            {
                //Create a new PDF document.
                PdfDocument document = new PdfDocument();
                //Add a page to the document.
                PdfPage page = document.Pages.Add();
                //Create PDF graphics for the page.
                PdfGraphics graphics = page.Graphics;

                //Set the standard font.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
                //Draw the text.
                graphics.DrawString(ex.Message.ToString(), font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

                //Creating the stream object.
                stream = new MemoryStream();
                //Save the document into memory stream.
                document.Save(stream);
                //Close the document.
                document.Close(true);
            }

            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
