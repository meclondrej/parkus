using PluginAPI.Core.Attributes;

namespace parkus
{
    public class Plugin
    {
        public static Plugin Singleton;

        public const string PluginName = "parkus";
        public const string PluginVersion = "v1.0.0";
        public const string PluginDescription = "parkus server core plugin";
        public const string PluginAuthor = "meclondrej";

        [PluginEntryPoint(PluginName, PluginVersion, PluginDescription, PluginAuthor)]
        void LoadPlugin()
        {
            Singleton = this;
        }
    }
}