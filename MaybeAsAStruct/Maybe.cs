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

        public Maybe<TResult> Map<TResult>(Func<T, TResult> convert)
        {
            if(!hasValue)
                return new Maybe<TResult>();

            return convert(value);
        }

        public Maybe<TResult> Select<TResult>(Func<T, TResult> convert)
        {
            if (!hasValue)
                return new Maybe<TResult>();

            return convert(value);
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> convert)
        {
            if (!hasValue)
                return new Maybe<TResult>();

            return convert(value);
        }

        public Maybe<TFinalResult> SelectMany<TResult, TFinalResult>(
            Func<T, Maybe<TResult>> convert,
            Func<T, TResult, TFinalResult> finalSelect)
        {
            if (!hasValue)
                return new Maybe<TFinalResult>();

            var converted = convert(value);

            if (!converted.hasValue)
                return new Maybe<TFinalResult>();

            return finalSelect(value, converted.value);
        }

        public Maybe<T> Where(Func<T, bool> predicate)
        {
            if (!hasValue)
                return new Maybe<T>();

            if (predicate(value))
                return this;

            return new Maybe<T>();
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

            return value;
        }
    }
}