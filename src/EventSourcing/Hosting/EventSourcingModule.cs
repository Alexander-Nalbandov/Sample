﻿using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Milde.EventSourcing.Serialization;

namespace Milde.EventSourcing.Hosting
{
    public static class EventSourcingModule
    {
        public static ContainerBuilder WithEventSourcing(
            this ContainerBuilder builder, 
            Action<IEventSourcingConfiguration> configuration,
            IConfiguration config
        )
        {
            builder.RegisterType<Serializer>().AsImplementedInterfaces().SingleInstance();

            var eventSourcingConfiguration = new EventSourcingConfiguration(builder, config);
            configuration(eventSourcingConfiguration);

            return builder;
        }
    }
}
