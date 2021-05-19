using DDD.Util.Fault;
using DDD.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.ObjectMap
{
    /// <summary>
    /// object map manager
    /// </summary>
    public static class ObjectMapManager
    {
        static ObjectMapManager()
        {
            ObjectMapper = ContainerManager.Resolve<IObjectMap>();
        }

        /// <summary>
        /// Object Mapper
        /// </summary>
        public static IObjectMap ObjectMapper { get; set; }

        /// <summary>
        /// convert object
        /// </summary>
        /// <typeparam name="T">target data type</typeparam>
        /// <param name="sourceObj">source object</param>
        /// <returns>target data object</returns>
        public static T MapTo<T>(object sourceObj)
        {
            if (ObjectMapper == null)
            {
                throw new DDDException("ObjectMapManager.ObjectMapper is not initialized");
            }
            return ObjectMapper.MapTo<T>(sourceObj);
        }
    }
}
