using System.IO;
using System.Runtime.Serialization.Json;

namespace T034.Tools
{
    public class SerializeTools
    {
        public static T Deserialize<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var model = (T)serializer.ReadObject(stream);
            stream.Close();

            return model;
        }
    }
}