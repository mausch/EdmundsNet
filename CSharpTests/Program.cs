using Microsoft.FSharp.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpTests {
    class Program {
        static void Main(string[] args) {
            var edmunds = new EdmundsNet.Service(apiKey: "");
            var t = FSharpAsync.StartAsTask(edmunds.AsyncLookupByVIN("2G1FC3D33C9165616"), null, null);
            Console.WriteLine(t.Result);
        }
    }
}
