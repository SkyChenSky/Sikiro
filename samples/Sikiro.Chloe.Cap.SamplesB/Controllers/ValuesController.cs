using System;
using System.Diagnostics;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Chloe.Cap.SamplesB.Db;

namespace Sikiro.Chloe.Cap.SamplesB.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ICapPublisher _capBus;
        private readonly BusinessPlatformContext _businessPlatformContext;


        public ValuesController(ICapPublisher capPublisher, BusinessPlatformContext businessPlatformContext)
        {
            _capBus = capPublisher;
            _businessPlatformContext = businessPlatformContext;
        }

        [NonAction]
        [CapSubscribe("#.rabbitmq.mysql2")]
        public void Subscriber(DateTime time)
        {
            Debug.Write("{DateTime.Now}------------- Subscriber invoked, Sent time:{time}");
            Console.WriteLine($@"{DateTime.Now}------------- Subscriber invoked, Sent time:{time}");
        }
    }
}
