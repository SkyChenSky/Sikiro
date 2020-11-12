using Sikiro.Nosql.Mongo;

namespace Sikiro.MongoDB.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var mongodbClient = new MongoRepository("mongodb://ge:shi2019@mdb.gshichina.com:5004/geshiimdb");

            var phone = mongodbClient.ToList<Phone>(a => a.Type == EGoods.Phone);

            var computer = mongodbClient.ToList<Computer>(a => a.Type == EGoods.Computer);

            var shirt = mongodbClient.ToList<Shirt>(a => a.Type == EGoods.Shirt);


            mongodbClient.Add(new Computer
            {
                Weight = 1,
                IsCanOpen360 = true,
                Name = "战神"
            });

            mongodbClient.Add(new Phone
            {
                IsCan5G = false,
                Long = 112,
                Wide = 50,
                Name = "IphoneX"
            });

            mongodbClient.Add(new Shirt
            {
                Color = "Yellow",
                Name = "战神"
            });
        }
    }
}
