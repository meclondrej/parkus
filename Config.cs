using Exiled.API.Interfaces;

namespace parkus
{
    public class Config : IConfig
    {
        bool IConfig.IsEnabled { get; set; } = true;
        bool IConfig.Debug { get; set; } = false;
    }
}