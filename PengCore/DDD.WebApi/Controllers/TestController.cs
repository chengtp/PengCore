using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.HSSF.UserModel;

namespace DDD.WebApi.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private DapperDBContext context;    //已注入默认数据库，事务操作
        private IDemoService iDemoService;  //应用层:数据操作
        private readonly AutoMapper.IMapper modelMapper; //Dto映射
        private ILog Log;  //日志文件
        private readonly IOptions<Infrastructure.Dtos.Config.AppSettings> appSettings; //配置文件数据
        /// <summary>
        /// DemoController
        /// </summary>
        public TestController(DapperDBContext _context
            , IDemoService _iDemoService,
            AutoMapper.IMapper _modelMapper, IHostingEnvironment hostingEnv
            , IOptions<Infrastructure.Dtos.Config.AppSettings> _appSettings)
        {
            context = _context;
            iDemoService = _iDemoService;
            modelMapper = _modelMapper;
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(HomeController));
            appSettings = _appSettings;
        }



        /// <summary>
        /// 2021年5月19号，帮助匡整理xlx数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetXlsx")]
        public ActionResult GetXlsx()
        {


            List<T_xlsModel> list = new List<T_xlsModel>();

            string fileName = @"D:\1.xls";

            using (var fs = System.IO.File.OpenRead(fileName))
            {
                //把xls文件中的数据写入workbook1中
                var workbook1 = new HSSFWorkbook(fs);
                //获取第一个Sheet
                IList<NPOI.SS.UserModel.IName> listName = workbook1.GetAllNames();

                foreach (NPOI.SS.UserModel.IName item in listName)
                {
                    string SheetName = item.SheetName;
                    string firestSheetName = SheetName.Substring(0, 1);
                    //验证字符串是否是数字
                    Regex r = new Regex(@"^[+-]?\d*(,\d{3})*(\.\d+)?$");
                    if (r.IsMatch(firestSheetName))
                    {
                        var sheet = workbook1.GetSheet(SheetName);
                        //获取第一行 标题行
                        var row = sheet.GetRow(0);
                        //声明字段数组
                        // string[] fields = new string[row.LastCellNum];


                        for (var j = 3; j <= sheet.LastRowNum; j++)
                        {
                            //读取当前行数据
                            var dataRow = sheet.GetRow(j);

                            if (dataRow != null)
                            {
                                if (dataRow.GetCell(0) != null && dataRow.GetCell(1) != null && !string.IsNullOrWhiteSpace(dataRow.GetCell(1).ToString()))
                                {
                                    // 创建对象实例
                                    T_xlsModel model = new T_xlsModel();

                                    //当前表格 当前单元格 的值
                                    var cell0 = j - 3;
                                    var cell1 = dataRow.GetCell(1).ToString();
                                    var cell2 = dataRow.GetCell(2).ToString();
                                    var cell3 = dataRow.GetCell(3).ToString();
                                    var cell4 = dataRow.GetCell(4).ToString();
                                    var cell5 = dataRow.GetCell(5).ToString();
                                    var cell6 = dataRow.GetCell(6).ToString();


                                    model.No = cell0;
                                    model.Client = cell1;
                                    model.Matricule = cell2;
                                    model.Bordereau = cell3;
                                    model.Nature = cell4;
                                    model.Destination = cell5;
                                    model.Litre = cell6;
                                    model.DateString = SheetName;

                                    list.Add(model);

                                }
                            }

                        }

                    }


                }
            }





            if (list != null && list.Any())
            {
                //导入到xls中
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("No", "No");
                dic.Add("Client", "Client");
                dic.Add("Matricule", "Matricule");
                dic.Add("Bordereau", "Bordereau");
                dic.Add("Nature", "Nature");
                dic.Add("Destination", "Destination");
                dic.Add("Litre", "Litre");
                dic.Add("DateString", "DateString");

                DDD.Common.NPOIHelper.Export<T_xlsModel>(@"D:\2.xls", list, dic);

            }

            return Ok();
        }


        /// <summary>
        /// test1
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("test1")]
        public IActionResult test1()
        {

            return Ok("test1");
        }

        /// <summary>
        /// test2
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("test2")]
        public IActionResult test2()
        {
            return Ok("test2");
        }
    }

    /// <summary>
    /// 测试model
    /// </summary>
    public class T_xlsModel
    {

        /// <summary>
        /// 序号
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// Client
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// Matricule
        /// </summary>
        public string Matricule { get; set; }
        /// <summary>
        /// bordereau
        /// </summary>
        public string Bordereau { get; set; }
        /// <summary>
        /// Nature
        /// </summary>
        public string Nature { get; set; }
        /// <summary>
        /// Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// litre
        /// </summary>
        public string Litre { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DateString { get; set; }

    }
}
