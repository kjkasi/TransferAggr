﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Pages.Register;

public class InputModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    public string ReturnUrl { get; set; }

    public string? Button { get; set; }
}