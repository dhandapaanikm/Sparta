using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Ninject.Selection.Heuristics;
using Sparta.Core;
using Sparta.Core.Interfaces;
using Sparta.Core.Interfaces.ApplicationServices;
using Sparta.Core.Interfaces.Repositories;
using Sparta.Core.Interfaces.Services;
using Sparta.Core.Services;
using Sparta.Infrastructure;
using Sparta.Infrastructure.Encryption;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Sparta.API._2.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Sparta.API._2.App_Start.NinjectWebCommon), "Stop")]

namespace Sparta.API._2.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Components.Add<IInjectionHeuristic, CustomInjectionHeuristic>();

            kernel.Bind<IEncryptionClient>().To<EncryptionClient>();

            //Core Domain layer DI...
            kernel.Bind<ICountryService>().To<CountryService>().InRequestScope();//.WithConstructorArgument("userId", x => GetUserId());

            //Infrastructure.Data layer DI...

            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));

            kernel.Bind<IDataContext>().To<SpartaContext>().InRequestScope();
            kernel.Bind<IUnitOfWorkForService>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            var connectionString = ConfigurationManager.ConnectionStrings["SpartaContext"].ConnectionString;
            kernel.Bind<DbContext>().ToSelf().WithConstructorArgument("nameOrConnectionString", connectionString);

        }

        //private static string GetUserId()
        //{
        //    if (HttpContext.Current == null || HttpContext.Current.User == null)
        //    {
        //        return string.Empty;
        //    }

        //    return HttpContext.Current.User.Identity.Name;
        //}
    }
}
