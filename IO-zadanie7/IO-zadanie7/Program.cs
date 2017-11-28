using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Threading;

namespace IO_zadanie7
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream fsSource = new FileStream("plik.txt", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fsSource.Length];
            IAsyncResult state = fsSource.BeginRead(buffer, 0, buffer.Length, null, new object[] { });

            fsSource.EndRead(state);

            string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.WriteLine(s);
            

        }

        
    }
}