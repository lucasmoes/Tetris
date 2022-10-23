using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TetrisServer.Hubs
{
    public class TetrisHub : Hub
    {
        
        public async Task ReadyUp(int seed)
        {
            await Clients.Others.SendAsync("ReadyUp", seed);
        }
        
        public async Task StartGame(int seed)
        {
            await Clients.Others.SendAsync("StartGame", seed);
        }
        
        
        public async Task DropShape(bool state)
        {
            await Clients.Others.SendAsync("DropShape", state);
        }
        
        public async Task HardDrop(bool state)
        {
            await Clients.Others.SendAsync("HardDrop", state);
        }

        public async Task RotateShape(string direction)
        {
            await Clients.Others.SendAsync("RotateShape", direction);
        }

        public async Task MoveShape(string moveDirection)
        {
            await Clients.Others.SendAsync("MoveShape", moveDirection);
        }
        

    }
}
