using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_zadanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 6000 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 5000 });
            Thread.Sleep(7000);
        }

        static void ThreadProc(Object stateInfo)
        {
            var czas = ((object[])stateInfo)[0];
            Thread.Sleep((int)czas);
            Console.WriteLine("Spalem "+ czas + "ms");
        }


    }
}

//Gdy w głównym watku damy zbyt mały czas w Sleep to niektóre wątki z kolejki mogą nie zdążyć się zakończyć. 
//To że wątek jest wywoływany jako pierwszy nie znaczy że się zakończy jako pierwszy