using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DDD.Common
{
    /// <summary>
    /// DataTable转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DataTableConvertor<T> where T : class, new()
    {
        /// <summary>
        /// 将DataTable数据转换为实体对象数组（列名称等于属性名称）
        /// </summary>
        /// <param name="table">数据源</param>
        /// <returns></returns>
        public static T[] ConvertToEntityArray(DataTable table)
        {
            if (table == null || table.Rows.Count == 0) { return null; }
            T[] entitys = new T[table.Rows.Count];
            PropertyInfo p;
            Type t = typeof(T);
            Type propertyType;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                entitys[i] = new T();
                foreach (DataColumn column in table.Columns)
                {
                    //一列一个属性
                    //p = t.GetProperty(column.ColumnName);
                    p = GetPropertyInfoByName(t, column.ColumnName); //属性名大小写不敏感，防止写SQL语句的问题，造成对应不上

                    //属性没找到，或者属性为只读，则继续下一条
                    if (p == null || p.CanWrite == false) { continue; }
                    //如果值等于null，那就别设置了，继续下一条
                    if (table.Rows[i].IsNull(column) == true) { continue; }
                    //设置值
                    try
                    {
                        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = p.PropertyType.GetGenericArguments()[0];
                        }
                        else
                        {
                            propertyType = p.PropertyType;
                        }
                        //支持了数据库为int类型，而属性定义为枚举类型的转换
                        if (propertyType.IsEnum == true && column.DataType == typeof(Int32))
                        {
                            p.SetValue(entitys[i], table.Rows[i][column], null);
                        }
                        else
                        {
                            p.SetValue(entitys[i], Convert.ChangeType(table.Rows[i][column], propertyType), null);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return entitys;
        }

        /// <summary>
        /// 将DataRow数据转换为实体对象（列名称等于属性名称）
        /// </summary>
        /// <param name="drRow">数据行</param>
        /// <returns>对象实例</returns>
        public static T ConvertToEntity(DataRow drRow)
        {
            if (drRow == null) { return null; }
            T entitys = new T();
            PropertyInfo p;
            Type t = typeof(T);
            Type propertyType;
            foreach (DataColumn column in drRow.Table.Columns)
            {
                //一列一个属性
                //p = t.GetProperty(column.ColumnName);
                p = GetPropertyInfoByName(t, column.ColumnName); //属性名大小写不敏感，防止写SQL语句的问题，造成对应不上

                if (p != null && p.CanWrite == true)
                {
                    if (drRow.IsNull(column) == false)
                    {
                        //设置值
                        try
                        {
                            if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyType = p.PropertyType.GetGenericArguments()[0];
                            }
                            else
                            {
                                propertyType = p.PropertyType;
                            }
                            //支持了数据库为int类型，而属性定义为枚举类型的转换
                            if (propertyType.IsEnum == true && column.DataType == typeof(Int32))
                            {
                                p.SetValue(entitys, drRow[column], null);
                            }
                            else
                            {
                                p.SetValue(entitys, Convert.ChangeType(drRow[column], propertyType), null);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            return entitys;
        }



        ///// <summary>
        ///// 将读取的XML文档转换为实体对象数组（列名称等于属性名称）
        ///// </summary>
        ///// <param name="table">XML文档</param>
        ///// <returns>对象数组</returns>
        //public static T[] ConvertToEntityArray(string XmlFilePath)
        //{
        //    string strXmlPath = string.Format("{0}\\{1}.xml", XmlFilePath, typeof(T).Name);
        //    DataSet dsModel = new DataSet();
        //    dsModel.ReadXml(strXmlPath);
        //    if (ObjectOperation.IsDataSetNotNull(dsModel, 1))
        //    {
        //        return DataTableConvertor<T>.ConvertToEntityArray(dsModel.Tables[0]);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 转换成动态属性的实体数组
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T[] ConvertToDynamicPropertyEntityArray(DataTable table)
        {
            if (table == null || table.Rows.Count == 0) { return null; }
            if (typeof(T).IsSubclassOf(typeof(DynamicPropertyClass)) == false)
            {
                return ConvertToEntityArray(table);
            }

            T[] entitys = new T[table.Rows.Count];
            PropertyInfo p;
            Type propertyType;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                entitys[i] = new T();
                foreach (DataColumn column in table.Columns)
                {
                    p = (entitys[i] as DynamicPropertyClass).GetPropertyInfoByName(column.ColumnName);
                    if (p != null)
                    {
                        if (p.CanWrite == false) { continue; }
                        if (table.Rows[i].IsNull(column) == false)
                        {
                            //设置值
                            try
                            {
                                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    propertyType = p.PropertyType.GetGenericArguments()[0];
                                }
                                else
                                {
                                    propertyType = p.PropertyType;
                                }
                                //支持了数据库为int类型，而属性定义为枚举类型的转换
                                if (propertyType.IsEnum == true && column.DataType == typeof(Int32))
                                {
                                    p.SetValue(entitys[i], table.Rows[i][column], null);
                                }
                                else
                                {
                                    p.SetValue(entitys[i], Convert.ChangeType(table.Rows[i][column], propertyType), null);
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //不属于固定属性，则设置为动态属性
                        (entitys[i] as DynamicPropertyClass).SetProperty(column.ColumnName, column.DataType, table.Rows[i][column]);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 通过名称或取定义的属性信息（不区分大小写），不存在时返回Null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfoByName(Type entityType, string name)
        {
            IEnumerable<PropertyInfo> ps = entityType.GetProperties().Where(p => p.Name.ToLower().Equals(name.ToLower()) == true);

            if (ps.Count() > 0)
            {
                return ps.ElementAt(0);
            }
            else
            {
                return null;
            }

        }
    }
}
