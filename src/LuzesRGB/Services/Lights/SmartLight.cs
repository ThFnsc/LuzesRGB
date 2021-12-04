using System;
using System.Collections.Generic;
using System.Net;

namespace LuzesRGB.Services.Lights
{
    public class SmartLight
    {
        private static readonly Dictionary<Types, Type> _mapping = new Dictionary<Types, Type>
            {
                {Types.Magichome, typeof(MagicHomeLight) },
                {Types.LegacyMagichome, typeof(MagicHomeLightLegacy) },
                {Types.Yeelight, typeof(YeelightLight) },
                {Types.UDP35225, typeof(UDPLight) }
            };

        public string Name { get; set; }

        public string IP { get; set; }

        public Types Type { get; set; }

        public override string ToString() =>
            $"{Type}: {Name} @ {IP}";

        public ISmartLight Instantiate()
        {
            var instance = Activator.CreateInstance(_mapping[Type]) as ISmartLight;
            instance.IPAddress = IPAddress.Parse(IP);
            return instance;
        }

        public enum Types
        {
            Magichome,
            Yeelight,
            LegacyMagichome,
            UDP35225
        }
    }
}