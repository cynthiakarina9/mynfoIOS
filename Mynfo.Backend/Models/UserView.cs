namespace Mynfo.Backend.Models
{
    using Domain;

    public class UserView : User
    {
        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}