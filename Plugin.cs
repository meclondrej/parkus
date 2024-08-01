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

        public static IHandler[] NewHandlers => new IHandler[] {
            new Killcounter(),
            new ConnectionStatusBroadcast(),
            new RemoteKeycard(),
        };

        public IHandler[] handlers = null;

        public Plugin()
        {
            Singleton = this;
        }

        public override void OnEnabled()
        {
            handlers = NewHandlers;
            foreach (IHandler handler in handlers)
                handler.RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            foreach (IHandler handler in handlers)
                handler.UnregisterEvents();
            handlers = null;
            base.OnDisabled();
        }
    }
}