using System;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.MongoDB.Demo{
    /// <summary>
    /// 发送短信
    /// </summary>
    [Mongo(DbConfig.Name,"ChengongGood")]
    public class Goods : MongoEntity
    {
        public string Name { get; set; }

        public virtual EGoods Type { get; protected set; }

        public DateTime CreateDateTime => DateTime.Now;
    }

    [Mongo(DbConfig.Name, "ChengongGood")]
    public class Computer : Goods
    {
        public override EGoods Type => EGoods.Computer;

        public int Weight { get; set; }

        public bool IsCanOpen360 { get; set; }
    }

    [Mongo(DbConfig.Name, "ChengongGood")]
    public class Phone : Goods
    {
        public override EGoods Type => EGoods.Phone;

        public int Long { get; set; }

        public int Wide { get; set; }

        public bool IsCan5G { get; set; }
    }

    [Mongo(DbConfig.Name, "ChengongGood")]
    public class Shirt : Goods
    {
        public override EGoods Type => EGoods.Shirt;

        public string Color { get; set; }
    }

    public enum EGoods
    {
        Computer = 1,
        Phone = 2,
        Shirt = 3
    }

    public class DbConfig
    {
        public const string Name = "geshiimdb";
    }
}
