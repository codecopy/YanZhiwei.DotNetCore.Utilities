namespace YanZhiwei.NetStandard.Newtonsoft.Json.Utilities
{
    using global::Newtonsoft.Json;
    using System;
    using System.IO;
    
    /// <summary>
    /// Json 辅助类
    /// </summary>
    public sealed class JsonHelper
    {
        #region Methods
        
        /*
         * 参考：
         * 1.http://weblog.west-wind.com/posts/2008/Sep/03/DataTable-JSON-Serialization-in-JSONNET-and-JavaScriptSerializer
         * 2.http://blog.prabir.me/posts/json-in-classical-web-services-asmx
         */
        
        /// <summary>
        /// 反序列化Json数据格式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonText">Json文本</param>
        /// <returns>反序列化</returns>
        public static T Deserialize<T>(string jsonText)
        {
            T _jsonResult = default(T);
            
            if(!string.IsNullOrEmpty(jsonText))
            {
                JsonSerializer _json = new JsonSerializer();
                JsonInitialize(_json);
                
                using(StringReader reader = new StringReader(jsonText))
                {
                    using(JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        _jsonResult = _json.Deserialize<T>(jsonReader);
                    }
                }
            }
            
            return _jsonResult;
        }
        
        /// <summary>
        /// 序列化数据为Json数据格式.
        /// <para>说明 [JsonProperty("姓名")]重命名属性名称</para>
        /// <para>说明 [JsonIgnore]忽略属性</para>
        /// </summary>
        /// <param name="value">需要序列化对象</param>
        /// <returns>Jsonz字符串</returns>
        public static string Serialize(object value)
        {
            Type _type = value.GetType();
            JsonSerializer _json = new JsonSerializer();
            JsonInitialize(_json);
            
            using(StringWriter writer = new StringWriter())
            {
                using(JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.None;
                    jsonWriter.QuoteChar = '"';
                    _json.Serialize(jsonWriter, value);
                    return writer.ToString();
                }
            }
        }
        
        private static void JsonInitialize(JsonSerializer jsonSerializer)
        {
            if(jsonSerializer != null)
            {
                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
        }
        
        #endregion Methods
    }
}