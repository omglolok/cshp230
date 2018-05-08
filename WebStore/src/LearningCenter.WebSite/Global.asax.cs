using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
namespace LearningCenter.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterAutofac();
        }

        private void RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            RegisterAssemblyTypes(builder, Assembly.GetExecutingAssembly());
            var container = builder.Build();

            // configure depenedency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


        }

        private void RegisterAssemblyTypes(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly).AsSelf().AsImplementedInterfaces();

            var assemblyNames = assembly.GetReferencedAssemblies();

            foreach (var assemblyName in assemblyNames)
            {
                if (assemblyName.FullName.ToLower().Contains("learningcenter"))
                {
                    assembly = Assembly.Load(assemblyName);
                    RegisterAssemblyTypes(builder, assembly);
                }
            }
        }
    }
}
