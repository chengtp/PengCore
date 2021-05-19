using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace DDD.Common
{
    /// <summary>
    /// NPOI导出帮助类
    /// </summary>

    public class NPOIHelper
    {

        /// <summary>
        /// xls 导入到集合对象中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        //public static List<T_xlsModel> Import_test1<T_xlsModel>(string fileName)
        //{
        //    List<T_xlsModel> list = new List<T_xlsModel>();
            

        //    using (var fs = File.OpenRead(fileName))
        //    {
        //        //把xls文件中的数据写入workbook1中
        //        var workbook1 = new HSSFWorkbook(fs);
        //        //获取第一个Sheet
        //        var sheet = workbook1.GetSheetAt(0);
        //        //获取第一行 标题行
        //        var row = sheet.GetRow(0);
        //        //声明字段数组
        //       // string[] fields = new string[row.LastCellNum];

               
        //        for (var j = 1; j <= sheet.LastRowNum; j++)
        //        {
        //            //读取当前行数据
        //            var dataRow = sheet.GetRow(j);
        //            // 创建对象实例
        //            T_xlsModel t = new T_xlsModel();
        //            if (dataRow != null)
        //            {
        //                for (var k = 0; k <= dataRow.LastCellNum; k++)
        //                {   //当前表格 当前单元格 的值
        //                    var cell = dataRow.GetCell(k);
        //                    if (cell != null)
        //                    {
                               
        //                        p.SetValue(t, GetValue(cell.ToString(), p));
        //                    }
        //                }
        //            }
        //            list.Add(t);
        //        }
        //    }
        //    return list;
        //}





        //将Excel中文件导入到C#项目
        /// <summary>
        /// 导入EXCEL文件
        /// </summary>
        /// <typeparam name="T">指定对应的实体类</typeparam>
        /// <param name="fileName">EXCEL文件名</param>
        /// <param name="dic">字段标题对应表</param>
        /// <returns>list集合</returns>
        public static List<T> Import<T>(string fileName, Dictionary<string, string> dic) where T : new()
        {
            List<T> list = new List<T>();
            Type tp = typeof(T);

            using (var fs = File.OpenRead(fileName))
            {
                //把xls文件中的数据写入workbook1中
                var workbook1 = new HSSFWorkbook(fs);
                //获取第一个Sheet
                var sheet = workbook1.GetSheetAt(0);
                //获取第一行 标题行
                var row = sheet.GetRow(0);
                //声明字段数组
                string[] fields = new string[row.LastCellNum];

                for (var i = 0; i < row.LastCellNum; i++)
                {
                    string title = row.GetCell(i).ToString();
                    fields[i] = dic[title];
                }
                for (var j = 1; j <= sheet.LastRowNum; j++)
                {
                    //读取当前行数据
                    var dataRow = sheet.GetRow(j);
                    // 创建对象实例
                    T t = new T();
                    if (dataRow != null)
                    {
                        for (var k = 0; k <= dataRow.LastCellNum; k++)
                        {   //当前表格 当前单元格 的值
                            var cell = dataRow.GetCell(k);
                            if (cell != null)
                            {
                                var p = tp.GetProperty(fields[k]);
                                p.SetValue(t, GetValue(cell.ToString(), p));
                            }
                        }
                    }
                    list.Add(t);
                }
            }
            return list;
        }
        private static object GetValue(string obj, PropertyInfo p)
        {
            object o = null;
            switch (p.PropertyType.Name)
            {
                case "Int16":
                    o = Int16.Parse(obj);
                    break;
                case "Int32":
                    o = Int32.Parse(obj);
                    break;
                case "Int64":
                    o = Int64.Parse(obj);
                    break;
                case "double":
                    o = double.Parse(obj);
                    break;
                case "float":
                    o = float.Parse(obj);
                    break;
                case "String":
                    o = obj.ToString();
                    break;
                case "bool":
                    o = bool.Parse(obj);
                    break;
                case "DateTime":
                    o = DateTime.Parse(obj);
                    break;
            }
            return o;
        }
        /// <summary>
        /// 导出内容到exls文件
        /// </summary>
        /// <typeparam name="T">对应的实体类</typeparam>
        /// <param name="fileName">Exal文件名</param>
        /// <param name="list"></param>
        /// <param name="dic"></param>
        public static void Export<T>(string fileName, List<T> list, Dictionary<string, string> dic)
        {
            Type tp = typeof(T); //获取类型
            var ps = tp.GetProperties(); //获取属性集合

            //创建工作薄
            var workbook = new HSSFWorkbook();
            //创建表
            var table = workbook.CreateSheet("sheet1");
            //创建表头
            var row = table.CreateRow(0);
            for (int i = 0; i < ps.Length; i++)
            {
                var cell = row.CreateCell(i);//创建单元格
                cell.SetCellValue(dic[ps[i].Name]);
            }

            //模拟20行20列数据
            for (var i = 1; i <= list.Count; i++)
            {
                //创建新行
                var dataRow = table.CreateRow(i);

                for (int j = 0; j < ps.Length; j++)
                {
                    var cell = dataRow.CreateCell(j);//创建单元格
                    cell.SetCellValue(ps[j].GetValue(list[i - 1]).ToString());
                }
            }
            //打开xls文件，如没有则创建，如存在则打开该文件
            using (var fs = File.OpenWrite(fileName))
            {
                workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                Console.WriteLine("生成成功");
            }
        }
    }
}