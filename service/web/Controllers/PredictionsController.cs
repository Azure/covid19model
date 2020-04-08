using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<object>> GetModelAsync()
        {
            return await _modelDataProvider.GetModelDataAsync();
        }
    }
}