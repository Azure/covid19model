using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Controllers
{
    /// <summary>
    /// Provides API to return preview prediction data
    /// </summary>
    [Route("preview/predictions")]
    [ApiController]
    [Authorize]
    public class PreviewPredictionsController : ControllerBase
    {
        private readonly IModelDataProvider _modelDataProvider;

        public PreviewPredictionsController(IModelDataProvider modelDataProvider)
        {
            _modelDataProvider = modelDataProvider;
        }

        /// <summary>
        /// Gets the prediction model
        /// </summary>
        [HttpGet("plot/{country}")]
        public async Task<ActionResult<Stream>> GetModelAsync(string country, DateTime? date)
        {
            date = date ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetPredictionDataAsync(country, date.Value);
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
        public async Task<ActionResult<Stream>> GetModelAsync(DateTime? date)
        {
            date = date ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetPredictionDataAsync(null, date.Value);
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
        public async Task<ActionResult<Stream>> GetInterventionsAsync(string country, DateTime? date)
        {
            date = date ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetInterventionDataAsync(country, date.Value);
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
        public async Task<ActionResult<Stream>> GetInterventionsAsync(DateTime? date)
        {
            date = date ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetInterventionDataAsync(null, date.Value);
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