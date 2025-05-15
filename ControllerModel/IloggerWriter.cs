namespace LibrairieJsonHelper
{
    public interface ILoggerWriter
    {
        void WriteLog<T>(string path, T obj);

    }
}
