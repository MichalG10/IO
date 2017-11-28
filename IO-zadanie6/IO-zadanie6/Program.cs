using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Threading;

namespace IO_zadanie6
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream fsSource = new FileStream("plik.txt", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fsSource.Length];
            AutoResetEvent a = new AutoResetEvent(false);
            WaitHandle handle = a;
            fsSource.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[] { fsSource, buffer, a });
            
            handle.WaitOne();
           
        }

        static void myAsyncCallback(IAsyncResult state) {
            var param = (object[])state.AsyncState;
            FileStream fs = param[0] as FileStream;
            byte[] buff = param[1] as byte[];
            AutoResetEvent evet = param[2] as AutoResetEvent;
            string s = System.Text.Encoding.UTF8.GetString(buff, 0, buff.Length);
            Console.WriteLine(s);

            evet.Set();
            fs.EndRead(state);
        }


        
    }
}




