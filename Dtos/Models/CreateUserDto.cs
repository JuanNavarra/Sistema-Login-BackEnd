namespace Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
