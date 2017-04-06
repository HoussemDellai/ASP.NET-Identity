namespace XamarinApp.Models
{
    public class AuthResponse
    {
        /// <summary>   
        /// Tells if the API call was processed successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Result received from the API.
        /// Could be JSON or String.
        /// </summary>
        public string Result { get; set; }      
    }
}
