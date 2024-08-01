using System;
using parkus.Features;

namespace parkus
{
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public static Plugin Singleton;

        public override string Name => "parkus";
        public override string Prefix => "parkus";
        public override Version Version => new Version(0, 1, 0);
        public override string Author => "meclondrej";

        public Handlers handlers;

        public Plugin()
        {
            Singleton = this;
        }

        public override void OnEnabled()
        {
            handlers = new Handlers();
            handlers.RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            handlers.UnregisterEvents();
            handlers = null;
            base.OnDisabled();
        }
    }
}