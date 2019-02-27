using Newtonsoft.Json;
using System;
using System.IO;

namespace MovieFinder.Client.Data
{
    /// <summary>
    /// Extension Class use for API Clients
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Returns object in specified type from Stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
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
