﻿using System.ComponentModel.DataAnnotations;

namespace JwtAspNetCoreWebApiSample.Models.Dto;

public class UserDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
}