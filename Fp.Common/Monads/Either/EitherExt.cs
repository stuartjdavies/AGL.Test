using System;
using System.Collections.Generic;
using System.Text;

namespace Fp.Common.Monads.EitherMonad
{
    public static class EitherExt
    {
        public static TResult Match<TResult, TLeft, TRight>(this Either<TLeft, TRight> m,
                                                            Func<TLeft, TResult> onLeft,
                                                            Func<TRight, TResult> onRight)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    return onLeft(l.Value);
                case Right<TLeft, TRight> r:
                    return onRight(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static TRight GetRightValue<TLeft, TRight, TResult>(this Either<TLeft, TRight> m)
        { 
            switch (m)
            {
                case Right<TLeft, TRight> r:
                        return r.Value;
            };

            throw new System.Data.DataException("There is no right value");
        }

        public static IEnumerable<TResult> SelectLeft<TLeft, TRight, TResult>(this IEnumerable<Either<TLeft, TRight>> ms, Func<TLeft, TResult> f)
        {
            var xs = new List<TResult>();

            foreach (var m in ms)
            {
                switch (m)
                {
                    case Left<TLeft, TRight> l:
                        xs.Add(f(l.Value));
                        continue;
                };
            };

            return xs;
        }

        public static IEnumerable<TResult> SelectRight<TLeft, TRight, TResult>(this IEnumerable<Either<TLeft, TRight>> ms, Func<TRight, TResult> f)
        {
            var xs = new List<TResult>();

            foreach (var m in ms)
            {
                switch (m)
                {
                    case Right<TLeft, TRight> r:
                        xs.Add(f(r.Value));
                        continue;
                };
            };

            return xs;
        }

        public static bool IsRight<TLeft, TRight>(this Either<TLeft, TRight> m)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    return false;
                case Right<TLeft, TRight> r:
                    return true;
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static bool IsLeft<TLeft, TRight>(this Either<TLeft, TRight> m)
        => !IsRight(m);

        public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this TRight r)
        => Either<TLeft, TRight>.ReturnRight(r);


        public static Either<TLeft, TRight> ReturnRight<TLeft, TRight>(this TRight r)
        => Either<TLeft, TRight>.ReturnRight(r);

        public static Either<TLeft, TRight> ReturnLeft<TLeft, TRight>(this TLeft r)
        => Either<TLeft, TRight>.ReturnLeft(r);

        public static Either<TLeft, TRightTo> MapRight<TLeft, TRightFrom, TRightTo>(this Either<TLeft, TRightFrom> m, Func<TRightFrom, TRightTo> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return Either<TLeft, TRightTo>.ReturnLeft(l.Value);
                case Right<TLeft, TRightFrom> r:
                    return Either<TLeft, TRightTo>.ReturnRight(f(r.Value));
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static Either<TLeft, TRight1> LeftApplicative<TLeft, TRight1, TRight2>(this Either<TLeft, TRight1> m, Func<TRight1, Either<TLeft, TRight2>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight1> l:
                    return Either<TLeft, TRight1>.ReturnLeft(l.Value);
                case Right<TLeft, TRight1> r:
                    return f(r.Value).Match(failure => Either<TLeft, TRight1>.ReturnLeft(failure),
                                            success => m);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static Either<TLeft, TRight> OnLeft<TLeft, TRight>(this Either<TLeft, TRight> m, Action<TLeft> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    f(l.Value);
                    return m;
                case Right<TLeft, TRight> r:
                    return m;
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static Either<TLeft, TRight> OnRight<TLeft, TRight>(this Either<TLeft, TRight> m, Action<TRight> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    return m;
                case Right<TLeft, TRight> r:
                    f(r.Value);
                    return m;
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static Either<TLeftTo, TRight> MapLeft<TLeftFrom, TLeftTo, TRight>(this Either<TLeftFrom, TRight> m, Func<TLeftFrom, TLeftTo> f)
        {
            switch (m)
            {
                case Left<TLeftFrom, TRight> l:
                    return Either<TLeftTo, TRight>.ReturnLeft(f(l.Value));
                case Right<TLeftFrom, TRight> r:
                    return Either<TLeftTo, TRight>.ReturnRight(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static Either<TLeft, TRightTo> Bind<TLeft, TRightFrom, TRightTo>(this Either<TLeft, TRightFrom> m, Func<TRightFrom, Either<TLeft, TRightTo>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return Either<TLeft, TRightTo>.ReturnLeft(l.Value);
                case Right<TLeft, TRightFrom> r:
                    return f(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }
    }
}
