using Microsoft.Extensions.DependencyInjection;

namespace DDD.Infrastructure.IoC
{
    public class DefaultConfigHelper
    {
        public static void RegisterBasicType(IServiceCollection services)
        {

            #region Domain  领域层

            //services.AddTransient<IUserInfoRepository, UserInfoRepository>();
            services.AddTransient(typeof(DDD.Repository.Interfaces.BaseInterfaces.IRepository<>), typeof(Repository.BaseRepository<>));
            services.AddTransient<Interfaces.IUnitOfWorkFactory, DDD.Infrastructure.DapperUnitOfWorkFactory>();

            services.AddTransient<DDD.Repository.Interfaces.IDemoRepository, Repository.DemoRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IUserInfoRepository, Repository.UserInfoRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IDeptRepository, Repository.DeptRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IMenuRepository, Repository.MenuRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IPostRepository, Repository.PostRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IPostTypeRepository, Repository.PostTypeRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IRoleRepository, Repository.RoleRepository>();

            #endregion

            #region  Applicaton  应用层

            services.AddTransient<Application.Interfaces.IDemoService, Application.Services.DemoService>();
            services.AddTransient<Application.Interfaces.IUserInfoService, Application.Services.UserInfoService>();
            services.AddTransient<Application.Interfaces.IDeptService, Application.Services.DeptService>();
            services.AddTransient<Application.Interfaces.IMenuService, Application.Services.MenuService>();
            services.AddTransient<Application.Interfaces.IPostService, Application.Services.PostService>();
            services.AddTransient<Application.Interfaces.IPostTypeService, Application.Services.PostTypeService>();
            services.AddTransient<Application.Interfaces.IRoleService, Application.Services.RoleService>();

            #endregion
        }
    }
}
