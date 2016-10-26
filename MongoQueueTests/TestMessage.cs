﻿namespace MongoQueueTests
{
    public class TestMessage
    {
        public string Id { get; }
        public string Name { get; }
        public TestValueObject TestValueObject { get; }

        public TestMessage(string id, string name, TestValueObject testValueObject)
        {
            Id = id;
            Name = name;
            TestValueObject = testValueObject;
        }
    }
}