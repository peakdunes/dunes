using Azure;
using DUNES.API.Services.Auth;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //var user = await _userManager.GetUserAsync(User);

            //obtain roles by user
          
            var roles = await _authService.GetRolesFromClaims(User);

            if (roles == null)
           
                return Forbid("User has no roles assigned.");


            // Llamamos al service con la lista de roles que Identity nos da
            var menuItems = await _menuService.GetMenuHierarchyAsync(roles.Data);

            if (menuItems.Data == null || !menuItems.Data.Any())
                return Ok(ApiResponseFactory.Ok(menuItems.Data, "No menus available for this user"));

            return Ok(menuItems);

           
        }
        /// <summary>
        /// Gets Level 2 menu options based on a Level 1 code.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrieve the Level 2 menu items that belong to a given Level 1 menu code.
        ///
        /// **Example request:**
        /// GET /api/menu/level2?level1=01
        ///
        /// **Example response:**
        /// ```json
        /// [
        ///   { "code": "0101", "title": "Reports", "controller": "Reports", "action": "Index" },
        ///   { "code": "0102", "title": "Settings", "controller": "Settings", "action": "Index" }
        /// ]
        /// ```
        /// </remarks>
        /// <param name="level1">The Level 1 menu code (e.g. "01").</param>
        /// <response code="200">Returns the list of Level 2 menu options.</response>
        /// <response code="400">If the level1 parameter is missing or invalid.</response>
        /// 
        [HttpGet("level2/{level1}")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLevel2MenuOptions(string level1)
        {
            if (string.IsNullOrEmpty(level1))
                return BadRequest("Parameter 'level1' is required.");

            // ✅ Ya no pedimos user porque no se usa
            var roles = await _authService.GetRolesFromClaims(User);

            var menulevel2 = await _menuService.GetLevel2MenuOptions(level1, roles.Data);

            if (menulevel2.Data == null || !menulevel2.Data.Any())
                return Ok(ApiResponseFactory.Ok(menulevel2.Data, "No menus level 2 available for this user"));

            return Ok(menulevel2);
        }

        /// <summary>
        /// get all menu options for a level code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("api/menu/breadcrumb/{code}")]
        [ProducesResponseType(typeof(List<MenuItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBreadcrumb(string code)
        {
            // 1. Obtener los roles desde los claims
            var roleResponse = await _authService.GetRolesFromClaims(User);

            if (roleResponse == null || !roleResponse.Success || roleResponse.Data == null || !roleResponse.Data.Any())
                return Unauthorized(ApiResponseFactory.NotFound<List<MenuItemDto>>("User does not have any valid roles."));

            // 2. Generar el breadcrumb usando los roles
            var breadcrumb = await _menuService.BuildBreadcrumbAsync(code, roleResponse.Data);

            if (breadcrumb == null || !breadcrumb.Any())
                return NotFound(ApiResponseFactory.NotFound<List<MenuItemDto>>($"No breadcrumb found for menu code '{code}'"));

            // 3. Devolver la respuesta envuelta como ApiResponse
            return Ok(ApiResponseFactory.Ok(breadcrumb));
        }

        /// <summary>
        /// get all menu information for a controller/action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
       
        [HttpGet("codeByControllerAction")]
       // [HttpGet("codeByControllerAction")]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> GetCodeByControllerAction(string controller, string action)
        {
            var menu = await _menuService.GetCodeByControllerAction(controller, action);

            if (menu == null)

                return NotFound(ApiResponseFactory.NotFound<List<MenuItemDto>>($"No breadcrumb found for this controller/action"));


            return Ok(ApiResponseFactory.Ok(menu));
        }
    }
}
