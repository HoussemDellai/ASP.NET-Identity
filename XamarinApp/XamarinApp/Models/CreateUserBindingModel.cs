//using System.ComponentModel.DataAnnotations;

namespace XamarinApp.Models
{
    /// <summary>
    /// The class is very simple, it contains properties for the fields we want to send 
    /// from the client to our API with some data annotation attributes 
    /// which help us to validate the model before submitting it to the database, 
    /// notice how we added property named “RoleName”.
    /// </summary>
    public class CreateUserBindingModel
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RoleName { get; set; }
        
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
    }
}