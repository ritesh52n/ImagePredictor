using Microsoft.AspNetCore.Http;

namespace ImagePredictor.Models
{
    public class PredictionModel
    {
        public IFormFile File { get; set; }
        public string PredictedLabel { get; set; }
    }
}
