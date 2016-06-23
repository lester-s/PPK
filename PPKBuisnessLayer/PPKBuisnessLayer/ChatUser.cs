using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PPKBuisnessLayer
{
    public class ChatUser
    {
        public Socket UserSocket { get; set; }
        public BinaryReader UserReader { get; set; }
        public BinaryWriter UserWriter { get; set; }
    }
}
