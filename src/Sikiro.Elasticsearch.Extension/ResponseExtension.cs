using Nest;
using Sikiro.Tookits.Base;

namespace Sikiro.Elasticsearch.Extension
{
    public static class ResponseExtension
    {
        public static ApiResult<TResult> GetApiResult<T, TResult>(this ISearchResponse<T> searchResponse)
            where T : class, new() where TResult : class, new()
        {
            return searchResponse.ApiCall.Success
                ? ApiResult<TResult>.IsSuccess((TResult)searchResponse.Documents)
                : ApiResult<TResult>.IsFailed(searchResponse.ApiCall.OriginalException.Message);
        }

        public static ApiResult GetApiResult(this CreateResponse createResponse)
        {
            return createResponse.ApiCall.Success
                ? ApiResult.IsSuccess()
                : ApiResult.IsFailed(createResponse.ApiCall.OriginalException.Message);
        }
    }
}
