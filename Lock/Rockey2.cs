using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JD_Proc.Lock
{

    internal class Rockey2
    {

        [DllImport("Rockey2.dll")]
        internal static extern int RY2_Find();
        [DllImport("Rockey2.dll")]
        public static extern int RY2_Open(int mode, UInt32 uid, out UInt32 hid);
        [DllImport("Rockey2.dll")]
        internal static extern int RY2_Close(int handle);
        [DllImport("Rockey2.dll")]
        internal static extern int RY2_GenUID(int handle, out UInt32 uid, [MarshalAs(UnmanagedType.LPArray)] char[] seed, int isProtect);
        [DllImport("Rockey2.dll")]
        internal static extern int RY2_Read(int handle, int block_index, [MarshalAs(UnmanagedType.LPArray)] char[] buffer512);
        [DllImport("Rockey2.dll")]
        internal static extern int RY2_Write(int handle, int block_index, [MarshalAs(UnmanagedType.LPArray)] char[] buffer512);
        [DllImport("MSVCRT.dll")]
        internal static extern long atol([MarshalAs(UnmanagedType.LPArray)] char[] sTemp);


        int Rockey_find = 0;
        int Rockey_open = 0;
        public int Rockey_find_dongle;

        public Rockey2()
        {
            // Rockey2 동글이 연결된 개수를 반환해 줍니다.
            Rockey_find_dongle = RY2_Find();
            //Debug.Print(Rockey_find.ToString());
           
            //Debug.Print($"HID output from rockey open: {Convert.ToString(Rockey_opening(), 16)}");

            //Debug.Print($"Gen UID result : {Rockey_generating()}");
        }

        public uint Rockey_opening()
        {
            uint handle = 0;
            uint uid = 0x0000abcd; // 여기에 실제 UID를 설정하세요.
            uint hid = 0x00000000; // 여기에 실제 HID를 설정하세요.

            // Rockey2를 열고 UID 및 HID를 사용하여 장치를 찾습니다.
            int status = RY2_Open(0, uid, out uint ohid);
            //Debug.Print($"Rockey open result : {status.ToString()}");

            return ohid;
        }

        public int Rockey_generating()
        {
            //0134 - 1149156542
            //013a - 3515036914
            char[] seedcode = new char[4] { '0', '1', '3', '4'};
            int result = int.MaxValue;
            result = RY2_GenUID(0, out uint Nuid, seedcode, 0);
            //Debug.Print($"Nuid: {Nuid}");

            return result;
        }

        

        
    }
}
