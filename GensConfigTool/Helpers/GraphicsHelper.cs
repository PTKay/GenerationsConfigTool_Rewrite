using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GensConfigTool.Helpers
{
    public class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(byte[] lpszDeviceName, [param: MarshalAs(UnmanagedType.U4)] int iModeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string deviceName, int modeNum, ref DISPLAY_DEVICE displayDevice, int flags);
    }
    public enum ScreenOrientation
    {
        /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle0"]/*' />
        /// <devdoc>
        ///    <para>
        ///       The screen is oriented at 0 degrees
        ///    </para>
        /// </devdoc>
        Angle0 = 0,

        /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle90"]/*' />
        /// <devdoc>
        ///    <para>
        ///       The screen is oriented at 90 degrees
        ///    </para>
        /// </devdoc>
        Angle90 = 1,

        /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle180"]/*' />
        /// <devdoc>
        ///    <para>
        ///       The screen is oriented at 180 degrees.
        ///    </para>
        /// </devdoc>
        Angle180 = 2,

        /// <include file='doc\ScreenOrientation.uex' path='docs/doc[@for="Day.Angle270"]/*' />
        /// <devdoc>
        ///    <para>
        ///       The screen is oriented at 270 degrees.
        ///    </para>
        /// </devdoc>
        Angle270 = 3,
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE
    {
        private const int CCHDEVICENAME = 32;
        private const int CCHFORMNAME = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public ScreenOrientation dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
        public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAY_DEVICE
    {
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        public int StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    public static class GraphicsHelper
    {
        public static List<DISPLAY_DEVICE> GetGraphicsAdapters()
        {
            int i = 0;
            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            List<DISPLAY_DEVICE> result = new List<DISPLAY_DEVICE>();
            displayDevice.cb = Marshal.SizeOf(displayDevice);
            while (NativeMethods.EnumDisplayDevices(null, i, ref displayDevice, 1))
            {
                if ((displayDevice.StateFlags & 0x1) > 0)
                    result.Add(displayDevice);
                i++;
            }

            return result;
        }

        public static List<DISPLAY_DEVICE> GetMonitors(string graphicsAdapter)
        {

            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            List<DISPLAY_DEVICE> result = new List<DISPLAY_DEVICE>();
            int i = 0;
            displayDevice.cb = Marshal.SizeOf(displayDevice);
            while (NativeMethods.EnumDisplayDevices(graphicsAdapter, i, ref displayDevice, 0))
            {
                result.Add(displayDevice);
                i++;
            }

            return result;
        }

        public static List<DEVMODE> GetDeviceModes(string graphicsAdapter)
        {
            var lptArray = new byte[graphicsAdapter.Length + 1];

            var index = 0;
            foreach (char c in graphicsAdapter.ToCharArray())
                lptArray[index++] = Convert.ToByte(c);

            lptArray[index] = Convert.ToByte('\0');

            int i = 0;
            DEVMODE devMode = new DEVMODE();
            List<DEVMODE> result = new List<DEVMODE>();
            while (NativeMethods.EnumDisplaySettings(lptArray, i, ref devMode))
            {
                result.Add(devMode);
                i++;
            }
            return result;
        }
    }
}
