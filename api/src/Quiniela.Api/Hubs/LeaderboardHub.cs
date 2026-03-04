using Microsoft.AspNetCore.SignalR;

namespace Quiniela.Api.Hubs;

public class LeaderboardHub : Hub
{
    public async Task JoinRound(string roundId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roundId);
    }

    public async Task LeaveRound(string roundId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roundId);
    }
}
