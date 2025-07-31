using DUNES.Shared.DTOs.Auth;
using DUNES.API.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DUNES.API.Controllers.Auth
{
    /// <summary>
    /// Provides menu options for authenticated users based on their assigned roles.
    /// </summary>
    [Authorize] // Only logged-in users can access this controller
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMenuService _menuService;
        private readonly UserManager<IdentityUser> _userManager;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="userManager"></param>
        /// <param name="authService"></param>
        public MenuController(IMenuService menuService, UserManager<IdentityUser> userManager, IAuthService authService )
        {
            _menuService = menuService;
            _userManager = userManager;
            _authService = authService;
        }

        /// <summary>
        /// Returns the hierarchical menu for the authenticated user if they have assigned menus.
        /// </summary>
        [HttpGet("options")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MenuItemDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMenuOptions()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return Unauthorized();

            //Aquí pedimos el usuario actual a Identity
            var user = await _userManager.GetUserAsync(User);

            //Y ahora sacamos todos los roles del usuario con Identity
            // var roles = await _userManager.GetRolesAsync(user);

            var roles = await _authService.GetRolesFromClaims(User);

           // if (roles == null || !roles.Any())
                if (roles != null)
                return Forbid("User has no roles assigned.");


            // Llamamos al service con la lista de roles que Identity nos da
            var menuItems = await _menuService.GetMenuHierarchyAsync(roles);

            return Ok(menuItems);
        }

        //[HttpGet("debug-claims")]
        //public IActionResult GetClaimsDebug()
        //{
        //    var claimsList = User.Claims
        //        .Select(c => new
        //        {
        //            Type = c.Type,
        //            Value = c.Value
        //        })
        //        .ToList();

        //    return Ok(new
        //    {
        //        IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
        //        AuthenticationType = User.Identity?.AuthenticationType,
        //        Claims = claimsList
        //    });
        //}
    }
}
