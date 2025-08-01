using System.Text.Json.Serialization;

namespace DUNES.API.Models.Auth
{
    /// <summary>
    /// Represents the login credentials required to authenticate a user.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// The user's email address used for login.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
         
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
