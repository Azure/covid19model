using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Data
{
    /// <summary>
    /// Provides model inferred data
    /// </summary>
    public interface IModelDataProvider
    {
        /// <summary>
        /// Gets the inference data from the model
        /// </summary>
        /// <param name="country">The country to get predictive data for</param>
        /// <returns></returns>
        Task<Stream> GetPredictionDataAsync(string country);

        /// <summary>
        /// Gets the intervention inference data from the model
        /// </summary>
        /// <param name="country">The country to get predictive data for</param>
        /// <returns></returns>
        Task<Stream> GetInterventionDataAsync(string country);
    }
}
