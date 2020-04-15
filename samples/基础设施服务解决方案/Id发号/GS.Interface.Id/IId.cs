using WebApiClient;
using WebApiClient.Attributes;

namespace GS.Interface.Id
{
    /// <summary>
    /// Id生成(GS|D10)
    /// </summary>
    public interface IId : IHttpApi
    {
        [HttpPost("Id/Generate")]
        ITask<string> Create(string format = null);
    }
}
