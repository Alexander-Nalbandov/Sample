using System;
using Autofac;
using Sample.Infrastructure.Remoting.RabbitMq.Registration;

namespace Sample.Infrastructure.Remoting.Rabbit.Registration
{
    public static class ContainerExtensions
    {
        public static ContainerBuilder WithRabbitRemoting(
            this ContainerBuilder builder, Action<ServiceConfigurator> action)
        {
            builder.RegisterModule<RabbitModule>();
            var configurator = new ServiceConfigurator(builder);
            action(configurator);

            return builder;
        }
    }
}