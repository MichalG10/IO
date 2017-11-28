using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Drawing;

namespace IO_zadanie8
{
    class Program
    {
        public delegate int Foo(int X);

        static void Main(string[] args)
        {
            Program a = new Program();
            WaitHandle[] waitHandles = new WaitHandle[4];
            Foo MyFoo = new Foo(Fib_Iter);
            waitHandles[0] = new AutoResetEvent(false);
            MyFoo.BeginInvoke(20,myAsyncCallback, new object[] { MyFoo, waitHandles[0], "Fib_iter: " });

            MyFoo = Fib_Rek;
            waitHandles[1] = new AutoResetEvent(false);
            MyFoo.BeginInvoke(20, myAsyncCallback, new object[] { MyFoo, waitHandles[1], "Fib_Rek: " });

            MyFoo = Sil_Iter;
            waitHandles[2] = new AutoResetEvent(false);
            MyFoo.BeginInvoke(10, myAsyncCallback, new object[] { MyFoo, waitHandles[2], "Sil_iter: " });

            MyFoo = Sil_Rek;
            waitHandles[3] = new AutoResetEvent(false);
            MyFoo.BeginInvoke(10, myAsyncCallback, new object[] { MyFoo, waitHandles[3], "Sil_Rek: " });
            WaitHandle.WaitAll(waitHandles);
            Console.WriteLine("Poczekal");
        }

        static int Fib_Iter(int z)
        {
            int a = 1;
            int b = 1;
            for(int i = 2; i <z; i++)
            {
                int c = a+b;
                a = b;
                b = c;
            }
            z = b;
            
            return z;
        }

        static int Fib_Rek(int z)
        {
            if (z <= 0) return 0;
            else if (z == 1 || z == 2) return 1;
            else return Fib_Rek(z - 2) + Fib_Rek(z - 1);
        }

        static int Sil_Iter(int z)
        {
            int sil = 1;
            for(int i = 1; i <= z; i++)
            {
                sil = sil * i;
            }
            
            return sil;
        }

        static int Sil_Rek(int z)
        {
            if (z == 0) return 1;
            else if (z == 1) return 1;
            else return Sil_Rek(z - 1) * z;
        }

        static void myAsyncCallback(IAsyncResult state)
        {
            var param = (object[])state.AsyncState;
            Foo fs = param[0] as Foo;
            String s = param[2] as String;
            int z = fs.EndInvoke(state);
            Console.WriteLine(s+z);
            AutoResetEvent evet = param[1] as AutoResetEvent;
            evet.Set();
        }

    }
}
