using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalHealth.Blazor.Client.Infrastructure.Utilities
{
    // Original author: John Thiriet
    // https://johnthiriet.com/
    internal static class StreamUtilities
    {
        internal static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                return default(T);
            }
            
            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var result = js.Deserialize<T>(jtr);
                return result;
            }
        }
        
        internal static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
        
        internal static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using var sr = new StreamReader(stream);
                content = await sr.ReadToEndAsync();
            }

            return content;
        }
    }
}