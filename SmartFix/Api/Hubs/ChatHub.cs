using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SmartFix.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var role = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (role == "Manager" || role == "Admin")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Managers");
        }
        
        await base.OnConnectedAsync();
    }
}