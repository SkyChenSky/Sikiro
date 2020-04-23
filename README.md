# Blog
+ [.Net微服务实战之技术选型篇](https://www.cnblogs.com/skychen1218/p/12531412.html)
+ [.Net微服务实战之技术架构分层篇](https://www.cnblogs.com/skychen1218/p/12653155.html)
# Sikiro.Tookits 
Sikiro.Tookits is base And Frequently-used Tools Library.

## Getting Started

### Nuget

You can run the following command to install the Sikiro.Tookits in your project。

```
PM> Install-Package Sikiro.Tookits
```

### What does it have？

* Base
```c#
var pl = new PageList<User>(1, 10, 100, new List<User>());

var sr = new ServiceResult<User>();
if (sr.Error)
    return;
```
* Extension
```c#
var list = new List<User>().DistinctBy(a => a.Name);

DataTable dt = list.ToDataTable();

int numString = "1".TryInt(1);
```
* Helper
```c#
Guid guid = GuidHelper.GenerateComb();
```
and so on



# Sikiro.Nosql.Mongo
This is mongo repository.Base on MongoDB.Driver.It is easy to use.

## Getting Started

### Nuget

You can run the following command to install the Sikiro.Nosql.Mongo in your project。

```
PM> Install-Package Sikiro.Nosql.Mongo
```

### Connection

```c#
var mongoRepository = new MongoRepository("mongodb://10.1.20.143:27017");
```

### Defining User Entity
```c#
[Mongo("Chengongtest", "User")]
public class User : MongoEntity
{
    public string Name { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime BirthDateTime { get; set; }

    public User Son { get; set; }

    public int Sex { get; set; }

    public List<string> AddressList { get; set; }
}
```

### Add
```c#
var addresult = mongoRepository.Add(new User
{
    Name = "skychen",
    BirthDateTime = new DateTime(1991, 2, 2),
    AddressList = new List<string> { "guangdong", "guangzhou" },
    Sex = 1,
    Son = new User
    {
        Name = "xiaochenpi",
        BirthDateTime = DateTime.Now
    }
});
```

### UPDATE
Update according to the condition part field
```c#
mongoRepository.Update<User>(a => a.Id == u.Id, a => new User { AddressList = new List<string> { "guangdong", "jiangmen", "cuihuwan" } });
```

You can also update the entity field information based on the primary key 
```c#
getResult.Name = "superskychen";
mongoRepository.Update(getResult);
```

### DELETE
Delete according to the condition
```c#
mongoRepository.Delete<User>(a => a.Id == u.Id);
```

### QUERY

#### GET
Get the first data by filtering condition

```c#
var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
```
#### TOLIST
You can also query qualified data list.
```c#
var listResult = mongoRepository.ToList<User>(a => a.Id == u.Id);
```
### PAGELIST
```c#
var listResult = mongoRepository.PageList<User>(a => a.Id == u.Id, a => a.Desc(b => b.BirthDateTime), 1, 10);
```

### Finally a complete Demo

```c#
var url = "mongodb://10.1.20.143:27017";
var mongoRepository = new MongoRepository(url);

var u = new User
{
    Name = "skychen",
    BirthDateTime = new DateTime(1991, 2, 2),
    AddressList = new List<string> { "guangdong", "guangzhou" },
    Sex = 1,
    Son = new User
    {
        Name = "xiaochenpi",
        BirthDateTime = DateTime.Now
    }
};

var addresult = mongoRepository.Add(u);

var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
getResult.Name = "superskychen";

mongoRepository.Update(getResult);

mongoRepository.Update<User>(a => a.Id == u.Id, a => new User { AddressList = new List<string> { "guangdong", "jiangmen", "cuihuwan" } });

mongoRepository.Exists<User>(a => a.Id == u.Id);

mongoRepository.Delete<User>(a => a.Id == u.Id);
```

# ExcelClient
基于NPOI封装

## Getting Started


### 注册

```c#
 public void ConfigureServices(IServiceCollection services)
{
    services.AddExcelClient("http://rpc.gshichina.com/api/file");
}
```

### MVC导出
使用Get请求方式新开页面导出Excel
```c#
public class DefaultController : Controller
    {
        private readonly ExcelClient _ec;

        public DefaultController(ExcelClient ec)
        {
            _ec = ec;
        }

        public void Index()
        {
            var list = new List<Student>
            {
                new Student
                {
                    Id = "1",
                    Name = "123123"
                },
                new Student
                {
                    Id = "12",
                    Name = "asdasdqwe"
                }
            };
            _ec.HttpExport(list, "excel名称");
        }
    }
```

### WebApi导出
```c#
[HttpGet]
public async Task<ActionResult<string>> Get()
{
    var list = new List<Student>
    {
        new Student
        {
            Id = "1",
            Name = "123123"
        },
        new Student
        {
            Id = "12",
            Name = "asdasdqwe"
        }
    };

    return await _ec.HttpExportAsync(list);
}
```

### Base64导入
```c#
[HttpGet("import")]
public IEnumerable<Student> Import()
{
    var baseString = "data:application/vnd.ms-excel;base64,UEsDBBQAAAgIAFdGME8xSZgR7wAAANMCAAALAAAAX3Jl";

    return _ec.HttpImport<Student>(baseString);
}
```

### form导入
```c#
public void Import()
{
    var file = Request.Form.Files[0];
    _ec.HttpImport<object>(file);
}
```

# DotNetCore.CAP.MySql-分布式事务（最终一致性）
基于DotNetCore.CAP.MySql与DotNetCore.CAP.RabbitMQ封装

## Getting Started


### 注册
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddChloeDbContext<BusinessPlatformContext>("Server=im.gshichina.com;Port=5002;Database=business_platform;Uid=ge;Pwd=shi2019");

    services.AddCap(x =>
    {
        x.UseMySql("Server=im.gshichina.com;Port=5002;Database=business_platform;Uid=ge;Pwd=shi2019");
        x.UseRabbitMQ(option =>
        {
            option.HostName = "rabbitmq.gshichina.com";
            option.Port = 5112;
            option.UserName = "guest";
            option.Password = "guest";
        });
        x.UseDashboard();
        x.FailedRetryCount = 5;
        x.FailedRetryInterval = 30;
    });

    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
}
```

### 事务A端
```c#
[Route("api/[controller]")]
public class ValuesController : Controller
{
    private readonly ICapPublisher _capBus;
    private readonly BusinessPlatformContext _businessPlatformContext;

    public ValuesController(ICapPublisher capPublisher, BusinessPlatformContext businessPlatformContext)
    {
        _capBus = capPublisher;
        _businessPlatformContext = businessPlatformContext;
    }

    [Route("~/adonet/transaction")]
    public IActionResult AdonetWithTransaction()
    {
        _businessPlatformContext.UseTransactionEx(_capBus, () =>
        {
            _businessPlatformContext.Insert(new Test
            {
                Id = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            });

            _capBus.Publish("sample.rabbitmq.mysql2", DateTime.Now);
        });

        return Ok();
    }
}
```

### 事务B端
```c#
[Route("api/[controller]")]
public class ValuesController : Controller
{
    private readonly ICapPublisher _capBus;
    private readonly BusinessPlatformContext _businessPlatformContext;

    public ValuesController(ICapPublisher capPublisher, BusinessPlatformContext businessPlatformContext)
    {
        _capBus = capPublisher;
        _businessPlatformContext = businessPlatformContext;
    }

    [NonAction]
    [CapSubscribe("#.rabbitmq.mysql2")]
    public void Subscriber(DateTime time)
    {
        Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Sent time:{time}");
    }
}
```

### 注意点
1. UseTransactionEx里必须使用_capBus.Publish
2. 默认重发间隔60秒，重试第3次与第四次间隔4分钟，默认次数上限50次
3. 配置文档 ：http://cap.dotnetcore.xyz/user-guide/zh/cap/configuration/