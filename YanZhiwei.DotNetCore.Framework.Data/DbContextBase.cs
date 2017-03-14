using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using YanZhiwei.DotNetCore.Framework.Contract;
using YanZhiwei.DotNetCore.Utilities.Collection;
using YanZhiwei.DotNetCore.Utilities.Common;

namespace YanZhiwei.DotNetCore.Framework.Data
{
    /// <summary>
    /// DAL基类，实现Repository通用泛型数据访问模式
    /// </summary>
    public class DbContextBase<F> : DbContext, IDataRepository<F>, IDisposable
    {
        public DbContextBase(DbContextOptions<DbContextBase<F>> options) : base(options)
        {
        }

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="connectionString">连接字符串</param>
        ///// <param name="auditLogger">IAuditable</param>
        //public DbContextBase(string connectionString, IAuditable auditLogger)
        //: this(connectionString)
        //{
        //    AuditLogger = auditLogger;
        //}

        /// <summary>
        /// 日志接口
        /// </summary>
        public IAuditable AuditLogger
        {
            get;
            set;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        public void Delete<T>(T entity)
        where T : ModelBase<F>
        {
            this.Entry<T>(entity).State = EntityState.Deleted;
            this.SaveChanges();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keyValues">查询依据的键</param>
        /// <returns>
        /// 实体类
        /// </returns>
        public new T Find<T>(params object[] keyValues)
        where T : ModelBase<F>
        {
            return this.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="conditions">委托.</param>
        /// <returns>
        /// 集合
        /// </returns>
        public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null)
        where T : ModelBase<F>
        {
            if (conditions == null)
                return this.Set<T>().ToList();
            else
                return this.Set<T>().Where(conditions).ToList();
        }

        /// <summary>
        /// 分页查找
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="S">泛型</typeparam>
        /// <param name="conditions">查找条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">分页集合</param>
        /// <returns>PagedList</returns>
        public PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex)
        where T : ModelBase<F>
        {
            var queryList = conditions == null ? this.Set<T>() : this.Set<T>().Where(conditions) as IQueryable<T>;
            return queryList.OrderByDescending(orderBy).ToPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns>
        /// 实体类
        /// </returns>
        public T Insert<T>(T entity)
        where T : ModelBase<F>
        {
            this.Set<T>().Add(entity);
            this.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库。
        /// </summary>
        /// <returns>
        /// 已写入基础数据库的对象的数目。
        /// </returns>
        public override int SaveChanges()
        {
            this.WriteAuditLog();
            var result = base.SaveChanges();
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">主键泛型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns>
        /// 实体类
        /// </returns>
        public new T Update<T>(T entity)
        where T : ModelBase<F>
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = EntityState.Modified;
            this.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 日志拦截写入
        /// </summary>
        internal void WriteAuditLog()
        {
            if (this.AuditLogger == null)
                return;

            foreach (var dbEntry in this.ChangeTracker.Entries<ModelBase>().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                AuditableAttribute _auditableAttr = dbEntry.Entity.GetType().GetTypeInfo().GetCustomAttribute<AuditableAttribute>();

                if (_auditableAttr == null)
                    continue;

                //string _operaterName = ServiceCallContext.Current.Operater.Name;
                //Task.Factory.StartNew(() =>
                //{
                //    TableAttribute _tableAttr = dbEntry.Entity.GetType().GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                //    string _tableName = _tableAttr != null ? _tableAttr.Name : dbEntry.Entity.GetType().Name;
                //    string _moduleName = dbEntry.Entity.GetType().FullName.Split('.').Skip(1).FirstOrDefault();
                //    this.AuditLogger.WriteLog(dbEntry.Entity.ID, _operaterName, _moduleName, _tableName, dbEntry.State.ToString(), dbEntry.Entity);
                //});
            }
        }
    }
}