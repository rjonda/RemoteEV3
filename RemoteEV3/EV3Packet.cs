using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteEV3 {
    public class EV3Packet {
        public EV3Packet(int messageLenght, int messageCounter, int nameLength, string name, int contentLength, string content, byte[] RAW) {
            // generate string packet
            this.messageLenght = messageLenght;
            this.messageCounter = messageCounter;
            this.nameLength = nameLength;
            this.name = name;
            this.contentLength = contentLength;
            this.string_content = content;
            this.RAW = RAW;
            this.type = "'";
        }

        public EV3Packet(int messageLenght, int messageCounter, int nameLength, string name, int contentLength, bool content, byte[] RAW) {
            // generate bool packet
            this.messageLenght = messageLenght;
            this.messageCounter = messageCounter;
            this.nameLength = nameLength;
            this.name = name;
            this.contentLength = contentLength;
            this.bool_content = content;
            this.RAW = RAW;
            this.type = "T";
        }

        public EV3Packet(int messageLenght, int messageCounter, int nameLength, string name, int contentLength, float content, byte[] RAW) {
            // generate float packet
            this.messageLenght = messageLenght;
            this.messageCounter = messageCounter;
            this.nameLength = nameLength;
            this.name = name;
            this.contentLength = contentLength;
            this.float_content = content;
            this.RAW = RAW;
            this.type = "0";
        }

        public byte[] RAW { get; private set; }

        public int messageLenght { get; private set; }
        public int messageCounter { get; private set; }
        public int nameLength { get; private set; }
        public string name { get; private set; }
        public int contentLength { get; private set; }
        public string string_content { get; private set; }
        public bool bool_content { get; private set; }
        public float float_content { get; private set; }
        public string type { get; private set; }
    }
}
