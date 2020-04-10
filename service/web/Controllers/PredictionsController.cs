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
        [HttpGet("plot/{country}")]
        public async Task<ActionResult<Stream>> GetModelAsync(string country)
        {
            var fileStream = await _modelDataProvider.GetPredictionDataAsync(country);
            if (fileStream == null)
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = $"base-plot-{country}.csv"
            };
        }

        /// <summary>
        /// Gets the prediction model
        /// </summary>
        [HttpGet("plot")]
        public async Task<ActionResult<Stream>> GetModelAsync()
        {
            var fileStream = await _modelDataProvider.GetPredictionDataAsync(null);
            if (fileStream == null)
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = $"base-plot.csv"
            };
        }

        [HttpGet("interventions/{country}")]
        public async Task<ActionResult<Stream>> GetInterventionsAsync(string country)
        {
            var fileStream = await _modelDataProvider.GetInterventionDataAsync(country);
            if (fileStream == null)
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = $"base-intervention-{country}.csv"
            };
        }

        [HttpGet("interventions")]
        public async Task<ActionResult<Stream>> GetInterventionsAsync()
        {
            var fileStream = await _modelDataProvider.GetInterventionDataAsync(null);
            if (fileStream == null)
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = $"base-intervention.csv"
            };
        }
    }
}