using System;
using System.IO;
using ImagePredictor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ImagePredictorTrainingModel;



namespace ImagePredictor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PredictImage(IFormFile UploadFIle)
        {
           
                if (UploadFIle != null && UploadFIle.Length > 0)
                {
                    // Construct the file path using the hosting environment
                    var uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");
                    var filePath = Path.Combine(uploadsFolder, UploadFIle.FileName);

                    // Save the uploaded file
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    UploadFIle.CopyTo(fileStream);
                    //}
                    using (var memoryStream = new MemoryStream())
                    {

                        UploadFIle.CopyTo(memoryStream);


                        var imageBytes = memoryStream.ToArray();

                        //var imageBytes = File.ReadAllBytes(@"C:\Users\Ritesh\Downloads\archive\train\Bear\0155b267ade95d1e.jpg");
                        MLModel.ModelInput sampleData = new MLModel.ModelInput()
                        {
                            ImageSource = imageBytes,
                        };

                        // Load model and predict output
                        var result = MLModel.Predict(sampleData);

                        // Create a view model and set its properties
                        var modelData = new PredictionModel();

                        modelData.File = UploadFIle;
                        modelData.PredictedLabel = result.PredictedLabel;


                        // Return the view with the model data
                        return View(modelData);
                    }
                    // Load the uploaded image for predictions
                    //var imageBytes = File.ReadAllBytes(filePath);

                   
                }
                else
                {
                    ModelState.AddModelError("File", "Please select a file.");
                    return View();
                }
            
           
        }

    }
}
