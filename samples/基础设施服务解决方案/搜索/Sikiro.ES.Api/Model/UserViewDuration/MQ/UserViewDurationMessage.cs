using System;
using EasyNetQ;
using Sikiro.Bus.Extension;

namespace Sikiro.ES.Api.Model.UserViewDuration.MQ
{
    [Queue("Queue.Sikiro.UserViewDuration", ExchangeName = "Exchange.Sikiro.UserViewDuration")]
    public class UserViewDurationMessage : EasyNetQEntity
    {
        public long EntityId { get; set; }

        public long EntityType { get; set; }

        public long CharpterId { get; set; }

        public long UserId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public long Duration { get; set; }

        public string Ip { get; set; }
    }
}
