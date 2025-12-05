using Azure;
using DUNES.API.Services.Auth;

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace DUNES.API.Controllers.Auth
{
    /// <summary>
    /// Provides menu options for authenticated users based on their assigned roles.
    /// </summary>
    [Authorize] // Only logged-in users can access this controller
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : BaseController
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
        public async Task<IActionResult> GetMenuOptions(CancellationToken ct)
        {
            return await HandleApi(ct => _menuService.GetMenuHierarchyAsync(User, ct), ct);

            
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
        /// <param name="ct">If the request was canceled by the user.</param>
        /// <response code="200">Returns the list of Level 2 menu options.</response>
        /// <response code="400">If the level1 parameter is missing or invalid.</response>
        /// 
        [HttpGet("level2/{level1}")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLevel2MenuOptions(string level1, CancellationToken ct)
        {

            return await HandleApi(ct => _menuService.GetLevel2MenuOptions(level1,User, ct), ct);
                      
        }

        /// <summary>
        /// get all menu options for a level code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("api/menu/breadcrumb/{code}")]
        [ProducesResponseType(typeof(List<MenuItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBreadcrumb(string code, CancellationToken ct)
        {
            return await HandleApi(ct => _menuService.BuildBreadcrumbAsync(code, User, ct), ct);
          
        }

        /// <summary>
        /// get all menu information for a controller/action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
       
        [HttpGet("codeByControllerAction")]
       // [HttpGet("codeByControllerAction")]
        public async Task<IActionResult> GetCodeByControllerAction(string controller, string action, CancellationToken ct)
        {

            return await HandleApi(ct => _menuService.GetCodeByControllerAction(controller, action, ct), ct);

                       
        }
        /// <summary>
        /// Get all active options menu
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _menuService.GetAllMenusAsync(ct), ct);
        }
    }
}
