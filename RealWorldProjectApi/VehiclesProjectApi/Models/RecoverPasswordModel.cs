namespace VehiclesProjectApi.Models
{
    /// <summary>
    /// Email to send password recovery info to.
    /// </summary>
    public class RecoverPasswordModel
    {
        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }
    }
}