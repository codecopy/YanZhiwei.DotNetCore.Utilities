namespace YanZhiwei.DotNetCore.Interfaces
{
    /// <summary>
    /// 分页集合接口
    /// </summary>
    public interface IPagedList
    {
        #region Properties

        /// <summary>
        /// 当前页索引
        /// </summary>
        int CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        int TotalItemCount
        {
            get;
            set;
        }

        #endregion Properties
    }
}