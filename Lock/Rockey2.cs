using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
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


        int Rockey_open = 0;
        public int Rockey_find_dongle;
        private uint uid = 3365499556; // 여기에 실제 UID를 설정하세요.

        public Rockey2()
        {
            // Rockey2 동글이 연결된 개수를 반환해 줍니다.
            Rockey_find_dongle = RY2_Find();
            //Debug.Print(Rockey_find_dongle.ToString());

            //Debug.Print($"HID output from rockey open: {Convert.ToString(Rockey_opening())}");

            //Debug.Print($"Gen UID result : {Rockey_generating()}");

            if (Rockey_find_dongle != 1)
            {
                MessageBox.Show("License가 없습니다.", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
            }
            else
            {
                if (Rockey_opening(uid) != 0)
                {
                    MessageBox.Show("License가 적절하지 않습니다.", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(0);
                }
                
            }
            Rockey_Read();
        }

        public int Rockey_opening(uint uid)
        {
            uint handle = 0;

            uint hid = 2148170306; // 여기에 실제 HID를 설정하세요.

            // hid 2148170306
            // Rockey2를 열고 UID 및 HID를 사용하여 장치를 찾습니다.
            int status = RY2_Open(1, uid, out uint ohid);
            //Debug.Print($"Rockey open result : {status.ToString()}");

            return status;
        }

        public int Rockey_generating()
        {
            //0134 - 1149156542
            //013a - 3515036914
            //0130 - 3365499556
            char[] seedcode = new char[4] { '0', '1', '3', '4' };
            int result = int.MaxValue;
            result = RY2_GenUID(0, out uint Nuid, seedcode, 0);
            Debug.Print($"Nuid: {Nuid}");

            return result;
        }

        public void Rockey_Closing()
        {
            RY2_Close(0);
        }

        public void Rockey_Read()
        {
            char[] buffer = new char[512];
            for (int i = 0; i < 4; i++)
            {
                Debug.Print(RY2_Read(0, i, buffer).ToString());
                string strvalue = new string(buffer);
                Debug.Print(strvalue);
            }
            
        }

        

        
    }
}
