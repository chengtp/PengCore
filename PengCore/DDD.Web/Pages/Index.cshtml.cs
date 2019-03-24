using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DDD.Web.Pages
{
    public class IndexModel : PageModel
    {
        private DapperDBContext _context;
        private IUserInfoService iUserInfoService;
        private readonly AutoMapper.IMapper modelMapper;
        public IndexModel(DapperDBContext context, IUserInfoService _iUserInfoService,
            AutoMapper.IMapper _modelMapper)
        {
            _context = context;
            iUserInfoService = _iUserInfoService;
            AutoMapper.IMapper modelMapper = _modelMapper;
        }

        public async Task OnGet()
        {
            var models = await iUserInfoService.GetModels();
           
        }
    }
}
