using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB.Services.Lights
{
    public class SmartLight
    {
        private static readonly Dictionary<Types, Type> Mapping = new Dictionary<Types, Type>
            {
                {Types.Magichome, typeof(MagicHomeLight) },
                {Types.LegacyMagichome, typeof(MagichomeLED) },
                {Types.Yeelight, typeof(YeelightLight) }
            };

        public string Name { get; set; }

        public string IP { get; set; }

        public Types Type { get; set; }

        public override string ToString() =>
            $"{Type}: {Name} @ {IP}";

        public ISmartLight Instantiate()
        {
            var instance = Activator.CreateInstance(Mapping[Type]) as ISmartLight;
            instance.IPAddress = IPAddress.Parse(IP);
            return instance;
        }

        public enum Types
        {
            Magichome,
            Yeelight,
            LegacyMagichome
        }
    }
}