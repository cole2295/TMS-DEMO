using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Transfer.Service;

namespace InfoService
{
    class Program
    {
        static void Main(string[] args)
        {
            ReceiveHandler rec = new ReceiveHandler("10.16.4.139", 5000);

            rec.StartListening();

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();

        }
    }
}
