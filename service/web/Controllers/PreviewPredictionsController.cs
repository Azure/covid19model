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
        public async Task<ActionResult<Stream>> GetModelAsync(string country, DateTime? modelDate)
        {
            return await GetModelResponseAsync(country, modelDate);
        }

        /// <summary>
        /// Gets the prediction model
        /// </summary>
        [HttpGet("plot")]
        public async Task<ActionResult<Stream>> GetModelAsync(DateTime? modelDate)
        {
            modelDate = modelDate ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetPredictionDataAsync(null, modelDate.Value);
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
        public async Task<ActionResult<Stream>> GetInterventionsAsync(string country, DateTime? modelDate)
        {
            return await GetInterventionsResponseAsync(country, modelDate.Value);
        }

        [HttpGet("interventions")]
        public async Task<ActionResult<Stream>> GetInterventionsAsync(DateTime? modelDate)
        {
            return await GetInterventionsResponseAsync(null, modelDate);
        }

        private async Task<ActionResult<Stream>> GetModelResponseAsync(string country, DateTime? modelDate)
        {
            modelDate = modelDate ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetPredictionDataAsync(country, modelDate.Value);
            if (fileStream == null)
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = $"base-plot-{country}.csv"
            };
        }

        private async Task<ActionResult<Stream>> GetInterventionsResponseAsync(string country, DateTime? modelDate)
        {
            modelDate = modelDate ?? DateTime.UtcNow;
            var fileStream = await _modelDataProvider.GetInterventionDataAsync(country, modelDate.Value);
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