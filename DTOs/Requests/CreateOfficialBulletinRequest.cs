using System.ComponentModel.DataAnnotations;

namespace SeniorLearnApi.DTOs.Requests;

public class CreateOfficialBulletinRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
}
