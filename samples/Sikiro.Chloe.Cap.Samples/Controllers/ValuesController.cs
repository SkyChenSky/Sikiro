using System;
using System.Globalization;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Chloe.Cap.Samples.Db;

namespace Sikiro.Chloe.Cap.Samples.Controllers
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

        [Route("~/without/transaction")]
        public async Task<IActionResult> WithoutTransaction()
        {
            await _capBus.PublishAsync("sample.rabbitmq.mysql2", DateTime.Now);

            return Ok();
        }

        [Route("~/adonet/transaction")]
        public IActionResult AdonetWithTransaction()
        {
            _businessPlatformContext.UseTransactionEx(_capBus, () =>
            {
                _businessPlatformContext.Insert(new Test
                {
                    Id = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                });

                _businessPlatformContext.Insert(new Test1
                {
                    Time = DateTime.Now
                });

                _capBus.Publish("sample.rabbitmq.mysql2", DateTime.Now);
            });

            return Ok();
        }
    }
}
