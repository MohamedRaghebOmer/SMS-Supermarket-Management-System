using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMS.Contracts.Requests.Auth
{
    public class LogoutRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }

        [Required]
        public required string Username { get; set; }
    }
}
