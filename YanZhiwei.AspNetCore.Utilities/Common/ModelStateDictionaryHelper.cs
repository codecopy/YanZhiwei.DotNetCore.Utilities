using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace YanZhiwei.AspNetCore.Utilities.Common
{
    /// <summary>
    /// ModelStateDictionary 辅助类
    /// </summary>
    public static class ModelStateDictionaryHelper
    {
        /// <summary>
        /// 获取ModelState错误信息
        /// </summary>
        /// <param name="modelState">ModelStateDictionary</param>
        /// <returns>错误信息</returns>
        public static string GetModelStateError(this ModelStateDictionary modelState)
        {
            string _modelStateError = string.Empty;
            foreach (var item in modelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    _modelStateError = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return _modelStateError;
        }
    }
}