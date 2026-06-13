using System;

namespace parkus
{
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public static Plugin Singleton;

        public override string Name => "parkus";
        public override string Prefix => "parkus";
        public override Version Version => new Version(0, 1, 0);
        public override string Author => "meclondrej";

        public Plugin()
        {
            Singleton = this;
        }

        public override void OnEnabled()
        {
            Handlers.RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Handlers.UnregisterEvents();
            Handlers.OnDisabled();
            base.OnDisabled();
        }
    }
}
