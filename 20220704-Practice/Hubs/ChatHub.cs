using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _20220704_Practice.Models;
using _20220704_Practice.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace _20220704_Practice
{
    //Authorize 加驗證
    [Authorize]
    public class ChatHub : Hub
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        /*public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }*/
        public async Task SendMessage(string message)
        {
            //var user = _httpContextAccessor.HttpContext.User.Claims.ToList();//從驗證把使用者的資料全部存入
            //var userName = user.Where(a => a.Type == "Username").First().ToString();//撈username

            //在Controller只要 User.Identity.Name
            //在Hub要 Context.User.Identity.Name
            var userName = Context.User.Identity.Name;

            var content = $"{userName} 於{DateTime.Now.ToShortTimeString()}說：{message}";
            
            await Clients.All.SendAsync("ReceiveMessage", content); //等待工作完成後再傳訊息
        }
        

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ConnectMessage", Context.ConnectionId);
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("DisconnectMessage", Context.ConnectionId, exception?.Message);
        }
    }
}
