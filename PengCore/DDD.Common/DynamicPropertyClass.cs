using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDD.Common
{
    /// <summary>
    /// 动态属性类（支持动态增加属性）
    /// </summary>
    public class DynamicPropertyClass
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DynamicPropertyClass()
        {
            this.IsCaseSensitive = true; //缺省区分大小写
            this.PropertyList = new Dictionary<string, DynamicProperty>();
        }


        /// <summary>
        /// 返回设置动态属性名称是否大小写敏感（缺省是敏感的，固有属性名称永远都是大小写不敏感的）
        /// （不允许有名称上和固有属性，仅仅是大小写不同的的名称的属性存在）
        /// </summary>
        private bool IsCaseSensitive { get; set; }

        /// <summary>
        /// 设置动态属性名称是否大小写敏感（缺省是敏感的，固有属性名称永远都是大小写不敏感的）
        /// </summary>
        /// <param name="isCaseSensitive"></param>
        public void SetCaseSensitive(bool isCaseSensitive)
        {
            this.IsCaseSensitive = isCaseSensitive;
        }


        /// <summary>
        /// 设置属性值（对于已经存在的属性可以不提供值的类型参数）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyValue"></param>
        public void SetProperty(string name, object propertyValue)
        {
            if (this.IsClassFiexedProperty(name) == true)
            {
                this.SetProperty(name, this.GetPropertyType(name), propertyValue);

            }
            else
            {
                //if (this.PropertyList == null || this.PropertyList.ContainsKey(name) == false)
                if (this.GetDynamicPropertyByName(name) == null)
                {
                    throw new ArgumentOutOfRangeException("name", string.Format("不存在属性{0}，必须提供属性值的类型", name));
                }
                this.SetProperty(name, this.GetPropertyType(name), propertyValue);
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyValueType"></param>
        /// <param name="propertyValue"></param>
        public void SetProperty(string name, Type propertyValueType, object propertyValue)
        {
            if (propertyValueType == null)
            {
                throw new ArgumentOutOfRangeException("propertyValueType", "属性值的类型为Null");
            }
            if (this.IsValidValue(propertyValueType, propertyValue) == false)
            {
                throw new ArgumentOutOfRangeException("propertyValue", string.Format("属性值不能正常转换成类型{0}", propertyValueType.Name));
            }

            if (this.IsClassFiexedProperty(name) == true)
            {
                //固定定义属性，则直接设置值后，返回

                //给定了类型，还需要判断类型是否正确
                if (this.GetPropertyType(name).Equals(propertyValueType) == false)
                {
                    throw new ArgumentOutOfRangeException("propertyValueType", string.Format("属性{0}为固定属性，给定值的类型（{1}）和类的属性定义（{2}）不符合",
                        name, propertyValueType.Name, this.GetPropertyType(name).Name));
                }


                this.GetPropertyInfoByName(name).SetValue(this, propertyValue, null);
                return;
            }

            if (this.PropertyList == null) { this.PropertyList = new Dictionary<string, DynamicProperty>(); }
            DynamicProperty dp = this.GetDynamicPropertyByName(name);
            if (dp == null)
            {
                DynamicProperty p = new DynamicProperty()
                {
                    Name = name,
                    PropertyType = propertyValueType,
                    Value = propertyValue
                };
                this.PropertyList.Add(name, p);
            }
            else
            {
                dp.Value = propertyValue;
            }


        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetPropertyValue(string name)
        {
            if (this.IsClassFiexedProperty(name) == true)
            {
                //return this.GetType().GetProperty(name).GetValue(this, null);
                return this.GetPropertyInfoByName(name).GetValue(this, null);
            }
            else
            {
                DynamicProperty dp = this.GetDynamicPropertyByName(name);
                if (dp == null)
                {
                    return null; //不包含返回null，还是抛出异常呢？？
                }
                else
                {
                    return dp.Value;
                }
            }


        }
        /// <summary>
        /// 获取属性值的类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type GetPropertyType(string name)
        {
            if (this.IsClassFiexedProperty(name) == true)
            {
                //return this.GetType().GetProperty(name).PropertyType;
                return this.GetPropertyInfoByName(name).PropertyType;
            }
            else
            {
                DynamicProperty dp = this.GetDynamicPropertyByName(name);
                if (dp == null)
                {
                    return null; //不包含返回null，还是抛出异常呢？？
                }
                else
                {
                    return dp.PropertyType;
                }
            }
        }

        /// <summary>
        /// 获取动态属性的名称列表
        /// </summary>
        /// <returns></returns>
        public string[] GetDynamicPropertyNames()
        {
            string[] result = null;

            if (this.PropertyList != null && this.PropertyList.Count > 0)
            {

                result = this.PropertyList.Select(p => p.Value.Name).ToArray();
            }
            return result;
        }

        /// <summary>
        /// 获取所有属性名称列表
        /// </summary>
        /// <returns></returns>
        public string[] GetAllPropertyNames()
        {
            List<string> result = new List<string>();

            result.AddRange(this.GetType().GetProperties().Select(p => p.Name).ToArray());

            string[] d = this.GetDynamicPropertyNames();
            if (d != null) { result.AddRange(d); }


            return result.ToArray();
        }


        /// <summary>
        /// 根据名称获取动态属性对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private DynamicProperty GetDynamicPropertyByName(string name)
        {
            if (this.PropertyList == null || this.PropertyList.Count == 0) { return null; }
            if (this.IsCaseSensitive == true)
            {
                if (this.PropertyList.ContainsKey(name) == true)
                {
                    return this.PropertyList[name];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                IEnumerable<DynamicProperty> v = this.PropertyList.Values.Where(k => k.Name.ToLower().Equals(name.ToLower()) == true);
                if (v.Count() == 0)
                {
                    return null;
                }
                else
                {
                    return v.ElementAt(0);
                }
            }

        }

        /// <summary>
        /// 是否类定义的固有属性（仅大小写不同，当作是相同的内容）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsClassFiexedProperty(string name)
        {
            bool result = true;

            if (this.GetPropertyInfoByName(name) == null)
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// 通过名称或取定义的属性信息（不区分大小写），不存在时返回Null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyInfo GetPropertyInfoByName(string name)
        {
            IEnumerable<PropertyInfo> ps = this.GetType().GetProperties().Where(p => p.Name.ToLower().Equals(name.ToLower()) == true);

            if (ps.Count() > 0)
            {
                return ps.ElementAt(0);
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// 看看属性值是否有效
        /// </summary>
        /// <param name="propertyValueType"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private bool IsValidValue(Type propertyValueType, object propertyValue)
        {
            if (propertyValueType == null) { return false; }

            bool result = true;

            try
            {
                if (propertyValueType.IsGenericType && propertyValueType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (propertyValue != null)
                    {
                        object x = Convert.ChangeType(propertyValue, propertyValueType.GetGenericArguments()[0]);
                    }
                }
                else
                {
                    if (propertyValue == null)
                    {
                        result = false;

                    }
                    else
                    {
                        object x = Convert.ChangeType(propertyValue, propertyValueType);
                    }
                }


            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }


        /// <summary>
        /// 内部动态属性列表
        /// </summary>
        private System.Collections.Generic.Dictionary<string, DynamicProperty> PropertyList;



        #region 嵌套类

        /// <summary>
        /// 一个动态属性
        /// </summary>
        private class DynamicProperty
        {
            /// <summary>
            /// 返回设置动态属性的名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 返回设置动态属性的值得类型
            /// </summary>
            public Type PropertyType { get; set; }

            /// <summary>
            /// 返回设置动态属性值
            /// </summary>
            public object Value { get; set; }
        }

        #endregion

    }
}
