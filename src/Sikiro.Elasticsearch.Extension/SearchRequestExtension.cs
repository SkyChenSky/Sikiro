using Nest;

namespace Sikiro.Elasticsearch.Extension
{
    public static class SearchRequestExtension
    {
        public static SearchDescriptor<T> SearchAfterTimestamp<T>(this SearchDescriptor<T> searchDescriptor, long? timestamp) where T : class
        {
            if (timestamp.HasValue)
                searchDescriptor = searchDescriptor.SearchAfter(timestamp.Value);

            return searchDescriptor;
        }
    }
}
