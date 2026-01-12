using DUNES.Shared.Models;
using DUNES.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Helpers
{
    public static class MessageHelper
    {
        /// <summary>
        /// Writes UI or Toast messages.
        /// Ignores canceled requests (StatusCode = 499).
        /// </summary>
        public static void SetMessage(
            Controller controller,
            ApiResponse<object> response,
            MessageDisplay display)
        {
            // 🔕 Ignore canceled requests
            if (response?.StatusCode == 499)
                return;

            if (display == MessageDisplay.Toast)
            {
                controller.TempData["ApiMessage"] = response.Message;
                controller.TempData["ApiType"] =
                    response.Success ? "success" : "error";
            }
            else
            {
                controller.TempData["UIMsg"] =
                    System.Text.Json.JsonSerializer.Serialize(
                        new UIMsg
                        {
                            Type = response.Success ? "success" : "error",
                            Message = response.Message
                        });
            }
        }

        // Overload legacy (no se toca)
        public static void SetMessage(
            Controller controller,
            string type,
            string message,
            MessageDisplay display)
        {
            if (display == MessageDisplay.Toast)
            {
                controller.TempData["ApiMessage"] = message;
                controller.TempData["ApiType"] = type.ToLower();
            }
            else
            {
                controller.TempData["UIMsg"] =
                    System.Text.Json.JsonSerializer.Serialize(
                        new UIMsg { Type = type, Message = message });
            }
        }
    }
}
