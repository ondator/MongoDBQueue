﻿using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoQueue.Autofac;
using MongoQueue.Legacy;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LegacyWriter
{
    class Program
    {
        static void Main()
        {
            DoStuff().Wait();
        }

        static async Task DoStuff()
        {
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
                .Build<ServiceProviderResolver>();
            container = containerBuilder.Build();
            var publisher = container.Resolve<QueueProvider>().GetPublisher();
            while (true)
            {
                try
                {
                    var rnd = new Random(DateTime.Now.Millisecond);
                    var id = rnd.Next();
                    var guid = Guid.NewGuid().ToString();
                    if (id % 2 == 0)
                    {
                        if (id % 4 == 0)
                        {
                            var message = new DomainMessage(guid, "exception");
                            await publisher.PublishAsync(message);
                        }
                        else
                        {
                            var message = new DomainMessage(guid, rnd.Next().ToString());
                            await publisher.PublishAsync(message);
                        }
                    }
                    else
                    {
                        var message = new AnotherDomainMessage(guid, rnd.Next().ToString(), "waddap indeed");
                        await publisher.PublishAsync(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    await Task.Delay(500);
                }
            }
        }
    }
}
