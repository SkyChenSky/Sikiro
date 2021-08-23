using System;
using Nest;
using Sikiro.Tookits.Extension;

namespace Sikiro.Elasticsearch.Extension
{
    public abstract class ElasticsearchEntity
    {
        private Guid? _id;

        public Guid Id
        {
            get
            {
                _id ??= Guid.NewGuid();
                return _id.Value;
            }
            set => _id = value;
        }

        private long? _timestamp;

        [Number(NumberType.Long, Name = "timestamp")]
        public long Timestamp
        {
            get
            {
                _timestamp ??= DateTime.Now.DateTimeToTimestampOfMicrosecond();
                return _timestamp.Value;
            }
            set => _timestamp = value;
        }
    }
}
