using System;

namespace Infrastructure.Identity.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public UserModel()
        {
                
        }
    }
}