using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using RemoteEV3;

namespace Test {
    class Program {

        static void Main(string[] args) {
            EV3Receiver ev3receiver = new EV3Receiver("COM5", OnCatch, 1);
            EV3Sender ev3sender = new EV3Sender("COM3");
            Console.WriteLine("Listening...");
            Console.ReadLine();
        }
        static void OnCatch(EV3Packet packet) {
            switch (packet.type) {
                case "'":
                    Console.WriteLine(packet.name + " (text): " + packet.string_content);
                    break;
                case "0":
                    Console.WriteLine(packet.name + " (number): " + packet.float_content);
                    break;
                case "T":
                    if (packet.bool_content) {
                        Console.WriteLine(packet.name + " (logic): true");
                    } else {
                        Console.WriteLine(packet.name + " (logic): false");
                    }                    
                    break;
            }
        }
    }
}
