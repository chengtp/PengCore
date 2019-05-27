using Newtonsoft.Json;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace DDD.Common
{
    public static class SerializationHelper
    {
        #region Json.Net方式

        /// <summary>
        /// 实体转json 对象
        /// </summary>
        /// <returns></returns>
        public static string modelToJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// json 对象转实体
        /// </summary>
        /// <returns></returns>
        public static T jsonToModel<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// json 转 xml (return XNode) 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static XNode jsonToXml(string jsonString, string rootName)
        {
            if (string.IsNullOrWhiteSpace(rootName))
            {
                rootName = "root";
            }
            XNode node = JsonConvert.DeserializeXNode(jsonString, rootName);

            return node;
        }

        /// <summary>
        ///  xml 转 json (XmlString)
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static string xmlToJsonByXmlString(string XmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlString);
            string result = JsonConvert.SerializeXmlNode(doc);
            return result;
        }

        /// <summary>
        ///  xml 转 json（path）
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static string xmlToJsonByPath(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            string result = JsonConvert.SerializeXmlNode(doc);
            return result;
        }
        /// <summary>
        ///  xml 转 json（inStream）
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static string xmlToJsonByStream(System.IO.Stream inStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(inStream);
            string result = JsonConvert.SerializeXmlNode(doc);
            return result;
        }

        #endregion


        #region xml文件 转对象
        /// <summary>
        /// xml文件 转对象
        /// </summary>
        /// <param name="xmlFileName">路径</param>
        /// <param name="type">类型：查询，添加，修改，删除 存储过程</param>
        /// <param name="idName">具体sql标识</param>
        /// <param name="idParamName">具体参数标识</param>
        /// <returns></returns>
        public static Common.SqlModel XmlFileToStringSql(string xmlFileName, string idName, string idParamName = null)
        {
            XmlDocument doc = new XmlDocument();
            SqlModel model = new SqlModel();
            model.id = idName;

            xmlFileName = Environment.CurrentDirectory + "/Maps/" + xmlFileName;
            //  xmlFileName = HttpContext.Current.Server.MapPath("/Maps/") + xmlFileName;

            //加载文档
            doc.Load(xmlFileName);
            //查询sql语句
            XmlNodeList xmlNodeList = doc.SelectSingleNode("/sqlMap/statements").SelectNodes("statement");
            if (xmlNodeList != null)
            {
                foreach (XmlNode item in xmlNodeList)
                {
                    if (item.Attributes.GetNamedItem("id") != null && item.Attributes.GetNamedItem("id").InnerText == idName)
                    {
                        model.oprationType = (OperationType)Enum.Parse(typeof(OperationType), item.Attributes.GetNamedItem("type").InnerText);
                        if (model.oprationType != OperationType.procedure)
                        {
                            model.commandType = Common.OperationType.select;
                        }
                        else
                        {
                            model.commandType = Common.OperationType.procedure;
                        }
                        model.sqlStatement = item.InnerText;
                        if (item.Attributes.GetNamedItem("parameterMap") != null)
                        {
                            model.parameterMap = item.Attributes.GetNamedItem("parameterMap").InnerText;
                        }
                        break;
                    }
                }
            }
            //查询参数
            XmlNodeList xmlNodeParamList = doc.SelectNodes("/sqlMap/parameterMaps/parameterMap");
            if (xmlNodeParamList != null)
            {
                foreach (XmlNode item in xmlNodeParamList)
                {
                    if (item.Attributes.GetNamedItem("id") != null && item.Attributes.GetNamedItem("id").InnerText == model.parameterMap)
                    {
                        XmlNodeList xmlNodeParamListPa = item.SelectNodes("parameter");
                        if (xmlNodeParamListPa != null && xmlNodeParamListPa.Count > 0)
                        {
                            List<SqlParameterModels> modelParams = new List<SqlParameterModels>();

                            foreach (XmlNode itemListPa in xmlNodeParamListPa)
                            {
                                SqlParameterModels modelParam = new SqlParameterModels();
                                modelParam.property = itemListPa.Attributes.GetNamedItem("property").InnerText;
                                modelParam.propertyType = itemListPa.Attributes.GetNamedItem("propertyType").InnerText;
                                modelParam.column = itemListPa.Attributes.GetNamedItem("column").InnerText;
                                modelParams.Add(modelParam);
                            }
                            model.listParameter = modelParams;
                        }

                        break;
                    }
                }
            }

            return model;
        }

        #endregion

    }


    #region xml 转 实体对象

    //xml 转 实体对象：目的查找sql语句
    public class SqlModel
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public DDD.Common.OperationType oprationType { get; set; }
        /// <summary>
        /// 指定如何解释命令字符串。
        /// </summary>
        public Common.OperationType commandType { get; set; } = Common.OperationType.select;
        /// <summary>
        /// 传入参数id值：唯一标识
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 需要执行的sql
        /// </summary>
        public string sqlStatement { get; set; }
        /// <summary>
        /// 参数id
        /// </summary>
        public string parameterMap { get; set; }
        /// <summary>
        /// 参数集合
        /// </summary>
        public List<SqlParameterModels> listParameter { get; set; }
    }

    /// <summary>
    /// 参数类
    /// </summary>
    public class SqlParameterModels
    {
        /// <summary>
        /// 实体参数
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public string propertyType { get; set; }
        /// <summary>
        /// 对应sql中的参数列名称
        /// </summary>
        public string column { get; set; }
    }

    #endregion

}

