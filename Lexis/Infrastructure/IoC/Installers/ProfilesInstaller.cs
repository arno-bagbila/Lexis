using Autofac;
using AutoMapper;

namespace LexisApi.Infrastructure.IoC.Installers;

/// <summary>
/// Register AutoMapper profiles
/// </summary>
public class ProfilesInstaller : Module
{
    protected override void Load(ContainerBuilder builder)
    {

        var mapper = new MapperConfiguration(cfg =>
        {

            cfg.AddMaps(ThisAssembly);
            //cfg.ConstructServicesUsing(t => container.Resolve(t));
        }).CreateMapper();

        builder
            .RegisterInstance(mapper)
            .AsImplementedInterfaces();
    }
}