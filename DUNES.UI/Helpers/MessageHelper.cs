using DUNES.UI.Models;

using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Helpers
{
    public static class MessageHelper
    {

        /// <summary>
        /// write error message in a TempData . The only way to do this is by writing
        /// to the controller, so you must pass the controller name
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void SetMessage(Controller controller, string type, string message)
        {
            controller.TempData["UIMsg"] = System.Text.Json.JsonSerializer.Serialize(
                new UIMsg { Type = type, Message = message });
        }

    }
}
