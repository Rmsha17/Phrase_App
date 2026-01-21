// Plan (pseudocode):
// 1. Provide extension methods to read a user's GUID identifier from a JWT/ClaimsPrincipal.
// 2. Support common claim keys: ClaimTypes.NameIdentifier, "sub", and "id" (string).
// 3. Implement a safe nullable accessor: GetUserId(this ClaimsPrincipal) -> Guid?
//    - If ClaimsPrincipal is null, return null.
//    - Check claims in order; if a claim exists, attempt Guid.TryParse.
//    - If parse succeeds, return Guid value; otherwise continue to next claim.
//    - If no claim or no valid GUID found, return null.
// 4. Implement a throwing accessor: GetRequiredUserId(this ClaimsPrincipal) -> Guid
//    - Call the nullable accessor; if null, throw InvalidOperationException with a clear message.
// 5. Add HttpContext overloads delegating to the ClaimsPrincipal methods for convenience.
// 6. Keep APIs minimal, thread-safe, and null-safe; avoid dependencies beyond System and ASP.NET types.

// Implementation:
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Phrase_App.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Attempts to retrieve the user's GUID identifier from available claims.
        /// Supports ClaimTypes.NameIdentifier, "sub", and "id".
        /// Returns null if no valid GUID claim is found.
        /// </summary>
        public static Guid? GetUserId(this ClaimsPrincipal? user)
        {
            if (user is null)
            {
                return null;
            }

            // Common claim types that may contain the user id
            var claimTypes = new[] { ClaimTypes.NameIdentifier, "sub", "id" };

            foreach (var type in claimTypes)
            {
                var claim = user.FindFirst(type);
                if (claim is null)
                {
                    continue;
                }

                if (Guid.TryParse(claim.Value, out var guid))
                {
                    return guid;
                }
            }

            // If no GUID was parsed, return null
            return null;
        }

        /// <summary>
        /// Retrieves the user's GUID identifier from claims or throws an InvalidOperationException if not found/invalid.
        /// </summary>
        public static Guid GetRequiredUserId(this ClaimsPrincipal? user)
        {
            var id = user.GetUserId();
            if (id.HasValue)
            {
                return id.Value;
            }

            throw new InvalidOperationException("User identifier claim not found or is not a valid GUID.");
        }

        /// <summary>
        /// Convenience overload for HttpContext that delegates to ClaimsPrincipal extensions.
        /// Returns null if context or user is null or no valid GUID claim is found.
        /// </summary>
        public static Guid? GetUserId(this HttpContext? context)
        {
            return context?.User.GetUserId();
        }

        /// <summary>
        /// Convenience overload for HttpContext that delegates to ClaimsPrincipal extensions and throws if not present.
        /// </summary>
        public static Guid GetRequiredUserId(this HttpContext? context)
        {
            return context?.User.GetRequiredUserId() ?? throw new InvalidOperationException("HttpContext or User is null when attempting to get required user id.");
        }
    }
}