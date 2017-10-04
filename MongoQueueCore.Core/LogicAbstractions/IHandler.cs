﻿using System.Threading;
using System.Threading.Tasks;

namespace MongoQueue.Core.LogicAbstractions
{
    public interface IHandler<TMessage>
    {
        Task Handle(TMessage message, bool resend, CancellationToken cancellationToken);
    }

    public interface IHandler
    {
        Task Handle(string route, string messageId, object message, bool resend, CancellationToken cancellationToken);
    }
}