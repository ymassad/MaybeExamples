using System;
using System.Collections.Generic;
using System.Text;

namespace MaybeAsASumType
{
    public abstract class Maybe<T>
    {
        private Maybe()
        {
        }

        public sealed class Some : Maybe<T>
        {
            public Some(T value) => Value = value;
            public T Value { get; }
        }

        public sealed class None : Maybe<T>
        {
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (this is Some s)
                return some(s.Value);

            return none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (this is Some s)
            {
                some(s.Value);
            }
            else
            {
                none();
            }
        }

        public static implicit operator Maybe<T>(T value)
        {
            if (value == null)
                return new None();

            return new Some(value);
        }

        public static implicit operator Maybe<T>(Maybe.MaybeNone value)
        {
            return new None();
        }

        public bool TryGetValue(out T value)
        {
            if (this is Some some)
            {
                value = some.Value;
                return true;
            }

            value = default(T);
            return false;
        }
    }

    public static class Maybe
    {
        public class MaybeNone
        {
        }

        public static MaybeNone None { get; } = new MaybeNone();

        public static Maybe<T> Some<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return new Maybe<T>.Some(value);
        }
    }
}