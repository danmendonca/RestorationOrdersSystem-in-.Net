using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

class Program
{
    List<Product> ps = new List<Product>();
    static void Main(string[] args)
    {
        RemotingConfiguration.Configure("Printer.exe.config", false);
        Printer p = new Printer();

        while (!Console.ReadLine().Equals("q")) { }
    }

    
}