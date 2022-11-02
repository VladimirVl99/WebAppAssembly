using WebAppAssembly.Shared.Entities.IikoCloudApi;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class Checkin
    {
        public IEnumerable<LoyaltyProgramResult>? LoyaltyProgramResults { get; set; }
        public string? WarningMessage { get; set; }
    }
}
