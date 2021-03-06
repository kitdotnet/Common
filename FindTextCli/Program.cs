﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FindTextCli
{
    class Program
    {
        static HashSet<string> expressions = new HashSet<string>();
        static HashSet<string> extensions = new HashSet<string>();
        static ExpressionOperator expressionOperator = ExpressionOperator.And;
        static string directory = string.Empty;
        static bool caseInsensitive = false;
        static bool recursiveDirectorySearch = false;
        static bool showHelp = false, showLines = false, trimLines = false, showLineNumbers = false;
        static bool forceExpression = false;
        static List<Regex> regexCollection = new List<Regex>();
        static RegexOptions regexOptions = RegexOptions.Multiline;
        static DirectoryInfo dirInfo;
        static SearchOption searchOption;

        static void Main(string[] args)
        {
            int exitCode = -1;

            try
            {
                HandleArguments(args);

                if (showHelp)
                {
                    ShowHelp();
                    Environment.Exit(0);
                }

                ValidateConfig();
                Configure(out Func<string, List<string>> func);

                IEnumerable<FileInfo> files;

                if (extensions.Any())
                {
                    files = dirInfo.GetFiles("*", searchOption).Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));
                }
                else
                {
                    files = dirInfo.GetFiles("*", searchOption);
                }

                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        ProcessFile(file, func);
                    }
                }
                else
                {
                    Console.WriteLine("No files matched your extension configuration or no file were present to find.");
                }

                exitCode = 0;
            }
            catch (Exception exc)
            {
                if (exc is ArgumentException)
                {
                    exitCode = -2;
                    ShowHelp(exc.Message);
                }
                else
                {
                    exitCode = -3;
                    Console.WriteLine(exc.ToString());
                }
            }
            finally
            {
                Environment.Exit(exitCode);
            }
        }

        private static void ProcessFile(FileInfo file, Func<string, List<string>> func)
        {
            string text = File.ReadAllText(file.FullName);
            List<string> lines = func.Invoke(text);

            if (lines.Any())
            {
                Console.WriteLine($"{Environment.NewLine}{file.FullName}");

                if (showLineNumbers)
                {
                    ShowLinesWithLineNumbers(file, lines);
                }
                else if (showLines)
                {
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }

        private static void ShowLinesWithLineNumbers(FileInfo fileInfo, IEnumerable<string> matchedLines)
        {
            var lines = File.ReadAllLines(fileInfo.FullName);
            int matchedLineCounter = 0;
            int lineNumber = 0;
            foreach (string line in lines)
            {
                lineNumber++;
                if (matchedLines.Select(l => l.Trim()).Contains(line.Trim()))
                {
                    matchedLineCounter++;
                    string lineToShow = trimLines ? line.Trim() : line;
                    Console.WriteLine($"{lineNumber}\t{lineToShow}");

                    // once we've found all the expected matches, get out.
                    if (matchedLineCounter == matchedLines.Count()) { break; }
                }
            }
        }

        private static List<string> ProcessFileWithAndOperator(string text)
        {
            var lines = new List<string>();
            var regex = regexCollection.ElementAt(0);
            MatchCollection matches = regex.Matches(text);
            if (matches.Any())
            {
                foreach (Match match in matches)
                {
                    string line = match.Groups[0].Value;
                    for (int r = 1; r < regexCollection.Count(); r++)
                    {
                        if (!regexCollection.ElementAt(r).IsMatch(line))
                        {
                            line = null;
                            break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (trimLines) { line = line.Trim(); }
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }

        private static List<string> ProcessFileWithOrOperator(string text)
        {
            var lines = new List<string>();
            foreach (var regex in regexCollection)
            {
                MatchCollection matches = regex.Matches(text);
                if (matches.Any())
                {
                    foreach (Match match in matches)
                    {
                        string line = match.Groups[0].Value;
                        if (trimLines) { line = line.Trim(); }
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }

        private static void Configure(out Func<string, List<string>> func)
        {
            if (caseInsensitive) { regexOptions |= RegexOptions.IgnoreCase; }
            if (showLineNumbers) { showLines = true; }
            searchOption = recursiveDirectorySearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            dirInfo = new DirectoryInfo(directory);

            foreach (var pattern in expressions)
            {
                regexCollection.Add(new Regex(pattern, regexOptions));
            }

            if (expressionOperator == ExpressionOperator.And)
            {
                func = ProcessFileWithAndOperator;
            }
            else if (expressionOperator == ExpressionOperator.Or)
            {
                func = ProcessFileWithOrOperator;
            }
            else
            {
                throw new ArgumentException($"Logical Operator '{expressionOperator}' is not supported.");
            }
        }

        private static void ValidateConfig()
        {
            if (!expressions.Any())
            {
                throw new ArgumentException($"No expressions were provided.");
            }

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("No directory was provided");
            }

            if (!Directory.Exists(directory))
            {
                throw new ArgumentException($"{directory} does not exist.");
            }
        }

        private static void HandleArguments(string[] args)
        {
            for (int a = 0; a < args.Length; a++)
            {
                string argument = args[a].ToString();

                switch (argument)
                {
                    case "--help":
                    case "-h":
                    case "?":
                        showHelp = true;
                        break;
                    case "--directory":
                    case "--dir":
                    case "-d":
                        if (a > args.Length - 1) { throw new ArgumentException($"Expecting a directory after {args[a]}"); }
                        directory = args[++a];
                        break;
                    case "--expression":
                    case "-e":
                        if (a > args.Length - 1) { throw new ArgumentException($"Expecting an expression after {args[a]}"); }
                        AddExpression(args[++a]);
                        break;
                    case "--insensitive":
                    case "-i":
                        caseInsensitive = true;
                        break;
                    case "--extension":
                    case "-x":
                        if (a > args.Length - 1) { throw new ArgumentException($"Expecting an extension after {args[a]}"); }
                        AddExtension(args[++a]);
                        break;
                    case "--operator":
                    case "-o":
                        if (a > args.Length - 1) { throw new ArgumentException($"Expecting an operator after {args[a]}"); }
                        if (!Enum.TryParse<ExpressionOperator>(args[++a], true, out expressionOperator))
                        {
                            throw new ArgumentException($"{args[a]} is not a valid operator.");
                        }
                        break;
                    case "--recursive":
                    case "-r":
                        recursiveDirectorySearch = true;
                        break;
                    case "--show-lines":
                    case "-l":
                        showLines = true;
                        break;
                    case "--show-line-numbers":
                    case "-ln":
                        showLineNumbers = true;
                        break;
                    case "--trim":
                    case "-t":
                        trimLines = true;
                        break;
                    case "--force":
                    case "-f":
                        forceExpression = true;
                        break;
                    default:
                        throw new ArgumentException($"'{args[a]}' is an unknown argument.");
                }
            }
        }

        private static void AddExpression(string expression)
        {
            if (forceExpression || (expression.StartsWith("^") && expression.EndsWith("$")))
            {
                expressions.Add(expression);
            }
            else if (expression.StartsWith("^"))
            {
                AddExpression($"{expression}.+?$");
            }
            else if (expression.EndsWith("$"))
            {
                AddExpression($"^.+?{expression}");
            }
            else
            {
                AddExpression($"^.+?{expression}.+?$");
            }
        }

        private static void AddExtension(string extension)
        {
            while (extension.StartsWith("."))
            {
                extension = extension.Substring(1);
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                extensions.Add($".{extension}");
            }
        }

        private static void ShowHelp(string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }

            Dictionary<string, string> helpDefinitions = new Dictionary<string, string>()
            {
                { "--directory | --dir | -d <directory>","The directory to search."},
                { "--expression | -e <expression>", "A regular expression by which to search."},
                { "[--extension | -x <file extension>]", "Add file extension to extensions searched. When no extensions are provided, all files are searched." },
                { "[--operator | -o <And | Or>]",  "Use 'And' to combine expressions together and use 'Or' if any expression match is desired."},
                { "[--force | -f]","Force your expression to be accepted without manipulation."},
                { "[--insensitive | -i]", "Make search case insensitive." },
                { "[--recursive | -r]", "Make file searching include subdirectories." },
                { "[--show-lines | -l]", "Show the matching lines." },
                { "[--show-line-numbers | -ln]", "Show the line numbers for matching lines."},
                { "[--trim | -t]", "Trim the matching lines in the output."},
                { "[--help | -h | ?]","Show this help." }
            };

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            int maxKeyLength = helpDefinitions.Keys.Max(k => k.Length) + 1;

            Console.WriteLine($"{Environment.NewLine}{assemblyName} {string.Join(' ', helpDefinitions.Keys)}{Environment.NewLine}");

            foreach (var helpItem in helpDefinitions)
            {
                Console.WriteLine($"{helpItem.Key.PadRight(maxKeyLength)}\t{helpItem.Value}");
            }

            Console.WriteLine($"{Environment.NewLine}Examples:");
            Console.WriteLine($"{Environment.NewLine}Find any file containing the whole word 'the' - case-sensitive search:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\"");
            Console.WriteLine($"{Environment.NewLine}Same search, but case insensitive:");
            Console.WriteLine($"\t{assemblyName} - \"/c/repos\" -e \"\\bthe\\b\" -i");
            Console.WriteLine($"{Environment.NewLine}Same search, but case insensitive and searching subdirectories:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -i -r");
            Console.WriteLine($"{Environment.NewLine}Shows lines:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -i -r -l");
            Console.WriteLine($"{Environment.NewLine}Shows lines with line numbers:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -i -r -ln");
            Console.WriteLine($"equivalent to:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -i -r -ln -l");
            Console.WriteLine($"{Environment.NewLine}Trim the lines in the output:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -i -r -ln -t");
            Console.WriteLine($"{Environment.NewLine}Find any file containing the whole word 'the' AND the whole word 'best' - case-sensitive search:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -e \"\\bbest\\b\" -r -ln -t");
            Console.WriteLine($"equivalent to:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -e \"\\bbest\\b\" -r -ln -t -o And");
            Console.WriteLine($"{Environment.NewLine}Find any file containing the whole word 'the' OR the whole word 'best' - case-sensitive search:");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"\\bthe\\b\" -e \"\\bbest\\b\" -r -ln -t -o Or");
            Console.WriteLine($"{Environment.NewLine}Force your expression (to avoid full-line-capturing manipulation):");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"First Name: [a-zA-Z]+\" -r -ln -f");
            Console.WriteLine($"{Environment.NewLine}Same query, but find only first names like 'James' (case insensitive):");
            Console.WriteLine($"\t{assemblyName} -d \"/c/repos\" -e \"First Name\\s+?:\\s+?[a-zA-Z]+\" -e \"\\bJames\\b\" -i -r -ln -f");
        }
    }

    enum ExpressionOperator { And, Or }
}