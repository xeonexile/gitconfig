using System;

namespace ExileLab.Extensions.Configuration
{
    internal class TimedEntry<T>
    {
        public T Value { get; }
        public DateTimeOffset ExpireAt { get; }

        public bool IsValid => ExpireAt.Ticks - DateTimeOffset.UtcNow.Ticks > 0;

        public TimedEntry(T v, DateTimeOffset expireAt)
        {
            Value = v;
            ExpireAt = expireAt;
        }
    }

    internal static class TimedEntry
    {
        public static TimedEntry<T> Create<T>(T v, TimeSpan ttl) =>
            new TimedEntry<T>(v, DateTimeOffset.UtcNow.Add(ttl));
    }
}