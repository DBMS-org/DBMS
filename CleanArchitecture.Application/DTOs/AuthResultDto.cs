using System;
using CleanArchitecture.Application.DTOs;
namespace CleanArchitecture.Application.AuthResultDTOs
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; }
        public string RefreshToken { get; set; }
    }
}