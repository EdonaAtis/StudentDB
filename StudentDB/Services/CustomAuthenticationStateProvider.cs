using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server;

namespace StudentDB.Services
{
    public class CustomAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomAuthenticationStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
            : base(loggerFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var user = authenticationState.User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            var userId = user.FindFirst(c => c.Type == "sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var userInDb = await userManager.FindByIdAsync(userId);

                if (userInDb == null)
                {
                    return false;
                }

                // Check if user is still in the required roles
                var isInRole = await userManager.IsInRoleAsync(userInDb, "CourseManager");
                if (!isInRole)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
