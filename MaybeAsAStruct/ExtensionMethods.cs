using System;
using System.Collections.Generic;
using System.Linq;

namespace MaybeAsAStruct
{
    public static class ExtensionMethods
    {
        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable.Where(predicate).FirstOrNone();
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (enumerable is IList<T> list)
            {
                if (list.Count > 0) return list[0];
            }
            else
            {
                using (IEnumerator<T> e = enumerable.GetEnumerator())
                {
                    if (e.MoveNext()) return e.Current;
                }
            }

            return new Maybe<T>();
        }

        public static Maybe<int> TryParseToInt(this string str)
        {
            if (int.TryParse(str, out var result))
                return result;

            return Maybe.None;
        }

        public static T[] ValueOrEmptyArray<T>(this Maybe<T[]> maybe)
        {
            return maybe.ValueOr(Array.Empty<T>());
        }

        public static string ValueOrEmptyString(this Maybe<string> maybe)
        {
            return maybe.ValueOr(string.Empty);
        }

        public static IEnumerable<T> GetItemsWithValue<T>(this IEnumerable<Maybe<T>> enumerable)
        {
            foreach (var maybe in enumerable)
            {
                if (maybe.TryGetValue(out var value))
                    yield return value;
            }
        }
    }
}