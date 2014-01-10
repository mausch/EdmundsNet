using Microsoft.FSharp.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdmundsNet;

namespace CSharpTests {
    class Program {
        static void Main(string[] args) {
            var edmunds = new Service(apiKey: "");
            var t = edmunds.AsyncLookupByVIN("2G1FC3D33C9165616").GetOrThrow();
            Console.WriteLine(t);
        }
    }
}
