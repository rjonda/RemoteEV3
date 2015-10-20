using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RemoteEV3 {
    
    public delegate void DataReceivedDelegate(EV3Packet packet);
    public class EV3Receiver {

        private DataReceivedDelegate dataReceivedDelegate;
        private SerialPort EV3ComPort = new SerialPort();

        private delegate void NXTMailboxCallback(object sender, string s);
        private System.Timers.Timer NXTMailbox1CallbackTimer = null;
        static readonly object _object = new object();

        public EV3Receiver(string portName, DataReceivedDelegate dataReceivedDelegate, int interval) {
            this.dataReceivedDelegate = dataReceivedDelegate;
            try {
                EV3ComPort.PortName = portName;
                EV3ComPort.BaudRate = 9600;
                EV3ComPort.DataBits = 8;
                EV3ComPort.Parity = Parity.None;
                EV3ComPort.StopBits = StopBits.One;

                EV3ComPort.Open();

                EV3ComPort.ReadTimeout = 20000;

                EV3ComPort.DtrEnable = true;
                EV3ComPort.RtsEnable = true;

                NXTMailbox1CallbackTimer = new System.Timers.Timer(interval);
                NXTMailbox1CallbackTimer.Elapsed += new ElapsedEventHandler(NXTMailbox1CallbackHandler);
                NXTMailbox1CallbackTimer.Enabled = true;

            } catch (System.IO.IOException ex) {
                Console.WriteLine(ex.ToString());
                Console.Read();
                return;

            } catch (ArgumentException ex) {
                Console.WriteLine(ex.ToString());
                Console.Read();
                return;
            }
        }

        public bool isConnected() {
            return EV3ComPort.IsOpen;
        }

        private void NXTMailbox1CallbackHandler(object source, ElapsedEventArgs e) {
            lock (_object) {
                // When data is received, do following: 

                //bbbbmmmmttssllaaaLLLLppp

                //bbbb = bytes in the message, little endian
                //mmmm = message counter
                //tt   = 0×81
                //ss   = 0x9E
                //ll   = mailbox name length INCLUDING the \0 terminator
                //aaa… = mailbox name, should be terminated with a \0
                //LLLL = payload length INCLUDING the , little endian
                //ppp… = payload, should be terminated with the \0

                int bytes = EV3ComPort.BytesToRead;
                byte[] data = new byte[bytes];
                EV3ComPort.Read(data, 0, bytes);

                if (data.Length > 1) {
                    //Console.WriteLine(BitConverter.ToString(data));

                    int messageLenght = BitConverter.ToInt16(data, 0);
                    int messageCounter = BitConverter.ToInt16(new byte[] { data[3], data[2] }, 0);

                    int nameLength = BitConverter.ToInt16(new byte[] { data[6], 0x00 }, 0);
                    string name = Encoding.ASCII.GetString(data.Skip(7).Take(nameLength - 1).ToArray());


                    // Generate EV3Packet
                    if (data[data.Count() - 3] == 0x01 && data[data.Count() - 2] == 0x00 && data[data.Count() - 1] == 0x01) {
                        dataReceivedDelegate(new EV3Packet(messageLenght, messageCounter, nameLength, name, 1, true, data));
                    } else if (data[data.Count() - 3] == 0x01 && data[data.Count() - 2] == 0x00 && data[data.Count() - 1] == 0x00) {
                        dataReceivedDelegate(new EV3Packet(messageLenght, messageCounter, nameLength, name, 1, false, data));
                    } else if (data[data.Count() - 1] == 0x00) {
                        int contentLength = BitConverter.ToInt16(new byte[] { data[7 + nameLength], data[8 + nameLength] }, 0);
                        string content = Encoding.ASCII.GetString(data.Skip(9 + nameLength).Take(contentLength - 1).ToArray());

                        // call delegate from main thread
                        dataReceivedDelegate(new EV3Packet(messageLenght, messageCounter, nameLength, name, contentLength, content, data));
                    } else {
                        int contentLength = BitConverter.ToInt16(new byte[] { data[7 + nameLength], data[8 + nameLength] }, 0);
                        float content = BitConverter.ToSingle(data.Skip(9 + nameLength).Take(contentLength).ToArray(), 0);

                        // call delegate from main thread
                        dataReceivedDelegate(new EV3Packet(messageLenght, messageCounter, nameLength, name, contentLength, content, data));
                    }
                }
            }
        }
    }
}
