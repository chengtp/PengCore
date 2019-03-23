using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.IoC
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public class AutofacConfigHelper
    {
        public static void RegisterBasicType(ContainerBuilder builder)
        {
            //ioc 注册

            //builder.RegisterType<DDD.Repository.UnitOfWork.EFUnitOfWork>().As<DDD.Repository.UnitOfWork.IEFUnitOfWork>();
            //builder.RegisterType<DDD.Repository.Repositories.DepartmentRepository>().As<DDD.Domain.IRepositories.IDepartmentRepository>();
            //builder.RegisterType<DDD.Repository.Repositories.UserRepository>().As<DDD.Domain.IRepositories.IUserRepository>();
            //builder.RegisterType<DDD.Repository.Repositories.MenuRepository>().As<DDD.Domain.IRepositories.IMenuRepository>();
            //builder.RegisterType<DDD.Repository.Repositories.RoleRepository>().As<DDD.Domain.IRepositories.IRoleRepository>();

            //builder.RegisterType<MenuRepository>().As<IMenuRepository>();
            //builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>();
            //builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            //builder.RegisterType<UserRepository>().As<IUserRepository>();

        }
    }
}
