namespace Dtos
{
using System;
using System.Collections.Generic;
using System.Text;

    public class SendEMailDto
    {
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string PasswordFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
