using Newtonsoft.Json;
using System;
using System.IO;

namespace MovieFinder.Client.Data
{
    public static class ClientExtensions
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException(string.Format("Can't read from this client - {0}", nameof(stream)));
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var obj = new JsonSerializer().Deserialize<T>(jsonTextReader);

                    return obj;
                }
            }
        }
    }
}
