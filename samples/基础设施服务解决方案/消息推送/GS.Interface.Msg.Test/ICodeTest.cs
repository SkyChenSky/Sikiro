using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WebApiClient;

namespace GS.Interface.Msg.Test
{
    public class ICodeTest
    {
        private ICode _code;

        [SetUp]
        public void Setup()
        {
            HttpApi.Register<ICode>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri("http://localhost:8031/");
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            });

            _code = HttpApi.Resolve<ICode>();
        }

        [Test]
        public async Task Create_IsNormal_IsTrue()
        {
            var order = await _code.Create("+8618988561110", 120, "192.168.1.32");

            Assert.IsTrue(order.Success);
        }

        [Test]
        public async Task Repeat_IsNormal_IsTrue()
        {
            var order = await _code.Create("13533324275", 10, "192.168.1.32", 10);

            Assert.IsTrue(order.Success);
        }

        [Test]
        public async Task Vaild_IsNormal_IsTrue()
        {
            var order = await _code.Vaild("18988561110", "652544");

            Assert.IsTrue(order.Success);
        }
    }
}