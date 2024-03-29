﻿using Dapper;
using Dapper.Contrib.Extensions;
using DDD.Infrastructure.Dtos;
using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using DDD.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 用户仓储数据
    /// </summary>
    public class UserInfoRepository : BaseRepository<Sys_UserInfo>, IUserInfoRepository
    {
        private DapperDBContext _context;
        public UserInfoRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }


        public void Etesttt()
        {
            DapperUnitOfWorkFactory tttt = new DapperUnitOfWorkFactory(_context);
            IUnitOfWork unitOfWork = tttt.BeginTransaction();

            unitOfWork.Commit();
            unitOfWork.Rollback();




            _context.BeginTransaction();

            _context.Commit();
            _context.Rollback();


            _context._connection.DeleteAllAsync<string>(null, null);

        }

        public Task<UserInfoLoginOut> GetModelById()
        {
            throw new NotImplementedException();
        }

        #region 用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserInfoLoginOut> GetModelByLogin(UserInfoLoginInput model)
        {
            DynamicParameters dparams = new DynamicParameters();
            dparams.Add("LoginName", model.LoginName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            dparams.Add("Password", model.Password, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            return await _context._connection.QueryFirstOrDefault(DDD.Infrastructure.SqlMaps.UserSql.UserInfoLogin(model.LoginName, model.Password), dparams);

        }

        #endregion

        #region 查询所有用户集合信息

        /// <summary>
        /// 查询所有用户集合信息
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<Sys_UserInfo>> GetModels()
        {
            PagedRequest request = new PagedRequest()
            {
                 WhereString=" 1=1"
            };
            var list = await QueryPageAsync(request);

            return list;
            //  return await _context._connection.GetAllAsync<Sys_UserInfo>();
        }

        #endregion



        public Task<bool> CreateModel()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<UserInfoLoginOut>> GetModelsBy()
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateModel()
        {
            throw new NotImplementedException();
        }


    }
}
