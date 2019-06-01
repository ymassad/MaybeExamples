using System;

namespace MaybeAsAStruct
{
    public struct Maybe<T>
    {
        private readonly T value;

        private readonly bool hasValue;

        private Maybe(T value)
        {
            this.value = value;
            hasValue = true;
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (hasValue)
                return some(value);

            return none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (hasValue)
            {
                some(value);
            }
            else
            {
                none();
            }
        }

        public static implicit operator Maybe<T>(T value)
        {
            if(value == null)
                return new Maybe<T>();

            return new Maybe<T>(value);
        }

        public static implicit operator Maybe<T>(Maybe.MaybeNone value)
        {
            return new Maybe<T>();
        }

        public bool TryGetValue(out T value)
        {
            if (hasValue)
            {
                value = this.value;
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
    }
}