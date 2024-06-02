namespace LMS_library.Data_Service
{
    public interface ICaching
    {
        T GetData<T>(string key);
        bool SetData<T>(string key , T value , DateTimeOffset expirationTime);
        object RemoveData(string key);
    }
}
