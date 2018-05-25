using IocExample.Classes;
using Ninject;
using Ninject.Modules;
using System;

namespace IocExample
{
    class Program
    {

        //-------------------------original

        /*static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var sqlConnectionFactory = new SqlConnectionFactory("SQL Connection", logger);
            var createUserHandler = new CreateUserHandler(new UserService(new QueryExecutor(sqlConnectionFactory), new CommandExecutor(sqlConnectionFactory), new CacheService(logger, new RestClient("API KEY"))), logger);

            createUserHandler.Handle();
        }*/


        //------------------------ninject

        /*static void Main(string[] args)
        {

            var ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ILogger>().To<ConsoleLogger>();
            ninjectKernel.Bind<IConnectionFactory>().To<SqlConnectionFactory>().InSingletonScope().WithConstructorArgument("sqlConnection", "Hello");
            ninjectKernel.Bind<RestClient>().ToSelf().WithConstructorArgument("apiKey", "API KEY");

            var createUserHandler = ninjectKernel.Get<CreateUserHandler>();
            createUserHandler.Handle();

        }*/

        //--------------------custom resolver

        static void Main(string[] args)
        {
            var customResolver = new CustomResolver();

            customResolver.Bind<ILogger, ConsoleLogger>();
            customResolver.BindToObject<IConnectionFactory>(new SqlConnectionFactory("Hello", customResolver.Get<ILogger>()));
            customResolver.BindToObject<RestClient>(new RestClient("API_KEY"));

            var createUserHandler = customResolver.Get<CreateUserHandler>();
            createUserHandler.Handle();
        }
    }
}
