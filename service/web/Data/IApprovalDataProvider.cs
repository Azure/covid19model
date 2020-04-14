using System;
using System.Threading.Tasks;

namespace Web.Data
{
    public interface IApprovalDataProvider
    {
        Task<bool> IsDataApproved(DateTime dateRequested);
    }
}