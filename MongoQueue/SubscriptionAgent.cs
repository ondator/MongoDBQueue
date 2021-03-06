﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoQueue.Core;
using MongoQueue.Core.AgentAbstractions;
using MongoQueue.Core.Entities;

namespace MongoQueue
{
    public class SubscriptionAgent : ISubscriptionAgent
    {
        private readonly MongoAgent _mongoAgent;

        public SubscriptionAgent(MongoAgent mongoAgent)
        {
            _mongoAgent = mongoAgent;
        }

        public List<Subscriber> GetSubscribers(string topic)
        {
            var subscribersCollection = _mongoAgent.GetSubscribers();
            var subscribers =
                subscribersCollection.Find(Builders<Subscriber>.Filter.All(x => x.Topics, new[] { topic })).ToList();
            return subscribers;
        }

        public async Task<List<Subscriber>> GetSubscribersAsync(string topic)
        {
            var subscribersCollection = _mongoAgent.GetSubscribers();
            var subscribers =
                await
                    (await
                            subscribersCollection.FindAsync(Builders<Subscriber>.Filter.All(x => x.Topics, new[] { topic })))
                        .ToListAsync();
            return subscribers;
        }

        public async Task UpdateSubscriber(string route, string[] topics)
        {
            var subscriber = new Subscriber(route, topics);
            var subscribersCollection = _mongoAgent.GetSubscribers();
            var nameFilter = Builders<Subscriber>.Filter.Eq(x => x.Name, route);
            var existingSubscriber = await (await subscribersCollection.FindAsync(nameFilter)).FirstOrDefaultAsync();
            if (existingSubscriber != null)
            {
                subscriber.Id = existingSubscriber.Id;
                await subscribersCollection.ReplaceOneAsync(nameFilter, subscriber);
            }
            else
            {
                await subscribersCollection.InsertOneAsync(subscriber);
            }
        }
    }
}