namespace OpsGenieApi.Helpers
{
    public interface IJsonSerializer
    {
        T DeserializeFromString<T>(string json);

        string SerializeToString<T>(T data);
    }
}