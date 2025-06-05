using System.ComponentModel.DataAnnotations;

namespace SeniorLearnApi.DTOs.Requests;

public class SignOutRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
