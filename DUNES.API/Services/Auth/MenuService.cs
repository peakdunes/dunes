using DUNES.API.DTOs.B2B;
using DUNES.API.RepositoriesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;


namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// Handles the business logic for menu retrieval and hierarchy building.
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;
        private readonly IAuthService _authService;
       // private readonly IMenuService _menuService;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public MenuService(IMenuRepository repository, IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
           
        }
        /// <summary>
        /// Gets the level 2 menus associated with level1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<MenuItemDto>>> GetLevel2MenuOptions(string level1, ClaimsPrincipal User, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(level1))
                return ApiResponseFactory.BadRequest<List<MenuItemDto>>("Parameter 'level1' is required.");

            var roles = await _authService.GetRolesFromClaims(User, ct);
            if (!roles.Success || roles.Data == null || !roles.Data.Any())
                return ApiResponseFactory.Forbidden<List<MenuItemDto>>("User has no roles assigned.");

            var flatMenu = await _repository.GetLevel2MenuOptions(level1, roles.Data, ct);

            if (flatMenu == null || !flatMenu.Any())
                return ApiResponseFactory.BadRequest<List<MenuItemDto>>("There is not menu  options available for this user.");

            return ApiResponseFactory.Ok(flatMenu, "Menu options level2 availables for this user");


        }



        /// <summary>
        /// get menu options nivel 1 for role list
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
          public async Task<ApiResponse<List<MenuItemDto>>> GetMenuHierarchyAsync(ClaimsPrincipal user, CancellationToken ct)

        {

            // 1. Validar autenticación
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
                return ApiResponseFactory.Unauthorized<List<MenuItemDto>>("User not authenticated");

            // 2. Obtener roles
            var roles = await _authService.GetRolesFromClaims(user, ct);
            if (!roles.Success || roles.Data == null || !roles.Data.Any())
                return ApiResponseFactory.Forbidden<List<MenuItemDto>>("User has no roles assigned.");

            // 3. Buscar menús
            var menuItems = await _repository.GetAllActiveMenusAsync(roles.Data, ct);
            if (menuItems == null || !menuItems.Any())
                return ApiResponseFactory.Ok(new List<MenuItemDto>(), "No menus available for this user");

            // 4. Retornar éxito
            return ApiResponseFactory.Ok(menuItems);

        }

        /// <summary>
        /// get all menu option for a specific code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// 


        public async Task<ApiResponse<List<MenuItemDto>>> BuildBreadcrumbAsync(string code, ClaimsPrincipal user, CancellationToken ct)
        {

            // 1. Code validation

            if (string.IsNullOrEmpty(code))
                return ApiResponseFactory.Error<List<MenuItemDto>>("Menu code is required");

            // 2. Validar autenticación
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
                return ApiResponseFactory.Unauthorized<List<MenuItemDto>>("User not authenticated");

            // 3. Obtener roles
            var roles = await _authService.GetRolesFromClaims(user, ct);
            if (!roles.Success || roles.Data == null || !roles.Data.Any())
                return ApiResponseFactory.Forbidden<List<MenuItemDto>>("User has no roles assigned.");

            // 4. Generar el breadcrumb usando los roles
           
            var allMenus = await _repository.GetAllMenusByRolesAsync(roles.Data, ct);


            if (allMenus == null || !allMenus.Any())
                return ApiResponseFactory.NotFound<List<MenuItemDto>>($"No breadcrumb found for menu code '{code}'");


            var breadcrumb = new List<MenuItemDto>();

            for (int i = 2; i <= code.Length; i += 2)
            {
                var partialCode = code.Substring(0, i);
                var match = allMenus.FirstOrDefault(m => m.Code == partialCode);
                if (match != null)
                {
                    breadcrumb.Add(match);
                }
            }


            // 5. Devolver la respuesta envuelta como ApiResponse
            return ApiResponseFactory.Ok(breadcrumb);





            //if (string.IsNullOrWhiteSpace(code))
            //{
            //    return new List<MenuItemDto>
            //        {
            //            new MenuItemDto
            //            {
            //                Code = "root",
            //                Title = "Menú General"
            //            }
            //        };
            //}

            //var allMenus = await _repository.GetAllMenusByRolesAsync(roles, ct);

            //var breadcrumb = new List<MenuItemDto>();

            //for (int i = 2; i <= code.Length; i += 2)
            //{
            //    var partialCode = code.Substring(0, i);
            //    var match = allMenus.FirstOrDefault(m => m.Code == partialCode);
            //    if (match != null)
            //    {
            //        breadcrumb.Add(match);
            //    }
            //}

            //return breadcrumb;
        }
        /// <summary>
        /// Get all information about a menu option
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<MenuItemDto>> GetCodeByControllerAction(string controller, string action, CancellationToken ct)
        {

         
            var MenuInfo = await _repository.GetCodeByControllerAction(controller, action, ct);


            if (MenuInfo == null)
            {

                return ApiResponseFactory.NotFound<MenuItemDto>("Information Not found");
               

            }
            else
            {

                return ApiResponseFactory.Ok(MenuInfo, "OK");
            }
        
        }
        /// <summary>
        /// get all active options menu
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<MenuItemDto>>> GetAllMenusAsync(CancellationToken ct)
        {
            var MenuInfo = await _repository.GetAllMenusAsync(ct);


            if (MenuInfo == null)
            {

                return ApiResponseFactory.NotFound<List<MenuItemDto>>("Information Not found");


            }
            else
            {

                return ApiResponseFactory.Ok(MenuInfo, "OK");
            }
        }
    }
}
