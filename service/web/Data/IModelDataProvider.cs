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
        /// <returns></returns>
        Task<Stream> GetModelDataAsync();
    }
}
