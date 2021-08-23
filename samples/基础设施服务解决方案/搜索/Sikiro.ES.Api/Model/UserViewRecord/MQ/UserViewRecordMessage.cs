using System;
using EasyNetQ;
using Sikiro.Bus.Extension;

namespace Sikiro.ES.Api.Model.UserViewRecord.MQ
{
    [Queue("Queue.SF.UserViewRecord", ExchangeName = "Exchange.SF.UserViewRecord")]
    public class UserViewRecordMessage : EasyNetQEntity
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
