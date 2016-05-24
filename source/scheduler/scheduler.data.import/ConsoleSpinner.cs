using System;
using System.Threading;

namespace scheduler.data.import
{
    public class ConsoleSpinner
    {
        private int m_counter;

        public void Turn()
        {
            m_counter++;
            switch (m_counter % 4)
            {
                case 0: Console.Write("/"); m_counter = 0; break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("|"); break;
            }
            Thread.Sleep(100);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
}