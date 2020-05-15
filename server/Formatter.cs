using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace server
{
    public class Formatter
    {
        static BinaryFormatter formatter = new BinaryFormatter();
        public static byte[] Serialize(Person[] tmp)
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, tmp);
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
        public static Person[] Deserialize(byte[] buffer)
        {
            Person[] tmp = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;
                tmp = (Person[])formatter.Deserialize(ms);
            }
            return tmp;
        }
    }
}
