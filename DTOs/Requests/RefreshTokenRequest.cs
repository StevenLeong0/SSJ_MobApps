using System.ComponentModel.DataAnnotations;

namespace SeniorLearnApi.DTOs.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
