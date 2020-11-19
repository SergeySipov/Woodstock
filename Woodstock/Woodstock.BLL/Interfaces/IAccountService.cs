﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Woodstock.BLL.DTOs;

namespace Woodstock.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(UserDTO userDTO, bool isPersistent, bool lockoutOnFailure);
        Task<IdentityResult> RegisterAsync(UserDTO userDTO, string claimRole);
        Task CompleteEmailConfirmationAsync(string email, string confirmToken);
        Task LogoutAsync();
        AuthenticationProperties ConfigureExternalAuthentication(string provider, string redirectUrl);
        Task<SignInResult> ExternalLoginAsync();
        Task<IdentityResult> ExternalRegisterAsync(UserDTO userDTO);
        Task<string> GenerateEmailConfirmationAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
    }
}
