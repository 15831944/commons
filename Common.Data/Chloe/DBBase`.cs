using Chloe;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.String;

namespace System.Data.Chloe
{
    public abstract class DBBase<T> : DBBase where T : class
    {
        #region 增

        /// <summary>
        /// lambda方式插入
        /// </summary>
        /// <param name="content">lambda表达式实体</param>
        /// <param name="table">表名，可选</param>
        /// <returns>插入数据的主键</returns>
        public object Insert(Expression<Func<T>> content, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Insert(content);
            }
            else
            {
                return this.DbContext.Insert(content, table);
            }
        }

        /// <summary>
        /// POCO方式插入
        /// </summary>
        /// <param name="entity">POCO实体</param>
        /// <param name="table">表名，可选</param>
        /// <returns>插入的数据</returns>
        public T Insert(T entity, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Insert(entity);
            }
            else
            {
                return this.DbContext.Insert(entity, table);
            }
        }

        /// <summary>
        /// 批量插入POCO
        /// </summary>
        /// <param name="entities">POCO实体列表</param>
        /// <param name="keepIdentity">是否要把自增属性值插入到数据库</param>
        public void Insert(List<T> entities, bool keepIdentity = false)
        {
            this.DbContext.InsertRange(entities, keepIdentity);
            return;
        }

        #endregion 增

        #region 删

        /// <summary>
        /// lambda方式删除
        /// </summary>
        /// <param name="condition">lambda表达式条件</param>
        /// <param name="table">表名，可选</param>
        /// <returns>删除的行数</returns>
        public int Delete(Expression<Func<T, bool>> condition, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Delete(condition);
            }
            else
            {
                return this.DbContext.Delete(condition, table);
            }
        }

        /// <summary>
        /// POCO方式删除
        /// </summary>
        /// <param name="entity">POCO实体条件</param>
        /// <param name="table">表名，可选</param>
        /// <returns>删除的行数</returns>
        public int Delete(T entity, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Delete(entity);
            }
            else
            {
                return this.DbContext.Delete(entity, table);
            }
        }

        /// <summary>
        /// 主键方式删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="table">表名，可选</param>
        /// <returns>删除的行数</returns>
        public int Delete(object key, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.DeleteByKey<T>(key);
            }
            else
            {
                return this.DbContext.DeleteByKey<T>(key, table);
            }
        }

        #endregion 删

        #region 改

        /// <summary>
        /// lambda方式更新数据
        /// </summary>
        /// <param name="condition">labmda表达式条件</param>
        /// <param name="content">lambda表达式实体</param>
        /// <param name="table">表名，可选</param>
        /// <returns>更新的行数</returns>
        public int Update(Expression<Func<T, bool>> condition, Expression<Func<T, T>> content, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Update(condition, content);
            }
            else
            {
                return this.DbContext.Update(condition, content, table);
            }
        }

        /// <summary>
        /// POCO方式更新数据
        /// </summary>
        /// <param name="entity">POCO实体</param>
        /// <param name="table">表名，可选</param>
        /// <returns>更改的行数</returns>
        public int Update(T entity, string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Update(entity);
            }
            else
            {
                return this.DbContext.Update(entity, table);
            }
        }

        /// <summary>
        /// POCO方式更新数据
        /// </summary>
        /// <param name="entity">POCO实体</param>
        /// <param name="table">表名，可选</param>
        /// <returns>更改的行数</returns>
        public int Update(T entity, Expression<Func<T, object>> content)
        {
            return this.DbContext.UpdateOnly(entity, content);
        }

        #endregion 改

        #region 查

        /// <summary>
        /// 根据主键查询单条数据
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="tracking"></param>
        /// <returns>POCO实体</returns>
        public T Query(object key, string table = null, bool tracking = false)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.QueryByKey<T>(key, tracking);
            }
            else
            {
                return this.DbContext.QueryByKey<T>(key, table, tracking);
            }
        }

        #endregion 查

        #region 基本查询

        /// <summary>
        /// 获取基本查询的强类型对象化查询接口
        /// </summary>
        /// <param name="table">表名，非必需</param>
        /// <returns>基本查询的强类型对象化查询接口</returns>
        public IQuery<T> GetQuery(string table = null)
        {
            if (IsNullOrWhiteSpace(table))
            {
                return this.DbContext.Query<T>();
            }
            else
            {
                return this.DbContext.Query<T>(table);
            }
        }

        /// <summary>
        /// lambda谓语获取基本查询的强类型对象化查询接口
        /// </summary>
        /// <param name="predicate">lambda谓语</param>
        /// <returns>基本查询的强类型对象化查询接口</returns>
        public IQuery<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return this.DbContext.Query(predicate);
        }

        #endregion 基本查询

        #region 联合查询

        /// <summary>
        /// 获取两个表连接的多表连接查询接口
        /// </summary>
        /// <typeparam name="T1">POCO实体类</typeparam>
        /// <param name="joinInfo">lambda表达式表述的表连接</param>
        /// <example>
        /// context.JoinQuery<User, City, Province>((user, city, province) => new object[]
        /// {
        /// JoinType.LeftJoin, user.CityId == city.Id,          /* 表 User 和 City 进行Left连接 */
        /// JoinType.LeftJoin, city.ProvinceId == province.Id   /* 表 City 和 Province 进行Left连接 */
        /// })
        /// </example>
        /// <returns>多表连接查询接口</returns>
        public IJoiningQuery<T, T1> GetQuery<T1>(Expression<Func<T, T1, object[]>> joinInfo)
        {
            return this.DbContext.JoinQuery(joinInfo);
        }

        /// <summary>
        /// 获取三个表连接的多表连接查询接口
        /// </summary>
        /// <typeparam name="T1">POCO实体类</typeparam>
        /// <typeparam name="T2">POCO实体类</typeparam>
        /// <param name="joinInfo">lambda表达式表述的表连接</param>
        /// <example>
        /// context.JoinQuery<User, City, Province>((user, city, province) => new object[]
        /// {
        /// JoinType.LeftJoin, user.CityId == city.Id,          /* 表 User 和 City 进行Left连接 */
        /// JoinType.LeftJoin, city.ProvinceId == province.Id   /* 表 City 和 Province 进行Left连接 */
        /// })
        /// </example>
        /// <returns>多表连接查询接口</returns>
        public IJoiningQuery<T, T1, T2> GetQuery<T1, T2>(Expression<Func<T, T1, T2, object[]>> joinInfo)
        {
            return this.DbContext.JoinQuery(joinInfo);
        }

        /// <summary>
        /// 获取四个表连接的多表连接查询接口
        /// </summary>
        /// <typeparam name="T1">POCO实体类</typeparam>
        /// <typeparam name="T2">POCO实体类</typeparam>
        /// <typeparam name="T3">POCO实体类</typeparam>
        /// <param name="joinInfo">lambda表达式表述的表连接</param>
        /// <example>
        /// context.JoinQuery<User, City, Province>((user, city, province) => new object[]
        /// {
        /// JoinType.LeftJoin, user.CityId == city.Id,          /* 表 User 和 City 进行Left连接 */
        /// JoinType.LeftJoin, city.ProvinceId == province.Id   /* 表 City 和 Province 进行Left连接 */
        /// })
        /// </example>
        /// <returns>多表连接查询接口</returns>
        public IJoiningQuery<T, T1, T2, T3> GetQuery<T1, T2, T3>(Expression<Func<T, T1, T2, T3, object[]>> joinInfo)
        {
            return this.DbContext.JoinQuery(joinInfo);
        }

        /// <summary>
        /// 获取五个表连接的多表连接查询接口
        /// </summary>
        /// <typeparam name="T1">POCO实体类</typeparam>
        /// <typeparam name="T2">POCO实体类</typeparam>
        /// <typeparam name="T3">POCO实体类</typeparam>
        /// <typeparam name="T4">POCO实体类</typeparam>
        /// <param name="joinInfo">lambda表达式表述的表连接</param>
        /// <example>
        /// <![CDATA[
        /// context.JoinQuery<User, City, Province>((user, city, province) => new object[]
        /// {
        /// JoinType.LeftJoin, user.CityId == city.Id,          /* 表 User 和 City 进行Left连接 */
        /// JoinType.LeftJoin, city.ProvinceId == province.Id   /* 表 City 和 Province 进行Left连接 */
        /// })
        /// ]]>
        /// </example>
        /// <returns>多表连接查询接口</returns>
        public IJoiningQuery<T, T1, T2, T3, T4> GetQuery<T1, T2, T3, T4>(Expression<Func<T, T1, T2, T3, T4, object[]>> joinInfo)
        {
            return this.DbContext.JoinQuery(joinInfo);
        }

        #endregion 联合查询
    }
}