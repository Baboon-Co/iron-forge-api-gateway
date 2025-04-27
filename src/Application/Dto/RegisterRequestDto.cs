using System.ComponentModel.DataAnnotations;

namespace Application.Dto;

public record RegisterRequestDto([Required] string Username, [Required] string Login, [Required] string Password);