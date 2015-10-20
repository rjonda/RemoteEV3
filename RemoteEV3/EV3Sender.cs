using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace RemoteEV3 {
    public class EV3Sender {

        private SerialPort EV3ComPort = new SerialPort();

        public EV3Sender(string portName) {
            // Connect to bluetooth port of EV3 brick
            try {
                EV3ComPort.PortName = portName;
                EV3ComPort.BaudRate = 9600;
                EV3ComPort.DataBits = 8;
                EV3ComPort.Parity = Parity.None;
                EV3ComPort.StopBits = StopBits.One;

                EV3ComPort.DtrEnable = true;
                EV3ComPort.RtsEnable = true;

                EV3ComPort.Open();

                EV3ComPort.ReadTimeout = 20000;

            } catch (System.IO.IOException e) {
                Console.WriteLine("! ERROR 1: " + e.Message);
                return;

            } catch (ArgumentException) {
                Console.WriteLine("! ERROR 2!");
                return;
            }
            return;
        }

        public bool isConnected() {
            return EV3ComPort.IsOpen;
        }

        public void Send(string name, string content) {
            // Sends text EV3Packet in this format: 

            // bbbbmmmmttssllaaaLLLLppp
            // bbbb = bytes in the message, little endian
            // mmmm = message counter
            // tt   = 0×81
            // ss   = 0x9E
            // ll   = mailbox name length INCLUDING the \0 terminator
            // aaa… = mailbox name, should be terminated with a \0
            // LLLL = payload length INCLUDING the , little endian
            // ppp… = payload, should be terminated with the \0

            List<byte> MessageHeaderList = new List<byte>();

            MessageHeaderList.AddRange(new byte[] { 0x00, 0x01, 0x81, 0x9E }); // mmmmm + tt + ss
            List<byte> by = BitConverter.GetBytes((Int16)(name.Length + 1)).Reverse().ToList();
            by.RemoveAt(0);
            MessageHeaderList.AddRange(by.ToArray()); // ll
            MessageHeaderList.AddRange(Encoding.ASCII.GetBytes(name)); // aaa…
            MessageHeaderList.AddRange(new byte[] { 0x00 }); // \0
            MessageHeaderList.AddRange(BitConverter.GetBytes((Int16)(content.Length + 1))); // LLLL
            MessageHeaderList.AddRange(Encoding.ASCII.GetBytes(content)); // ppp…
            MessageHeaderList.AddRange(new byte[] { 0x00 }); // \0
            MessageHeaderList.InsertRange(0, BitConverter.GetBytes((Int16)(MessageHeaderList.Count))); // bbbb

            EV3ComPort.Write(MessageHeaderList.ToArray(), 0, MessageHeaderList.ToArray().Length);
        }

        public void Send(string name, float content) {
            // Sends float EV3Packet in this format: 
            
            // bbbbmmmmttssllaaaLLLLppp
            // bbbb = bytes in the message, little endian
            // mmmm = message counter
            // tt   = 0×81
            // ss   = 0x9E
            // ll   = mailbox name length INCLUDING the \0 terminator
            // aaa… = mailbox name, should be terminated with a \0
            // LLLL = payload length INCLUDING the , little endian
            // ppp… = payload, should be terminated with the \0

            List<byte> MessageHeaderList = new List<byte>();

            MessageHeaderList.AddRange(new byte[] { 0x00, 0x01, 0x81, 0x9E }); // mmmmm + tt + ss
            List<byte> by = BitConverter.GetBytes((Int16)(name.Length + 1)).Reverse().ToList();
            by.RemoveAt(0);
            MessageHeaderList.AddRange(by.ToArray()); // ll
            MessageHeaderList.AddRange(Encoding.ASCII.GetBytes(name)); // aaa…
            MessageHeaderList.AddRange(new byte[] { 0x00 }); // \0
            MessageHeaderList.AddRange(BitConverter.GetBytes((Int16)(4))); // LLLL
            MessageHeaderList.AddRange(BitConverter.GetBytes(content)); // ppp…
            MessageHeaderList.InsertRange(0, BitConverter.GetBytes((Int16)(MessageHeaderList.Count))); // bbbb

            EV3ComPort.Write(MessageHeaderList.ToArray(), 0, MessageHeaderList.ToArray().Length);
        }
        public void Send(string name, bool content) {
            // Sends bool EV3Packet in this format: 
            
            //bbbbmmmmttssllaaaLLLLppp
            //bbbb = bytes in the message, little endian
            //mmmm = message counter
            //tt   = 0×81
            //ss   = 0x9E
            //ll   = mailbox name length INCLUDING the \0 terminator
            //aaa… = mailbox name, should be terminated with a \0
            //LLLL = payload length INCLUDING the , little endian
            //ppp… = payload, should be terminated with the \0

            List<byte> MessageHeaderList = new List<byte>();

            MessageHeaderList.AddRange(new byte[] { 0x00, 0x01, 0x81, 0x9E }); // mmmmm + tt + ss
            List<byte> by = BitConverter.GetBytes((Int16)(name.Length + 1)).Reverse().ToList();
            by.RemoveAt(0);
            MessageHeaderList.AddRange(by.ToArray()); // ll
            MessageHeaderList.AddRange(Encoding.ASCII.GetBytes(name)); // aaa…
            MessageHeaderList.AddRange(new byte[] { 0x00 }); // \0
            MessageHeaderList.AddRange(BitConverter.GetBytes((Int16)(1))); // LLLL
            if (content) {
                MessageHeaderList.AddRange(new byte[] { 0x01 }); // ppp…
            } else {
                MessageHeaderList.AddRange(new byte[] { 0x00 }); // ppp…
            }
            MessageHeaderList.AddRange(new byte[] { 0x00 }); // \0
            MessageHeaderList.InsertRange(0, BitConverter.GetBytes((Int16)(MessageHeaderList.Count))); // bbbb

            EV3ComPort.Write(MessageHeaderList.ToArray(), 0, MessageHeaderList.ToArray().Length);
        }
    }
}
