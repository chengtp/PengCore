using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;

namespace DDD.Common
{
    /// <summary>
    /// 反射类
    /// </summary>
    public static class Reflector
    {
        public static IEnumerable<PropertyInfo> GetScaffoldableProperties(object entity)
        {
            var props = entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(true).Any(attr => attr.GetType().Name == "EditableAttribute" && !IsEditable(p)) == false);
            return props.Where(p => p.PropertyType.IsSimpleType() || IsEditable(p));
        }
        public static IEnumerable<PropertyInfo> GetUpdateableProperties(object entity)
        {
            var updateableProperties = GetScaffoldableProperties(entity);
            //remove ones with ID
            updateableProperties = updateableProperties.Where(p => p.Name != "Id");
            //remove ones with key attribute
            updateableProperties = updateableProperties.Where(p => p.GetCustomAttributes(true).Any(attr => attr.GetType().Name == "PrimaryKeyAttribute") == false);
            //remove ones that are readonly
            updateableProperties = updateableProperties.Where(p => p.GetCustomAttributes(true).Any(attr => (attr.GetType().Name == "ReadOnlyAttribute") && IsReadOnly(p)) == false);

            return updateableProperties;
        }

        public static IEnumerable<PropertyInfo> GetIdProperties(object entity)
        {
            var type = entity.GetType();
            return GetIdProperties(type);
        }

        public static IEnumerable<PropertyInfo> GetIdProperties(Type type)
        {
            var tp = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(attr => attr.GetType().Name == "PrimaryKeyAttribute")).ToList();
            return tp.Any() ? tp : type.GetProperties().Where(p => p.Name == "Id");
        }


        public static string GetTableName(object entity)
        {
            var type = entity.GetType();
            return GetTableName(type);
        }

        public static string GetTableName(Type type)
        {
            var tableName = Encapsulate(type.Name);

            var tableattr = type.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
            if (tableattr != null)
            {
                tableName = Encapsulate(tableattr.Name);
                try
                {
                    if (!String.IsNullOrEmpty(tableattr.Schema))
                    {
                        string schemaName = Encapsulate(tableattr.Schema);
                        tableName = String.Format("{0}.{1}", schemaName, tableName);
                    }
                }
                catch (RuntimeBinderException)
                {
                    //Schema doesn't exist on this attribute.
                }
            }

            return tableName;
        }

        public static string GetColumnName(PropertyInfo propertyInfo)
        {
            var columnName = Encapsulate(propertyInfo.Name);

            var columnattr = propertyInfo.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == "ColumnAttribute") as dynamic;
            return columnName;
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(object entity)
        {
            if (entity == null) entity = new { };
            return entity.GetType().GetProperties();
        }

        private static string Encapsulate(string databaseword)
        {
            return string.Format("[{0}]", databaseword);
        }

        private static bool IsEditable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(false);
            if (attributes.Length > 0)
            {
                dynamic write = attributes.FirstOrDefault(x => x.GetType().Name == "EditableAttribute");
                if (write != null)
                {
                    return write.AllowEdit;
                }
            }
            return false;
        }
        public static bool IsReadOnly(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(false);
            if (attributes.Length > 0)
            {
                dynamic write = attributes.FirstOrDefault(x => x.GetType().Name == "ReadOnlyAttribute");
                if (write != null)
                {
                    return write.IsReadOnly;
                }
            }
            return false;
        }
    }
}
