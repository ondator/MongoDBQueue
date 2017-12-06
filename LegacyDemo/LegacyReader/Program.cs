﻿using System;
using System.Linq;
using System.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoQueue.Autofac;
using MongoQueue.Legacy;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LegacyReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string route = "listener";
            if (args.Any())
            {
                route = args[0];
            }

            var containerBuilder = new ContainerBuilder();
            IContainer container = null;

            var serviceProvider = new ServiceCollection
            {
                new ServiceDescriptor(
                    typeof(IContainer),
                    provider => container,
                    ServiceLifetime.Singleton)
            };

            containerBuilder.Populate(serviceProvider);
            new QueueBuilder()
                .AddAutofac<LegacyMessagingDependencyRegistrator>(containerBuilder)
                .AddHandler<DefaultHandler, DomainMessage>()
                .Build<ServiceProviderResolver>();
            container = containerBuilder.Build();
            var queue = container.Resolve<QueueProvider>();
            queue.Listen(route, CancellationToken.None).Wait();
            Console.WriteLine($"started listener {route}");
            Console.ReadLine();
        }
    }
}