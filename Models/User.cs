using System;
using System.Collections.Generic;

#nullable disable

namespace DTDM.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Passwd { get; set; }
        public int? Role { get; set; }
    }
}
