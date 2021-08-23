using System;
using System.Collections.Generic;
using System.Threading;
using Nest;

namespace Sikiro.Elasticsearch.Extension
{
    public static class ElasticClientExtension
    {
        public static bool BulkAll<T>(this IElasticClient elasticClient, IndexName indexName, IEnumerable<T> list) where T : class
        {
            const int size = 1000;
            var tokenSource = new CancellationTokenSource();

            var observableBulk = elasticClient.BulkAll(list, f => f
                    .MaxDegreeOfParallelism(8)
                    .BackOffTime(TimeSpan.FromSeconds(10))
                    .BackOffRetries(2)
                    .Size(size)
                    .RefreshOnCompleted()
                    .Index(indexName)
                    .BufferToBulk((r, buffer) => r.IndexMany(buffer))
                , tokenSource.Token);

            var countdownEvent = new CountdownEvent(1);

            Exception exception = null;

            void OnCompleted()
            {
                Console.WriteLine("BulkAll Finished");
                countdownEvent.Signal();
            }

            var bulkAllObserver = new BulkAllObserver(
                onNext: response =>
                {
                    Console.WriteLine($"Indexed {response.Page * size} with {response.Retries} retries");
                },
                onError: ex =>
                {
                    Console.WriteLine("BulkAll Error : {0}", ex);
                    exception = ex;
                    countdownEvent.Signal();
                },
                OnCompleted);

            observableBulk.Subscribe(bulkAllObserver);

            countdownEvent.Wait(tokenSource.Token);

            if (exception != null)
            {
                Console.WriteLine("BulkHotelGeo Error : {0}", exception);
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool BulkAll<T>(this IElasticClient elasticClient, IEnumerable<T> list) where T : class
        {
            return BulkAll(elasticClient, typeof(T).GetRelationName(), list);
        }

        public static CreateResponse Create<T>(this IElasticClient elasticClient, T document) where T : class
        {
            return elasticClient.Create(document, a => a.Index(typeof(T).GetRelationName()));
        }

    }
}
