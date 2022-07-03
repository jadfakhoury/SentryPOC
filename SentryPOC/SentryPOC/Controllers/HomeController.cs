using Microsoft.AspNetCore.Mvc;
using Sentry;
using SentryPOC.Models;
using System.Diagnostics;

namespace SentryPOC.Controllers
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
            SentrySdk.CaptureMessage("Hello Sentry");
            return View();
        }

        public IActionResult CatchedError()
        {
            try
            {
                SentrySdk.AddBreadcrumb("This is inside the CatchedError method;");
                throw new Exception("This is catched error exception.");
            }
            catch (Exception e)
            {
                var ioe = new InvalidOperationException("Bad POST! See Inner exception for details.", e);
                ioe.Data.Add("inventory",
                    new
                    {
                        SmallPotion = 3,
                        BigPotion = 0,
                        CheeseWheels = 512
                    });

                throw ioe;
            }
        }

        public IActionResult UncatchedTest()
        {
            SentrySdk.AddBreadcrumb("This is inside the UncatchedTest method;");
            throw new Exception("Test exception thrown in controller!");
        }

        public IActionResult ExplicitTest()
        {
            try
            {
                SentrySdk.AddBreadcrumb("This is inside the ExplicitTest method;");
                throw new Exception("Test exception thrown in controller!");
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                throw;
            }
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