using System;
using System.IO;

namespace MaybeAsAStruct
{
    class Program
    {
        static void Main(string[] args)
        {

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

        static void Test4()
        {
            var errorDescriptionMaybe =
                GetLogContents(13)
                    .Bind(contents => FindErrorCode(contents))
                    .Bind(errorCode => GetErrorDescription(errorCode));
        }

        static void Test5()
        {
            var errorDescriptionMaybe =
                GetLogContents(13)
                    .Bind(contents => FindErrorCode(contents)
                        .Bind(errorCode => GetErrorDescription(errorCode, contents)));
        }

        static void Test6()
        {
            var errorDescriptionMaybe =
                from contents in GetLogContents(13)
                from errorCode in FindErrorCode(contents)
                from errorDescription in GetErrorDescription(errorCode, contents)
                select errorDescription;
        }

        static void Test7()
        {
            var errorDescriptionMaybe =
                from contents in GetLogContents(13)
                from errorCode in FindErrorCode(contents)
                where errorCode < 1000
                from errorDescription in GetErrorDescription(errorCode, contents)
                select errorDescription;
        }

        static Maybe<string> GetLogContents(int id)
        {
            var filename = "c:\\logs\\" + id + ".log";

            if (File.Exists(filename))
                return File.ReadAllText(filename);

            return Maybe.None;
        }

        static Maybe<int> FindErrorCode(string logContents)
        {
            var logLines = logContents.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            return
                logLines
                    .FirstOrNone(x => x.StartsWith("Error code: "))
                    .Map(x => x.Substring("Error code: ".Length))
                    .Bind(x => x.TryParseToInt());
        }

        static Maybe<string> GetErrorDescription(int errorCode)
        {
            var filename = "c:\\errorCodes\\" + errorCode + ".txt";

            if (File.Exists(filename))
                return File.ReadAllText(filename);

            return Maybe.None;
        }

        static Maybe<string> GetErrorDescription(int errorCode, string logContents)
        {
            var logLines = logContents.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var linePrefix = "Error description for code " + errorCode + ": ";

            return
                logLines
                    .FirstOrNone(x => x.StartsWith(linePrefix))
                    .Map(x => x.Substring(linePrefix.Length));
        }
    }
}
