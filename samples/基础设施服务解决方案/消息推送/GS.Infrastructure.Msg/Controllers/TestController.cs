using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sikiro.Tookits.Extension;

namespace Sikiro.Infrastructure.Msg.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        public string Index()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            string localIp = NetworkInterface.GetAllNetworkInterfaces()
                .Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .FirstOrDefault(p => p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))?.Address.ToString();

            var result = new List<string>();

            var b = NetworkInterface.GetAllNetworkInterfaces()
                .Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .Where(p => p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))
                .Select(a =>
                {
                    return new
                    {
                        Address = a.Address.ToStr()
                    };
                }).ToList();


            return b.ToJson() + "------" + localIp;
        }
    }
}
