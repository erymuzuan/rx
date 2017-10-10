using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Mangements
{
    public static class InteractivePrompt
    {
        private static string m_prompt;
        private static int m_startingCursorLeft;
        private static int m_startingCursorTop;
        private static ConsoleKeyInfo m_key, m_lastKey;

        private static bool InputIsOnNewLine(int inputPosition)
        {
            return (inputPosition + m_prompt.Length > Console.BufferWidth - 1);
        }
        private static int GetCurrentLineForInput(List<char> input, int inputPosition)
        {
            var currentLine = 0;
            for (var i = 0; i < input.Count; i++)
            {
                if (input[i] == '\n')
                    currentLine += 1;
                if (i == inputPosition)
                    break;
            }
            return currentLine;
        }
        /// <summary>
        /// Gets the cursor position relative to the current line it is on
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputPosition"></param>
        /// <returns></returns>
        private static Tuple<int, int> GetCursorRelativePosition(List<char> input, int inputPosition)
        {
            var currentPos = 0;
            var currentLine = 0;
            for (var i = 0; i < input.Count; i++)
            {
                if (input[i] == '\n')
                {
                    currentLine += 1;
                    currentPos = 0;
                }
                if (i == inputPosition)
                {
                    if (currentLine == 0)
                    {
                        currentPos += m_prompt.Length;
                    }
                    break;
                }
                currentPos++;
            }
            return Tuple.Create(currentPos, currentLine);
        }
        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
        private static void ClearLine(List<char> input, int inputPosition)
        {
            var cursorLeft = InputIsOnNewLine(inputPosition) ? 0 : m_prompt.Length;
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
            Console.Write(new string(' ', input.Count + 5));
        }

        /// <summary>
        /// A hacktastic way to scroll the buffer - WriteLine
        /// </summary>
        /// <param name="lines"></param>
        private static void ScrollBuffer(int lines = 0)
        {
            for (var i = 0; i <= lines; i++)
                Console.WriteLine("");
            Console.SetCursorPosition(0, Console.CursorTop - lines);
            m_startingCursorTop = Console.CursorTop - lines;
        }

        /// <summary>
        /// RewriteLine will rewrite every character in the input List, and given the inputPosition
        /// will determine whether or not to continue to the next line
        /// </summary>
        /// <param name="input">The input buffer</param>
        /// <param name="inputPosition">Current character position in the List</param>
        private static void RewriteLine(List<char> input, int inputPosition)
        {
            try
            {
                Console.SetCursorPosition(m_startingCursorLeft, m_startingCursorTop);
                var coords = GetCursorRelativePosition(input, inputPosition);
                var cursorTop = m_startingCursorTop;
                int cursorLeft;

                if (GetCurrentLineForInput(input, inputPosition) == 0)
                {
                    cursorTop += (inputPosition + m_prompt.Length) / Console.BufferWidth;
                    cursorLeft = inputPosition + m_prompt.Length;
                }
                else
                {
                    cursorTop += coords.Item2;
                    cursorLeft = coords.Item1 - 1;
                }

                // if the new vertical cursor position is going to exceed the buffer height (i.e., we are
                // at the bottom of console) then we need to scroll the buffer however much we are about to exceed by
                if (cursorTop >= Console.BufferHeight)
                {
                    ScrollBuffer(cursorTop - Console.BufferHeight + 1);
                    RewriteLine(input, inputPosition);
                    return;
                }

                Console.Write(string.Concat(input));
                Console.SetCursorPosition(Mod(cursorLeft, Console.BufferWidth), cursorTop);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static IEnumerable<string> GetMatch(List<string> s, string input)
        {
            s.Add(input);
            for (var i = -1; i < s.Count;)
            {
                var direction = (m_key.Modifiers == ConsoleModifiers.Shift) ? -1 : 1;
                i = Mod((i + direction), s.Count);

                if (Regex.IsMatch(s[i], ".*(?:" + input + ").*", RegexOptions.IgnoreCase))
                {
                    yield return s[i];
                }
            }
        }

        static Tuple<int, int> HandleMoveLeft(List<char> input, int inputPosition)
        {
            var coords = GetCursorRelativePosition(input, inputPosition);
            var cursorLeftPosition = coords.Item1;
            var cursorTopPosition = Console.CursorTop;

            if (GetCurrentLineForInput(input, inputPosition) == 0)
                cursorLeftPosition = (coords.Item1) % Console.BufferWidth;

            if (Console.CursorLeft == 0)
                cursorTopPosition = Console.CursorTop - 1;

            return Tuple.Create(cursorLeftPosition, cursorTopPosition);
        }

        static Tuple<int, int> HandleMoveRight(List<char> input, int inputPosition)
        {
            var coords = GetCursorRelativePosition(input, inputPosition);
            var cursorLeftPosition = coords.Item1;
            var cursorTopPosition = Console.CursorTop;
            if (Console.CursorLeft + 1 >= Console.BufferWidth || input[inputPosition] == '\n')
            {
                cursorLeftPosition = 0;
                cursorTopPosition = Console.CursorTop + 1;
            }
            return Tuple.Create(cursorLeftPosition % Console.BufferWidth, cursorTopPosition);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) { action(item); }
        }

        /// <summary>
        /// Run will start an interactive prompt
        /// </summary>
        /// <param name="lambda">This func is provided for the user to handle the input.  Input is provided in both string and List&lt;char&gt;. A return response is provided as a string.</param>
        /// <param name="prompt">The prompt for the interactive shell</param>
        /// <param name="startupMsg">Startup msg to display to user</param>
        /// <param name="completionList"></param>
        public static void Run(Func<string, List<char>, List<string>, string> lambda, string prompt, string startupMsg, List<string> completionList = null)
        {
            m_prompt = prompt;
            Console.WriteLine(startupMsg);
            var inputHistory = new List<List<char>>();
            IEnumerator<string> wordIterator = null;

            while (true)
            {
                string completion = null;
                var input = new List<char>();
                m_startingCursorLeft = m_prompt.Length;
                m_startingCursorTop = Console.CursorTop;
                var inputPosition = 0;
                var inputHistoryPosition = inputHistory.Count;

                m_key = m_lastKey = new ConsoleKeyInfo();
                Console.Write(prompt);
                do
                {
                    m_key = Console.ReadKey(true);
                    switch (m_key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (inputPosition > 0)
                            {
                                inputPosition--;
                                var pos = HandleMoveLeft(input, inputPosition);
                                Console.SetCursorPosition(pos.Item1, pos.Item2);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (inputPosition < input.Count)
                            {
                                var pos = HandleMoveRight(input, inputPosition++);
                                Console.SetCursorPosition(pos.Item1, pos.Item2);
                            }
                            break;
                        default:
                            if (m_key.Key == ConsoleKey.Tab && completionList != null && completionList.Count > 0)
                            {
                                var tempPosition = inputPosition;
                                var word = new List<char>();
                                while (tempPosition-- > 0 && !string.IsNullOrWhiteSpace(input[tempPosition].ToString()))
                                    word.Insert(0, input[tempPosition]);

                                if (m_lastKey.Key == ConsoleKey.Tab)
                                {
                                    wordIterator?.MoveNext();
                                    if (completion != null)
                                    {
                                        ClearLine(input, inputPosition);
                                        for (var i = 0; i < completion.Length; i++)
                                        {
                                            input.RemoveAt(--inputPosition);
                                        }
                                        RewriteLine(input, inputPosition);
                                    }
                                    else
                                    {
                                        ClearLine(input, inputPosition);
                                        for (var i = 0; i < string.Concat(word).Length; i++)
                                        {
                                            input.RemoveAt(--inputPosition);
                                        }
                                        RewriteLine(input, inputPosition);
                                    }
                                }
                                else
                                {
                                    ClearLine(input, inputPosition);
                                    for (var i = 0; i < string.Concat(word).Length; i++)
                                    {
                                        input.RemoveAt(--inputPosition);
                                    }
                                    RewriteLine(input, inputPosition);
                                    wordIterator = GetMatch(completionList, string.Concat(word)).GetEnumerator();
                                    while (wordIterator.Current == null)
                                        wordIterator.MoveNext();
                                }

                                if (wordIterator != null) completion = wordIterator.Current;
                                ClearLine(input, inputPosition);
                                if (null == completion) throw new Exception(@"completion is null");
                                foreach (var c in completion.ToCharArray())
                                {
                                    input.Insert(inputPosition++, c);
                                }
                                RewriteLine(input, inputPosition);

                            }
                            else if (m_key.Key == ConsoleKey.Home || (m_key.Key == ConsoleKey.H && m_key.Modifiers == ConsoleModifiers.Control))
                            {
                                inputPosition = 0;
                                Console.SetCursorPosition(prompt.Length, m_startingCursorTop);
                            }

                            else if (m_key.Key == ConsoleKey.End || (m_key.Key == ConsoleKey.E && m_key.Modifiers == ConsoleModifiers.Control))
                            {
                                inputPosition = input.Count;
                                var cursorLeft = 0;
                                var cursorTop = m_startingCursorTop;
                                if ((inputPosition + m_prompt.Length) / Console.BufferWidth > 0)
                                {
                                    cursorTop += (inputPosition + m_prompt.Length) / Console.BufferWidth;
                                    cursorLeft = (inputPosition + m_prompt.Length) % Console.BufferWidth;
                                }
                                Console.SetCursorPosition(cursorLeft, cursorTop);
                            }

                            else if (m_key.Key == ConsoleKey.Delete)
                            {
                                if (inputPosition < input.Count)
                                {
                                    input.RemoveAt(inputPosition);
                                    ClearLine(input, inputPosition);
                                    RewriteLine(input, inputPosition);
                                }
                            }

                            else if (m_key.Key == ConsoleKey.UpArrow)
                            {
                                if (inputHistoryPosition > 0)
                                {
                                    inputHistoryPosition -= 1;
                                    ClearLine(input, inputPosition);

                                    // ToList() so we make a copy and don't use the reference in the list
                                    input = inputHistory[inputHistoryPosition].ToList();
                                    RewriteLine(input, input.Count);
                                    inputPosition = input.Count;
                                }
                            }
                            else if (m_key.Key == ConsoleKey.DownArrow)
                            {
                                if (inputHistoryPosition < inputHistory.Count - 1)
                                {
                                    inputHistoryPosition += 1;
                                    ClearLine(input, inputPosition);

                                    // ToList() so we make a copy and don't use the reference in the list
                                    input = inputHistory[inputHistoryPosition].ToList();
                                    RewriteLine(input, input.Count);
                                    inputPosition = input.Count;
                                }
                                else
                                {
                                    inputHistoryPosition = inputHistory.Count;
                                    ClearLine(input, inputPosition);
                                    Console.SetCursorPosition(prompt.Length, Console.CursorTop);
                                    input = new List<char>();
                                    inputPosition = 0;
                                }
                            }
                            else if (m_key.Key == ConsoleKey.Backspace)
                            {
                                if (inputPosition > 0)
                                {
                                    inputPosition--;
                                    input.RemoveAt(inputPosition);
                                    ClearLine(input, inputPosition);
                                    RewriteLine(input, inputPosition);
                                }
                            }

                            else if (m_key.Key == ConsoleKey.Escape)
                            {
                                if (m_lastKey.Key == ConsoleKey.Escape)
                                    Environment.Exit(0);
                                else
                                    Console.WriteLine(@"Press Escape again to exit.");
                            }

                            else if (m_key.Key == ConsoleKey.Enter && (m_key.Modifiers == ConsoleModifiers.Shift || m_key.Modifiers == ConsoleModifiers.Alt))
                            {
                                input.Insert(inputPosition++, '\n');
                                RewriteLine(input, inputPosition);
                            }

                            // multiline paste event
                            else if (m_key.Key == ConsoleKey.Enter && Console.KeyAvailable)
                            {
                                input.Insert(inputPosition++, '\n');
                                RewriteLine(input, inputPosition);
                            }

                            else if (m_key.Key != ConsoleKey.Enter)
                            {

                                input.Insert(inputPosition++, m_key.KeyChar);
                                RewriteLine(input, inputPosition);
                            }
                            break;
                    }

                    m_lastKey = m_key;
                } while (!(m_key.Key == ConsoleKey.Enter && Console.KeyAvailable == false)
                         // If Console.KeyAvailable = true then we have a multiline paste event
                         || (m_key.Key == ConsoleKey.Enter && (m_key.Modifiers == ConsoleModifiers.Shift || m_key.Modifiers == ConsoleModifiers.Alt)));

                var newlines = input.Count(a => a == '\n') > (input.Count / Console.BufferWidth)
                    ? input.Count(a => a == '\n')
                    : (input.Count / Console.BufferWidth);
                Console.SetCursorPosition(prompt.Length, m_startingCursorTop + newlines + 1);
                Enumerable.Range(0, newlines).ForEach(x => Console.WriteLine());
                Console.SetCursorPosition(prompt.Length, Console.CursorTop);


                var cmd = string.Concat(input);
                if (string.IsNullOrWhiteSpace(cmd))
                    continue;

                if (!inputHistory.Contains(input))
                    inputHistory.Add(input);

                Console.Write(lambda(cmd, input, completionList));

            }
        }
    }
}