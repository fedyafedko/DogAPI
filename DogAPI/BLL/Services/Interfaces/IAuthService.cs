﻿using Common.DTO.AuthDTO;

namespace BLL.Services.Interfaces;

public interface IAuthService
{
    Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user);
    Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user);
}