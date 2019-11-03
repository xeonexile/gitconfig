using System;
using Microsoft.Extensions.Options;

namespace ExileLab.Extensions.Configuration
{
    public static class TimedOptions
    {
        public static TimedOptions<TOptions> Create<TOptions>(Func<TOptions> s) where TOptions : class, new() =>
            new TimedOptions<TOptions>(s);
    }

    public class TimedOptions<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        private readonly Func<TOptions> _selector;

        public TimedOptions(Func<TOptions> selector)
        {
            _selector = selector;
        }
        public TOptions Value => _selector();
    }
}