using System;
using System.IO;

namespace MaybeAsASumType
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void Test1()
        {
            var contents = GetLogContents(1);

            if (contents is Maybe<string>.Some some)
            {
                Console.WriteLine(some.Value);
            }
            else
            {
                Console.WriteLine("Log file not found");
            }
        }

        static void Test2()
        {
            var contents = GetLogContents(1);

            if (contents.TryGetValue(out var value))
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.WriteLine("Log file not found");
            }
        }

        static void Test3()
        {
            var contents = GetLogContents(1);

            contents.Match(some: value => 
            {
                Console.WriteLine(value);
            },
            none: () =>
            {
                Console.WriteLine("Log file not found");
            });
        }

        static Maybe<string> GetLogContents(int id)
        {
            var filename = "c:\\logs\\" + id + ".log";

            if (File.Exists(filename))
                return File.ReadAllText(filename);

            return Maybe.None;
        }
    }
}