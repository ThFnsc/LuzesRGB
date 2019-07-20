using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;

namespace LuzesRGB {
    class Startup {

        public static void Set(bool startWithWindows) {
            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)
                .SetValue(Path.GetFileNameWithoutExtension(Application.ExecutablePath),
                startWithWindows ? $"\"{Application.ExecutablePath}\" -boot" : null);
        }

        public static bool Get() {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run")
                .GetValue(Path.GetFileNameWithoutExtension(Application.ExecutablePath)).ToString() == $"\"{Application.ExecutablePath}\" -boot";
        }
    }
}
