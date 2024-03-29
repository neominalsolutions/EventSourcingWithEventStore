﻿using EventSourcing_Example_With_EventStore.EventTypes.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Aggregates.Abstractions
{
    public abstract class StreamAggregate
    {
        //Oluşan tüm event'leri tutacak koleksiyon.
        protected readonly List<IEvent> events = new();
        public List<IEvent> GetEvents => events;
        //Event'lerin tutulacağı Aggregate/Stream adı.
        public string StreamName { get; private set; } 

        // userStreams
        public void SetStreamName(string streamName)
            => StreamName = streamName;
        //Stream adının atanıp atanmadığını kontrol eden fonksiyon.
        protected bool CheckStreamName()
            => string.IsNullOrEmpty(StreamName) || string.IsNullOrWhiteSpace(StreamName);
    }
}
