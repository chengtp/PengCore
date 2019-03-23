using DDD.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Interfaces
{
    public interface IUserInfoService
    {
        Task<IEnumerable<UserInfoOutput>> GetModels();
    }
}
