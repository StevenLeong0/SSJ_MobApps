using System.ComponentModel.DataAnnotations;

namespace SeniorLearnApi.DTOs.Requests;

public class UpdateSettingsRequest
{
    [Required]
    public int FontSize { get; set; }
    public bool DarkMode { get; set; }
    public bool EnableNotifications { get; set; }
}
