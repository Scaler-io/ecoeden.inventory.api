﻿using Ecoeden.Inventory.Domain.Models.Dtos;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecoeden.Inventory.Api.Services;

public class IdentityService(IHttpContextAccessor contextAccessor) : IIdentityService
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public const string IdClaim = ClaimTypes.NameIdentifier;
    public const string RoleClaim = ClaimTypes.Role;
    public const string FirstNameClaim = ClaimTypes.GivenName;
    public const string LastNameClaim = ClaimTypes.Surname;
    public const string UsernameClaim = JwtRegisteredClaimNames.Name;
    public const string EmailClaim = ClaimTypes.Email;
    public const string PermissionClaim = "permissions";

    public UserDto PrepareUser()
    {
        var token = _contextAccessor.HttpContext.Request.Headers.Authorization;
        var claims = _contextAccessor.HttpContext.User.Claims;

        if(!claims.Any())
        {
            return null;
        }

        var roleString = claims.Where(c => c.Type == RoleClaim).FirstOrDefault().Value;
        var permissionString = claims.Where(c => c.Type == PermissionClaim).FirstOrDefault().Value;

        return new()
        {
            Id = claims.Where(c => c.Type == IdClaim).FirstOrDefault().Value,
            FirstName = claims.Where(c => c.Type == FirstNameClaim).FirstOrDefault().Value,
            LastName = claims.Where(c => c.Type == LastNameClaim).FirstOrDefault().Value,
            UserName = claims.Where(c => c.Type == UsernameClaim).FirstOrDefault().Value,
            AuthorizationDto = new AuthorizationDto
            {
                Roles = JsonConvert.DeserializeObject<List<string>>(roleString),
                Permissions = JsonConvert.DeserializeObject<List<string>>(permissionString),
                Token = token
            }
        };
    }
}
