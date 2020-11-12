using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Sikiro.Nosql.Mongo.Base;
using Sikiro.Nosql.Mongo.Extension;
using Sikiro.Tookits.Base;

namespace Sikiro.Nosql.Mongo
{
    #region MongoDb操作封装

    /// <summary>
    /// MongoDb操作封装
    /// </summary>
    public class MongoRepository
    {
        #region 初始化
        private readonly MongoClient _mongoClient;
        private string DataBaseName;

        public MongoRepository(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
        }

        public MongoRepository(string connectionString, string dataBaseName)
        {
            _mongoClient = new MongoClient(connectionString);
            DataBaseName = dataBaseName;
        }

        static MongoRepository()
        {
            ConventionRegistry.Register("IgnoreExtraElements",
                new ConventionPack { new IgnoreExtraElementsConvention(true) }, type => true);

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
        }

        #endregion

        #region 库、集合

        private static MongoAttribute GetMongoAttribute<T>()
        {
            var mongoAttribute = typeof(T).GetMongoAttribute();
            if (mongoAttribute == null)
                throw new ArgumentException("MongoAttribute不能为空");

            return mongoAttribute;
        }

        public IMongoCollection<T> GetCollection<T>(string database, string collection)
        {
            var db = _mongoClient.GetDatabase(database);
            return db.GetCollection<T>(collection);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            if (!string.IsNullOrEmpty(DataBaseName))
                return GetCollection<T>(DataBaseName, typeof(T).Name);

            var mongoAttribute = GetMongoAttribute<T>();

            return GetCollection<T>(mongoAttribute.Database, mongoAttribute.Collection ?? typeof(T).Name);
        }

        public List<string> ListDatabases()
        {
            var dbList = new List<string>();
            using (var cursor = _mongoClient.ListDatabases())
            {
                cursor.ForEachAsync(d => dbList.Add(d.ToString())).ConfigureAwait(false);
            }

            return dbList;
        }

        public List<string> ListCollections(string database)
        {
            var db = _mongoClient.GetDatabase(database);
            var collList = new List<string>();

            using (var cursor = db.ListCollections())
            {
                cursor.ForEachAsync(d => collList.Add(d.ToString())).ConfigureAwait(false);
            }
            return collList;
        }

        public void DropCollection(string database, string collection)
        {
            var db = _mongoClient.GetDatabase(database);
            db.DropCollection(collection);
        }

        #endregion

        #region 增

        #region 增（同步）

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        public void Add<T>(string database, string collection, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            coll.InsertOne(entity);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public void Add<T>(T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            coll.InsertOne(entity);
        }

        #endregion

        #region 增（异步）

        /// <summary>
        /// 增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        /// <returns></returns>
        public Task AddAsync<T>(string database, string collection, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            return coll.InsertOneAsync(entity);
        }

        /// <summary>
        /// 增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        /// <returns></returns>
        public Task AddAsync<T>(T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            return coll.InsertOneAsync(entity);
        }

        #endregion

        #endregion

        #region 无则新增
        /// <summary>
        /// 无则新增
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection">不新增的条件</param>
        /// <param name="predicate"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddIfNotExist<T>(string database, string collection, Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            if (coll.CountDocuments(predicate) > 0)
                return false;

            coll.InsertOne(entity);
            return true;
        }

        /// <summary>
        /// 无则新增
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddIfNotExist<T>(string database, string collection, T entity) where T : MongoEntity
        {
            return AddIfNotExist(database, collection, a => a.Id == entity.Id, entity);
        }

        /// <summary>
        /// 无则新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddIfNotExist<T>(T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            if (coll.CountDocuments(a => a.Id == entity.Id) > 0)
                return false;

            coll.InsertOne(entity);
            return true;
        }

        /// <summary>
        /// 无则新增
        /// </summary>
        /// <param name="predicate">不新增的条件</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddIfNotExist<T>(Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            if (coll.CountDocuments(predicate) > 0)
                return false;

            coll.InsertOne(entity);
            return true;
        }
        #endregion

        #region 批量增

        #region 批量增（异步）

        /// <summary>
        /// 批量增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        public async Task BatchAddAsync<T>(string database, string collection, List<T> entity) where T : MongoEntity
        {
            if (!entity.Any())
                return;

            var coll = GetCollection<T>(database, collection);

            await coll.InsertManyAsync(entity);
        }

        /// <summary>
        /// 批量增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public async Task BatchAddAsync<T>(List<T> entity) where T : MongoEntity
        {
            if (!entity.Any())
                return;

            var coll = GetCollection<T>();

            await coll.InsertManyAsync(entity);
        }

        #endregion

        #region 批量增（同步）

        /// <summary>
        /// 批量增（同步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entities"></param>
        public void BatchAdd<T>(string database, string collection, IEnumerable<T> entities) where T : MongoEntity
        {
            if (!entities.Any())
                return;

            var coll = GetCollection<T>(database, collection);

            coll.InsertMany(entities);
        }

        /// <summary>
        /// 批量增（同步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">实体(文档)</param>
        public void BatchAdd<T>(IEnumerable<T> entities) where T : MongoEntity
        {
            if (!entities.Any())
                return;

            var coll = GetCollection<T>();

            coll.InsertMany(entities);
        }

        #endregion

        #endregion

        #region 新增更新
        #region 同步
        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public long Set<T>(string database, string collection, Expression<Func<T, bool>> predicate, T t) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var reuslt = coll.ReplaceOne(predicate, t, new ReplaceOptions { IsUpsert = true });

            return reuslt.ModifiedCount;
        }

        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public long Set<T>(string database, string collection, T t) where T : MongoEntity
        {
            return Set(database, collection, a => a.Id == t.Id, t);
        }

        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public long Set<T>(T t) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var reuslt = coll.ReplaceOne(a => a.Id == t.Id, t, new ReplaceOptions { IsUpsert = true });

            return reuslt.ModifiedCount;
        }
        #endregion

        #region 异步
        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<long> SetAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate, T t) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var reuslt = await coll.ReplaceOneAsync(predicate, t, new ReplaceOptions { IsUpsert = true });

            return reuslt.ModifiedCount;
        }

        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<long> SetAsync<T>(string database, string collection, T t) where T : MongoEntity
        {
            return await SetAsync(database, collection, a => a.Id == t.Id, t);
        }

        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<long> SetAsync<T>(T t) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var reuslt = await coll.ReplaceOneAsync(a => a.Id == t.Id, t, new ReplaceOptions { IsUpsert = true });

            return reuslt.ModifiedCount;
        }
        #endregion
        #endregion

        #region 删

        #region 删（同步）

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Delete<T>(T entity) where T : MongoEntity
        {
            return Delete<T>(a => a.Id == entity.Id) > 0;
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public long Delete<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var result = coll.DeleteMany(predicate).DeletedCount;

            return result;
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public long Delete<T>(string database, string collection, Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var result = coll.DeleteMany(predicate).DeletedCount;

            return result;
        }

        #endregion

        #region 删（异步）

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public Task<long> DeleteAsync<T>(T entity) where T : MongoEntity
        {
            return DeleteAsync<T>(a => a.Id == entity.Id);
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var result = await coll.DeleteManyAsync(predicate);

            return result.DeletedCount;
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public Task<long> DeleteAsync<T>(string database, string collection, T entity) where T : MongoEntity
        {
            return DeleteAsync<T>(database, collection, e => e.Id == entity.Id);
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var result = await coll.DeleteManyAsync(predicate);

            return result.DeletedCount;
        }

        #endregion

        #endregion

        #region 改

        #region 改（同步）

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update<T>(T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var updateDefinitionList = entity.GetUpdateDefinition();

            var result = coll.UpdateOne(a => a.Id == entity.Id, updateDefinitionList);

            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public long Update<T>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> updateExpression) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var updateDefinitionList = MongoUpdateExpression<T>.GetUpdateDefinition(updateExpression);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = coll.UpdateMany<T>(whereExpression, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="entity">字段</param>
        /// <returns></returns>
        public long Update<T>(string database, string collection, Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var updateDefinitionList = entity.GetUpdateDefinition();

            var result = coll.UpdateOne<T>(predicate, updateDefinitionList);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="whereExpression">条件</param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public long Update<T>(string database, string collection, Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> updateExpression) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var updateDefinitionList = MongoUpdateExpression<T>.GetUpdateDefinition(updateExpression);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = coll.UpdateMany<T>(whereExpression, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        #endregion

        #region 改（异步）

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体（根据主键更新）</param>
        /// <returns></returns>
        public Task<long> UpdateAsync<T>(T entity) where T : MongoEntity
        {
            return UpdateAsync(a => a.Id == entity.Id, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="entity">实体（根据主键更新）</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var updateDefinitionList = entity.GetUpdateDefinition();

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = await coll.UpdateOneAsync<T>(predicate, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var updateDefinitionList = entity.GetUpdateDefinition();

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = await coll.UpdateOneAsync<T>(predicate, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="lambda">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(string database, string collection,
            Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var updateDefinitionList = MongoUpdateExpression<T>.GetUpdateDefinition(lambda);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = await coll.UpdateManyAsync<T>(predicate, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="lambda">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var updateDefinitionList = MongoUpdateExpression<T>.GetUpdateDefinition(lambda);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = await coll.UpdateManyAsync<T>(predicate, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        #endregion

        #endregion

        #region 查

        #region 查（同步）

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            return Get(predicate, null);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            return find.FirstOrDefault();
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector"></param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public T Get<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> selector, Func<Sort<T>, Sort<T>> sort) where T : MongoEntity
        {
            return Get<T, T>(database, collection, predicate, selector, sort);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="selector">查询字段</param>
        /// <returns></returns>
        public TResult Get<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where T : MongoEntity
        {
            return Get(predicate, selector, null);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public TResult Get<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            return find.Project(selector).FirstOrDefault();
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public TResult Get<T, TResult>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            return find.Project(selector).FirstOrDefault();
        }

        #endregion

        #region 查（异步）

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var find = coll.Find(predicate);
            if (projector != null)
                find = find.Project(projector);

            return find.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var find = coll.Find(predicate);
            if (projector != null)
                find = find.Project(projector);

            return find.FirstOrDefaultAsync();
        }

        #endregion

        #endregion

        #region 列表

        #region 列表（同步）

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="sort">排序</param>
        /// <param name="top">取X</param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int? top) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            if (top != null)
                find = find.Limit(top);

            return find.Project(selector).ToList();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int? top) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            if (top != null)
                find = find.Limit(top);

            return find.Project(selector).ToList();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort) where T : MongoEntity
        {
            return ToList(predicate, selector, sort, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where T : MongoEntity
        {
            return ToList(predicate, selector, null);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> ToList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int? top) where T : MongoEntity
        {
            return ToList(predicate, a => a, sort, top);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> ToList<T>(Expression<Func<T, bool>> predicate, int top) where T : MongoEntity
        {
            return ToList(predicate, null, top);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> ToList<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            return ToList(predicate, null, null);
        }

        #endregion

        #region 列表（异步）

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Task<List<T>> ToListAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int? limit = null) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var find = coll.Find(predicate);

            if (projector != null)
                find = find.Project(projector);

            if (limit != null)
                find = find.Limit(limit);

            return find.ToListAsync();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Task<List<T>> ToListAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int? limit) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var find = coll.Find(predicate);

            if (projector != null)
                find = find.Project(projector);

            if (limit != null)
                find = find.Limit(limit);

            return find.ToListAsync();
        }

        #endregion

        #endregion

        #region 分页（采用了skip在数据量大了后会有性能问题，建议使用瀑布流加载+条件索引）

        #region 分页（同步）

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection">集合</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="sort">排序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="database">库</param>
        /// <param name="selector">查询字段</param>
        /// <returns></returns>
        public PageList<TResult> PageList<T, TResult>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize) where TResult : class
        {
            var coll = GetCollection<T>(database, collection);

            var count = (int)coll.CountDocuments<T>(predicate);

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);

            var items = find.Project(selector).ToList();

            return new PageList<TResult>(pageIndex, pageSize, count, items);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="sort">排序字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        /// <returns></returns>
        public PageList<TResult> PageList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize) where TResult : class
        {
            var coll = GetCollection<T>();

            var count = (int)coll.CountDocuments<T>(predicate);

            var find = coll.Find(predicate);

            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());

            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);

            var items = find.Project(selector).ToList();

            return new PageList<TResult>(pageIndex, pageSize, count, items);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        /// <returns></returns>
        public PageList<TResult> PageList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, int pageIndex, int pageSize) where TResult : class
        {
            return PageList(predicate, selector, null, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        public PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize) where T : MongoEntity
        {
            return PageList(predicate, a => a, sort, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        public PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize) where T : MongoEntity
        {
            return PageList(predicate, null, pageIndex, pageSize);
        }

        #endregion

        #region 分页（异步）

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<T>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, T>> projector = null,
            int pageIndex = 1, int pageSize = 20, Expression<Func<T, object>> orderby = null, bool desc = false) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var count = (int)await coll.CountDocumentsAsync<T>(predicate);

            var find = coll.Find(predicate);

            if (projector != null)
                find = find.Project(projector);

            if (orderby != null)
                find = desc ? find.SortByDescending(@orderby) : find.SortBy(@orderby);

            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);

            var items = await find.ToListAsync();

            return new PageList<T>(pageIndex, pageSize, count, items);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<T>(string database, string collection,
            Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int pageIndex = 1, int pageSize = 20,
            Expression<Func<T, object>> orderby = null, bool desc = false) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var count = (int)await coll.CountDocumentsAsync<T>(predicate);

            var find = coll.Find(predicate);

            if (projector != null)
                find = find.Project(projector);

            if (orderby != null)
                find = desc ? find.SortByDescending(@orderby) : find.SortBy(@orderby);

            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);

            var items = await find.ToListAsync();

            return new PageList<T>(pageIndex, pageSize, count, items);
        }

        #endregion

        #endregion

        #region 是否存在

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public bool Exists<T>(Expression<Func<T, bool>> predicate = null) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            var result = predicate != null ? coll.CountDocuments(predicate) : coll.EstimatedDocumentCount();

            return result > 0;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public bool Exists<T>(string database, string collection, Expression<Func<T, bool>> predicate = null) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            var result = predicate != null ? coll.CountDocuments(predicate) : coll.EstimatedDocumentCount();

            return result > 0;
        }
        #endregion

        #region 条数
        /// <summary>
        /// 按条件查询条数
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public long Count<T>(Expression<Func<T, bool>> predicate = null) where T : MongoEntity
        {
            var coll = GetCollection<T>();

            return predicate != null ? coll.CountDocuments(predicate) : coll.EstimatedDocumentCount();
        }

        /// <summary>
        /// 按条件查询条数
        /// </summary>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public long Count<T>(string database, string collection, Expression<Func<T, bool>> predicate = null) where T : MongoEntity
        {
            var coll = GetCollection<T>(database, collection);

            return predicate != null ? coll.CountDocuments(predicate) : coll.EstimatedDocumentCount();
        }
        #endregion

        #region 原子操作

        /// <summary>
        /// 查询出更新结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public T GetAndUpdate<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression)
        {
            var coll = GetCollection<T>(database, collection);

            var updateDefinitionList = MongoUpdateExpression<T>.GetUpdateDefinition(updateExpression);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            return coll.FindOneAndUpdate(predicate, updateDefinitionBuilder, new FindOneAndUpdateOptions<T, T>
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        /// <summary>
        /// 按条件查询条数
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="updateExpression">更新结果</param>
        /// <returns></returns>
        public T GetAndUpdate<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression)
        {
            var coll = GetCollection<T>();

            var updateDefinitionList = MongoUpdateExpression<T>.GetUpdateDefinition(updateExpression);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            return coll.FindOneAndUpdate(predicate, updateDefinitionBuilder, new FindOneAndUpdateOptions<T, T>
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        #endregion
    }

    #endregion
}