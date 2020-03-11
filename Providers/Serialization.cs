using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace ClaimsDemo.Providers
{
    public static class Serialization
    {
        public static byte[] ToByteArray(this object obj)
        {
            try
            {
                if (obj == null)
                {
                    return null;
                }
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, obj);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }
        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            try
            {
                if (byteArray == null)
                {
                    return default(T);
                }
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream(byteArray))
                {
                    return binaryFormatter.Deserialize(memoryStream) as T;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
