using System.ComponentModel.DataAnnotations;

namespace Application.Dto;

public record LoginRequestDto([Required] string Login, [Required] string Password);