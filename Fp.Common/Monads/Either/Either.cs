using System;
using System.Collections.Generic;
using System.Text;

namespace Fp.Common.Monads.EitherMonad
{
    public abstract class Either<TLeft, TRight>
    {
        public static Either<TLeft, TRight> ReturnLeft(TLeft l)
        => new Left<TLeft, TRight>(l);

        public static Either<TLeft, TRight> ReturnRight(TRight r)
        => new Right<TLeft, TRight>(r);
    }

    public class Left<TLeft, TRight> : Either<TLeft, TRight>
    {
        public TLeft Value { get; private set; }
        public Left(TLeft l) => Value = l;
    }

    public class Right<TLeft, TRight> : Either<TLeft, TRight>
    {
        public TRight Value { get; private set; }
        public Right(TRight r) => Value = r;
    }
}
