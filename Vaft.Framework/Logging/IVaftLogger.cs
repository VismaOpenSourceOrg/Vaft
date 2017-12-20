namespace Vaft.Framework.Logging
{
    public interface IVaftLogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
