using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Module = Autofac.Module;

namespace LexisApi.Infrastructure.IoC.Installers;

public class MediatorInstaller : Module
{
    #region Overrides

    protected override void Load(ContainerBuilder builder)
    {

        builder
            .RegisterAssemblyTypes(typeof(IMediator).Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        RegisterHandlers(builder);

        var services = new ServiceCollection();
        builder.Populate(services);
    }

    #endregion

    #region Internals

    private void RegisterHandlers(ContainerBuilder builder)
    {

        var mediatrOpenTypes = new[] {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }

    #endregion
}