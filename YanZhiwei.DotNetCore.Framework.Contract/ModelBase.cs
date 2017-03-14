namespace YanZhiwei.DotNetCore.Framework.Contract
{
    using System;

    /// <summary>
    /// 实体类基类
    /// </summary>
    public abstract class ModelBase<TPrimaryKey>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public ModelBase()
        {
            CreateTime = DateTime.Now;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey ID
        {
            get;
            set;
        }

        #endregion Properties
    }

    /// <summary>
    /// 定义默认主键类型为INT的实体基类
    /// </summary>
    public abstract class ModelBase : ModelBase<int>
    {
    }
}