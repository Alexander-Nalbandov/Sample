﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Milde.AggregateCache.InMemory;
using Milde.EventSourcing.Aggregates;
using Milde.EventSourcing.Cache;
using Milde.EventSourcing.Events;
using Milde.Redis.AggregateCache;
using Serilog;

namespace Milde.AggregateCache
{
    class AggregateCache<TAggregate> : IAggregateCache<TAggregate>
        where TAggregate : class, IAggregate
    {
        private readonly IRedisAggregateCache<TAggregate> _redisCache;
        private readonly IInMemoryAggregateCache<TAggregate> _inMemoryCache;

        private readonly ILogger _logger;


        public AggregateCache(
            IRedisAggregateCache<TAggregate> redisCache, 
            IInMemoryAggregateCache<TAggregate> inMemoryCache, 
            ILogger logger
        )
        {
            _redisCache = redisCache;
            _inMemoryCache = inMemoryCache;
            _logger = logger;
        }


        public async Task Initialize(IEventStore eventStore)
        {
            this._logger.Information("Started {AggregateType} cache initialization", typeof(TAggregate).Name);
            var timer = Stopwatch.StartNew();

            await this._redisCache.Initialize(eventStore);

            var aggregates = await this._redisCache.GetAllAggregates();
            this._inMemoryCache.PopulateCache(aggregates);

            timer.Stop();
            this._logger.Information("{AggregateType} cache initialization took {Elapsed} ms.", typeof(TAggregate).Name, timer.ElapsedMilliseconds);
        }


        public Task<IList<TAggregate>> GetAllAggregates()
        {
            return this._inMemoryCache.GetAllAggregates();
        }

        public Task<TAggregate> GetAggregate(Guid id)
        {
            return this._inMemoryCache.GetAggregate(id);
        }

        public Task<IList<TAggregate>> GetAggregates(Func<TAggregate, bool> predicate)
        {
            return this._inMemoryCache.GetAggregates(predicate);
        }

        public async Task SaveAggregate(TAggregate aggregate)
        {
            await this._redisCache.SaveAggregate(aggregate);
            await this._inMemoryCache.SaveAggregate(aggregate);
        }

        public async Task SaveAggregates(IList<TAggregate> aggregates)
        {
            await this._redisCache.SaveAggregates(aggregates);
            await this._inMemoryCache.SaveAggregates(aggregates);
        }
    }
}
