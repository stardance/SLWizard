using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SL.Utils
{
    public class XmlHelper
    {
        public static T Read<T>(string path) where T : class, new()
        {
            using (var sr = new StreamReader(path))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                T t = (T)ser.Deserialize(sr);
                return t;
            }
        }

        public static void Write<T>(string path, T t) where T : class, new()
        {
            using (var sr = new StreamWriter(path))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(sr, t);
            }
        }

    }
}
