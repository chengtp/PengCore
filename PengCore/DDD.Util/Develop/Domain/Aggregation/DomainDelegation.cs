using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Domain.Aggregation
{
    /// <summary>
    /// data change delegate
    /// </summary>
    /// <typeparam name="T">DataType</typeparam>
    /// <param name="datas">datas</param>
    public delegate void DataChange<T>(params T[] datas);

    /// <summary>
    /// query data delegate
    /// </summary>
    /// <param name="datas">datas</param>
    /// <returns></returns>
    public delegate void QueryData<T>(ref IEnumerable<T> datas);
}
