# 基础设施服务
文件服务、分布式ID服务


## 文件服务-GS.Infrastructure.File

### 网关Base64上传接口

```
-POST http://api.gshichina.com/infrastructure/file/
```
- 请求参数
```json
{
"data":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAA8AAAAIcCAYAAAA5Xcd7AA"
}
```
- 响应参数
```json
{
  "message": "string",
  "exception": {
    "message": "string",
    "stackTrace": "string",
    "source": "string"
  },
  "success": true,
  "error": true,
  "failed": true
}
```

### 标签、报表导出接口
- 请求参数
```c#
var result = await _file.Export(new ReportRequest
{
      Url = "http://resource.gshichina.com/frx/20191216/03FC14FB4F9B39D2FA5285B723DB5289.frx",
      Param = new Dictionary<string, object>
      {
            {"Parameter1","测试数据123ssdf" },
            {"Parameter2",12343453 }
      }
});
```

- 响应参数
```json
{
  "resultCode": 0,
  "message": "string",
  "exception": {
    "message": "string",
    "stackTrace": "string",
    "source": "string"
  },
  "data": {
    "url": "string",
    "width": 0,
    "height": 0
  },
  "success": true,
  "error": true,
  "failed": true
}
```

## ID生成服务-GS.Infrastructure.Id

- API网关调用
```
-POST http://api.gshichina.com/infrastructure/idgenerate/Id/Generate?format=GS|yyyyMMdd
```
- RPC调用
```
http://fabioapi.gshichina.com/IDGenerate/
```
```c#
services.AddHttpApi<IId>().ConfigureHttpApiConfig(c =>
{
      c.HttpHost = new Uri(configuration["IdServerUrl"]);
      c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
});
```
```c#
public class GoodsWarehouseController : BaseController
{
      private readonly IId _id;

      public GoodsWarehouseController(IId id)
      {
          _id = id;
      }

      [HttpPost("CreateId")]
      public async Task<string> CreateId()
      {
          var id = await _id.Create(ConstString.OrderId);
          return id;
      }
}
```
- 格式规则

```
无参数 = GS.Tookits-GuidHelper.GenerateComb() ---- 1e73f039010262d4eab2b20c984becb4
```
```
GS|yyyyMMdd|D2 ---- GS2019092401
```
```
GS|D10 ---- GS0000000001
```