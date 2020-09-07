using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;

namespace LuzesRGB.Helpers
{
    public class WindowsStartup
    {
        private static readonly string SUB_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string APP_NAME = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
        private static readonly string EXEC_COMMAND = $"\"{Application.ExecutablePath}\" -boot";

        public static void Set(bool startWithWindows)
        {
            if (startWithWindows)
                Registry.CurrentUser.OpenSubKey(SUB_KEY, true).SetValue(APP_NAME, EXEC_COMMAND);
            else
                Registry.CurrentUser.OpenSubKey(SUB_KEY, true).DeleteValue(APP_NAME);
        }

        public static bool Get() =>
            Registry.CurrentUser.OpenSubKey(SUB_KEY).GetValue(APP_NAME)?.ToString() == EXEC_COMMAND;
    }
}