using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Controllers
{
    /// <summary>
    /// Provides API to return prediction data
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly IModelDataProvider _modelDataProvider;

        public PredictionsController(IModelDataProvider modelDataProvider)
        {
            _modelDataProvider = modelDataProvider;
        }

        /// <summary>
        /// Gets the prediction model
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Stream>> GetModelAsync()
        {
            var fileStream =  await _modelDataProvider.GetModelDataAsync();
            return new FileStreamResult(fileStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "test.jpg"
            };
        }
    }
}