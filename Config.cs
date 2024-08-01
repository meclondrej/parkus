using System.ComponentModel;
using Exiled.API.Interfaces;

namespace parkus
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled")]
        public bool IsEnabled { get; set; } = true;
        [Description("Whether or not debug messages should be shown in the console")]
        public bool Debug { get; set; } = false;
    }
}