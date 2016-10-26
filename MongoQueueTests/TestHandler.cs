﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MongoQueue.Core;
using MongoQueue.Core.Read;

namespace MongoQueueTests
{
    public class TestHandler : MessageHandlerBase<TestMessage>
    {
        public override async Task Handle(TestMessage message, bool resend, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} {message.Id} {message.Name}  {resend}");
            ResultHolder.Add(message.Id, message.Name);
        }
    }
}