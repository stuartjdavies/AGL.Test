using System;
using System.Collections.Generic;
using System.Text;

namespace Fp.Common
{
    public static class Fp
    {
        public static B Pipe<A, B>(this A v, Func<A, B> f)
        => f(v);

        public static void ForEach<T>(this IEnumerable<T> xs, Action<T> f)
        { foreach (var x in xs) f(x); }

        public static Func<A, C> Compose<A, B, C>(Func<A, B> f, Func<B, C> g)
        => (x => g(f(x)));

        public static Func<A, Func<B, C>> Curry<A, B, C>(Func<A, B, C> f)
        => a => b => f(a, b);

        public static T Id<T>(T v) => v;
    }
}
