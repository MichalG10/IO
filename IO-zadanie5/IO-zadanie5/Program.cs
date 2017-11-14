using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Drawing;

namespace IO_zadanie5
{
    class Program

    {
        static int n = 100;
        static int[] tab = new int[n];
        static int wielkosc = 3;
        static int ilosc = 0;
        static int suma = 0;

        static Random rand = new Random();

        static private Object thisLock = new Object();
       
        static void Main(string[] args)
        {
            
            for (int i = 0; i < n; i++)
            {
                int r = rand.Next(1000);
                tab[i] = r;
                Console.WriteLine(tab[i]);
            }
            ilosc = n / wielkosc;
            bool niep = false;
            int reszta = 0;
            if (n % wielkosc != 0)
            {
                niep = true;
                reszta = n % wielkosc;
            }

            int numOfThreads = ilosc;
            if (niep == true) numOfThreads++;
            Console.WriteLine(numOfThreads);
            WaitHandle[] waitHandles = new WaitHandle[numOfThreads];

            for (int i = 0; i < ilosc; i++)
            {
                int z = i * wielkosc;
                waitHandles[i] = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(Blok, new object[] {z, z+wielkosc-1, waitHandles[i] });
                if (ilosc-1 == i && niep == true) {
                    Console.WriteLine("i+1= " + (i));
                    z = z + wielkosc;
                    waitHandles[i+1] = new AutoResetEvent(false);
                    ThreadPool.QueueUserWorkItem(Blok, new object[] { z, z + reszta - 1, waitHandles[i+1] });
                }
               
            }

            WaitHandle.WaitAll(waitHandles);
            Console.WriteLine("Poczekal");
        }


        static void Blok(Object stateInfo)
        {
            var pom = ((object[])stateInfo)[0];
            int poczatek = (int)pom;
            pom = ((object[])stateInfo)[1];
            int koniec = (int)pom;
            int sum = 0;
            for(int i = poczatek; i <= koniec; i++)
            {
                sum = sum + tab[i];
            }
            Sumuj(sum);
            pom = ((object[])stateInfo)[2];
            AutoResetEvent waitHandle = (AutoResetEvent)pom;
            waitHandle.Set();
        }

        static void Sumuj(int sum)
        {
            lock (Program.thisLock) { 
                int pom = suma + sum;
                Console.WriteLine(sum + " + " + suma + "=" + pom);
                suma = pom;
                Console.WriteLine(suma);

            }

        }
    }
}
//Tablica o określonej wielkości jest wypełniana liczbami pseudolosowymi od 0 do 1000. Jeśli wielkosc tablicy modulo wielkosc jednego fragmentu jest różna od 0 to liczba fragmentow jest zwiekszana o 1.
//Następnie tworzona jest tablica handlerów. Przy wywolaniu każdego wątku tworzony jest obiekt AutoResetEvent przypisany do danego wątku. Gdy wątek się wykona nastepuje wywołanie metody set i zmiana stanu Eventu.
//Gdy wszystkie wątki się wykonają następuje wyjście z metody WaitAll.
//Mechanizm pozwala na synchronizacje pojedyńczych wątków lub określonej grupy.
//Wywołanie zbyt wielu wątków może doprowadzić do wysypania programu.