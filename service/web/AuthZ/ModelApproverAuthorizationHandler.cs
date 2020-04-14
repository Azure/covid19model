using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AuthZ.Requirements;
using Web.Data;

namespace Web.AuthZ
{
    public class ModelApproverAuthorizationHandler : AuthorizationHandler<ModelApproverRequirement>
    {
        private readonly IApprovalDataProvider _approvalDataProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ModelApproverAuthorizationHandler(IApprovalDataProvider approvalDataProvider, IHttpContextAccessor httpContextAccessor)
        {
            _approvalDataProvider = approvalDataProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ModelApproverRequirement requirement)
        {
            var routeDate = _httpContextAccessor.HttpContext.GetRouteData();

            DateTime dateRequested;

            if (routeDate.Values.TryGetValue("modelDate", out var modelDateRequested))
            {
                if (!DateTime.TryParse(modelDateRequested.ToString(), out dateRequested))
                {
                    // bad data, don't authorize request
                    context.Fail();
                    return;
                }
            }
            else
            {
                dateRequested = DateTime.UtcNow;
            }

            // allow anonymous access for approved data
            if (await _approvalDataProvider.IsDataApproved(dateRequested))
            {
                context.Succeed(requirement);
            }
            // Require authenticated users for preview data
            // TODO: enforce ic.ac.uk
            else if (context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }

            // otherwise challenge
        }
    }
}
