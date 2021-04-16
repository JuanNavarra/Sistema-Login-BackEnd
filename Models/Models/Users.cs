namespace Models
{
    using System;
    using System.Collections.Generic;

    public partial class Users
    {
        public int Iduser { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TokenConfirmation { get; set; }
        public bool IsConfirmed { get; set; }
        public bool State { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string TokenChangePassword { get; set; }
        public bool IsConfirmedChange { get; set; }
        public string TokenLogin { get; set; }
    }
}
