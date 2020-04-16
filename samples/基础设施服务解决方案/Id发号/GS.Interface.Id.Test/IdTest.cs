using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using WebApiClient;

namespace Sikiro.Interface.Id.Test
{
    public class IdTest
    {
        private IId _id;

        [SetUp]
        public void Setup()
        {
            HttpApi.Register<IId>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri("http://localhost:8099/");
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            });

            _id = HttpApi.Resolve<IId>();
        }

        [Test]
        public void Logon_IsNormal_IsTrue()
        {
            var sw = Stopwatch.StartNew();

            Enumerable.Range(1, 1000).AsParallel().ForAll(a =>
             {
                 var order = _id.Create("GS|D10");
             });

            sw.Stop();
        }
    }
}