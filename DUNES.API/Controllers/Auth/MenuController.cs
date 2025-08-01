using Azure;
using DUNES.API.Services.Auth;
using DUNES.Shared.DTOs.Auth;
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

            //obtain user information 
            var user = await _userManager.GetUserAsync(User);

            //obtain roles by user
          
            var roles = await _authService.GetRolesFromClaims(User);

            if (roles == null)
           
                return Forbid("User has no roles assigned.");


            // Llamamos al service con la lista de roles que Identity nos da
            var menuItems = await _menuService.GetMenuHierarchyAsync(roles.Data);


            return Ok(menuItems);

           
        }

     
    }
}
