namespace Vaft.Framework.Logging
{
    public static class VaftLogInitializer
    {
        public static IVaftLogger Run()
        {
            return VaftLogger.Initialize();
        }
    }
}
