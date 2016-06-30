using System.ComponentModel.DataAnnotations;
    
namespace KeesTalksTech.Utilities.Web.Mvc.Logon
{
    /// <summary>
    /// Simple model for a logon.
    /// </summary>
    public class LogonModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        public string Password { get; set; }
    }
}