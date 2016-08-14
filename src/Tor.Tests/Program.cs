using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

/* credit to: http://blogs.msdn.com/b/jpsanders/archive/2011/04/26/how-to-set-the-proxy-for-the-webbrowser-control-in-net.aspx
 * for the proxy classes
 */

namespace Tor.Tests
{
    static class Program
    {
        static readonly string agentName = "Mozilla/5.0 (compatible, MSIE 11, Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";

        [DllImport("user32.dll")]
        static extern bool SetProcessDPIAware();

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr InternetOpen(string lpszAgent, int dwAccessType, string lpszProxyName, string lpszProxyBypass, int dwFlags);

        [DllImport("wininet.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool InternetCloseHandle(IntPtr hInternet);

        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern bool InternetSetOption(IntPtr hInternet, INTERNET_OPTION dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern bool InternetQueryOptionList(IntPtr handle, INTERNET_OPTION optionFlag, ref INTERNET_PER_CONN_OPTION_LIST optionList, ref int size);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct INTERNET_PER_CONN_OPTION_LIST
        {
            public int Size;
            public IntPtr Connection;
            public int OptionCount;
            public int OptionError;
            public IntPtr pOptions;
        }

        enum INTERNET_OPTION
        {
            INTERNET_OPTION_PER_CONNECTION_OPTION = 75,
            INTERNET_OPTION_SETTINGS_CHANGED = 39,
            INTERNET_OPTION_REFRESH = 37
        }

        enum INTERNET_PER_CONN_OPTIONENUM
        {
            INTERNET_PER_CONN_FLAGS = 1,
            INTERNET_PER_CONN_PROXY_SERVER = 2,
            INTERNET_PER_CONN_PROXY_BYPASS = 3,
            INTERNET_PER_CONN_AUTOCONFIG_URL = 4,
            INTERNET_PER_CONN_AUTODISCOVERY_FLAGS = 5,
            INTERNET_PER_CONN_AUTOCONFIG_SECONDARY_URL = 6,
            INTERNET_PER_CONN_AUTOCONFIG_RELOAD_DELAY_MINS = 7,
            INTERNET_PER_CONN_AUTOCONFIG_LAST_DETECT_TIME = 8,
            INTERNET_PER_CONN_AUTOCONFIG_LAST_DETECT_URL = 9,
            INTERNET_PER_CONN_FLAGS_UI = 10
        }

        const int INTERNET_OPEN_TYPE_DIRECT = 1;
        const int INTERNET_OPEN_TYPE_PRECONFIG = 0;

        enum INTERNET_OPTION_PER_CONN_FLAGS
        {
            PROXY_TYPE_DIRECT = 0x00000001,   // direct to net
            PROXY_TYPE_PROXY = 0x00000002,   // via named proxy
            PROXY_TYPE_AUTO_PROXY_URL = 0x00000004,   // autoproxy URL
            PROXY_TYPE_AUTO_DETECT = 0x00000008   // use autoproxy detection
        }

        [StructLayout(LayoutKind.Explicit)]
        struct INTERNET_PER_CONN_OPTION_OPTIONUNION
        {
            [FieldOffset(0)]
            public int dwValue;
            [FieldOffset(0)]
            public IntPtr pszValue;
            [FieldOffset(0)]
            public FILETIME ftValue;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INTERNET_PER_CONN_OPTION
        {
            public int dwOption;
            public INTERNET_PER_CONN_OPTION_OPTIONUNION value;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                    SetProcessDPIAware();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ProgramUI());
            }
            catch { } 
        }

        public static bool SetConnectionProxy(string proxyServer)
        {
            IntPtr hInternet = InternetOpen(agentName, INTERNET_OPEN_TYPE_DIRECT, null, null, 0);

            INTERNET_PER_CONN_OPTION[] options = new INTERNET_PER_CONN_OPTION[2];
            options[0] = new INTERNET_PER_CONN_OPTION();
            options[0].dwOption = (int)INTERNET_PER_CONN_OPTIONENUM.INTERNET_PER_CONN_FLAGS;
            options[0].value.dwValue = (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY;

            options[1] = new INTERNET_PER_CONN_OPTION();
            options[1].dwOption = (int)INTERNET_PER_CONN_OPTIONENUM.INTERNET_PER_CONN_PROXY_SERVER;
            options[1].value.pszValue = Marshal.StringToHGlobalAnsi(proxyServer);

            IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(options[0]) + Marshal.SizeOf(options[1]));
            IntPtr current = buffer;

            for (int i = 0; i < options.Length; i++)
            {
                Marshal.StructureToPtr(options[i], current, false);
                current = (IntPtr)((int)current + Marshal.SizeOf(options[i]));
            }

            INTERNET_PER_CONN_OPTION_LIST optionList = new INTERNET_PER_CONN_OPTION_LIST();
            optionList.pOptions = buffer;
            optionList.Size = Marshal.SizeOf(optionList);
            optionList.Connection = IntPtr.Zero;
            optionList.OptionCount = options.Length;
            optionList.OptionError = 0;

            int size = Marshal.SizeOf(optionList);
            IntPtr intPtrStruct = Marshal.AllocCoTaskMem(size);
            Marshal.StructureToPtr(optionList, intPtrStruct, true);

            bool bReturn = InternetSetOption(hInternet, INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, intPtrStruct, size);

            Marshal.FreeCoTaskMem(buffer);
            Marshal.FreeCoTaskMem(intPtrStruct);

            InternetCloseHandle(hInternet);

            return bReturn;
        }
    }
}
