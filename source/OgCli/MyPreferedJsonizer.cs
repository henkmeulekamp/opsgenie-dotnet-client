using OpsGenieApi.Helpers;
using ServiceStack.Text;

namespace OpsGenieCli
{
    internal class MyPreferedJsonizer : IJsonSerializer
    {
        public T DeserializeFromString<T>(string json)
        {
            return JsonSerializer.DeserializeFromString<T>(json);
        }

        public string SerializeToString<T>(T data)
        {
            return JsonSerializer.SerializeToString(data);
        }
    }
}