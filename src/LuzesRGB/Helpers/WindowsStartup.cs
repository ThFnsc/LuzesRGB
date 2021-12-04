using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;

namespace LuzesRGB.Helpers
{
    public class WindowsStartup
    {
        private static readonly string _sUB_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string _aPP_NAME = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
        private static readonly string _eXEC_COMMAND = $"\"{Application.ExecutablePath}\" -boot";

        public static void Set(bool startWithWindows)
        {
            if (startWithWindows)
                Registry.CurrentUser.OpenSubKey(_sUB_KEY, true).SetValue(_aPP_NAME, _eXEC_COMMAND);
            else
                Registry.CurrentUser.OpenSubKey(_sUB_KEY, true).DeleteValue(_aPP_NAME);
        }

        public static bool Get() =>
            Registry.CurrentUser.OpenSubKey(_sUB_KEY).GetValue(_aPP_NAME)?.ToString() == _eXEC_COMMAND;
    }
}