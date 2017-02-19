using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Authorization.Policies
{
    public static class SalesOperations
    {
        private const string _view = "View";

        public static readonly OperationAuthorizationRequirement View =
            new OperationAuthorizationRequirement { Name = _view };
    }
}
