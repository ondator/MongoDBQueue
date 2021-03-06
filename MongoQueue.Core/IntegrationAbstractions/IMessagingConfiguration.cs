﻿using System;

namespace MongoQueue.Core.IntegrationAbstractions
{
    public interface IMessagingConfiguration
    {
        string ConnectionString { get; }
        string Database { get; }
        TimeSpan ResendInterval { get; }
        TimeSpan ResendThreshold { get; }
        int MaxResendsThreshold { get; }
        CursorType CursorType { get; }

    }
}