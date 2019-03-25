using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private DapperDBContext _context;
        private IUserInfoService iUserInfoService;
        private readonly AutoMapper.IMapper modelMapper;
        public HomeController(DapperDBContext context, IUserInfoService _iUserInfoService,
            AutoMapper.IMapper _modelMapper)
        {
            _context = context;
            iUserInfoService = _iUserInfoService;
            modelMapper = _modelMapper;
        }

        // GET: Index
        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            var models = await iUserInfoService.GetModels();
            return Ok(models);
        }

    }
}