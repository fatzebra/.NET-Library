using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var gw = new FatZebra.Gateway("TEST", "TEST");
            gw.GatewayAddress = "gateway.sandbox.fatzebra.com.au";

            var ping = gw.Ping();
            Console.WriteLine("Ping: {0}", ping ? "OK" : "Failed");

            var purchase = gw.Purchase(100, "M Savage", "5123456789012346", DateTime.Now.AddYears(1), "123", "test" + Guid.NewGuid().ToString(), "127.0.0.1");

            Console.WriteLine("Purchase. Request: {0}, Result: {1}, ID: {2}", purchase.Successful, purchase.Result.Successful, purchase.Result.ID);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
