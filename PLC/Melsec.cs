using ActUtlType64Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JD_Proc.PLC
{
    #region [ Enum ]
    public enum MelsecTypeNumber
    {
        None = 0, Bit = 1, Word = 2, DoubleWord = 3
    }

    /// <summary>
    /// - 비트 단위 라벨링
    ///     X와 Y는 입력
    ///     M과 L은 내부 접점
    ///     F와 V는 내부 기억 장치
    ///     B와 SB는 내부 비트
    ///     DX와 DY는 직접 주소 지정
    /// </summary>
    public enum MelsecBitAddress
    {
        X, Y, M, L, F, V, B, SB, DX, DY, SM
    }

    /// <summary>
    /// - 워드 단위 라벨링
    ///     D와 W는 데이터
    ///     R과 ZR은 파일 레지스터
    ///     SD와 SW는 스트레치 레지스터
    ///     TD와 TW는 타이머
    /// </summary>
    public enum MelsecWordAddress
    {
        D, W, R, ZR, SD, SW, TD, TW
    }

    /// <summary>
    /// - 더블워드 단위 라벨링
    ///     LD는 더블워드 레지스터
    ///     CN
    ///     FD
    /// </summary>
    public enum MelsecDoubleWordAddress
    {
        LD, CN, FD
    }

    public enum ConvertType
    {
        Binary, Hex, ASCII
    }
    #endregion

    public class Melsec
    {
        #region var
        public ActUtlType64Lib.ActUtlType64 actUtlType64;
        int isConnect = -1;
        #endregion

        #region 생성자
        public Melsec(int actLogicalStationNumber)
        {
            actUtlType64 = new ActUtlType64Lib.ActUtlType64();
            actUtlType64.ActLogicalStationNumber = actLogicalStationNumber;
           
        }
        #endregion

        #region conncet / open
        /// <summary>
        /// PC <-> PLC 연결상태를 반환
        /// </summary>
        /// <returns>연결상태(true = 연결중, false = 연결중이 아님)</returns>
        public bool IsConnected()
        {

            if (isConnect == 0)
                return true;
            else
                return false;
        }

        public void Open()
        {
            isConnect = actUtlType64.Open();
        }

        #endregion

        #region Read Ascii 
        public bool ReadDeviceBlock(string _sAdd, out string _str)
        {
            short[] sInt = new short[10];
            Array.Clear(sInt, 0, sInt.Length);
            bool bCheck = ReadDeviceBlock2(_sAdd, sInt.Length, out sInt[0]);

            _str = "";
            if (bCheck)
            {
                for (int i = 0; i < sInt.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(sInt[i]);
                    _str += Encoding.Default.GetString(bytes);
                }
            }

            return bCheck;

            //_str = "" ;
            //Array.Clear(sInt, 0, sInt.Length);
            //bool bCheck = ReadDeviceBlock2(_sAdd, sInt.Length,out sInt[0]);
            //for (int i = 0; i < sInt.Length; i++)
            //{
            //    if(sInt[i] != 0) _str += Convert.ToChar(sInt[i]);
            //}
            //return bCheck ;
        }

        public bool ReadDeviceBlock2(string szDevice, int iSize, out short lplData)
        {
            lplData = 0;

            int iRst = -1;

            iRst = actUtlType64.ReadDeviceBlock2(szDevice, iSize, out lplData);

            return false;
        }
        #endregion

        #region write ascii 
        public bool WriteDeviceBlock(string _sAdd, string _str)
        {
            short[] sInt = new short[10];
            if (_str == "")
            {

                Array.Clear(sInt, 0, sInt.Length);
                bool bCheck1 = WriteDeviceBlock2(_sAdd, sInt.Length, ref sInt[0]);
                return bCheck1;
            }
            //if(!_str.Contains(Option.Save_Path)) return false ;
            //
            ////
            ////string sStr = @"D:\DWAMSaveImage\2020Y\12M\17D\131511_K30" ;
            //int    iStt = _str.IndexOf(Option.Save_Path) ;
            //int    iLen = Option.Save_Path.Length;
            //string sStr = _str.Substring(iStt + iLen,20);

            string str = _str;
            string[] str_temp;

            if (str.Length % 2 == 0)
            {
                str_temp = new string[str.Length / 2];
                sInt = new short[str_temp.Length];

                for (int i = 0; i < str.Length / 2; i++)
                {
                    str_temp[i] = str.Substring(i * 2, 2);
                }

                for (int i = 0; i < str_temp.Length; i++)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(str_temp[i]);
                    short sh = BitConverter.ToInt16(bytes, 0);
                    sInt[i] = sh;
                }
            }
            else
            {
                str_temp = new string[(str.Length / 2) + 1];
                sInt = new short[str_temp.Length];

                for (int i = 0; i < str.Length / 2 + 1; i++)
                {
                    if (i < (str.Length - 1) / 2)
                        str_temp[i] = str.Substring(i * 2, 2);
                    else
                        str_temp[i] = str.Substring(i * 2, 1);
                }

                for (int i = 0; i < str_temp.Length; i++)
                {
                    if (i < str_temp.Length - 1)
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(str_temp[i]);
                        short sh = BitConverter.ToInt16(bytes, 0);
                        sInt[i] = sh;
                    }
                    else
                    {
                        char data = Convert.ToChar(str_temp[i].Substring(0, 1));
                        sInt[i] = (short)data;
                    }
                }
            }

            bool bCheck2 = WriteDeviceBlock2(_sAdd, sInt.Length, ref sInt[0]);
            return bCheck2;
        }

        public bool WriteDeviceBlock2(string szDevice, int iSize, ref short iData)
        {
            int iRst = -1;

            iRst = actUtlType64.WriteDeviceBlock2(szDevice, iSize, ref iData);

            return false;
        }
        #endregion

        #region 메모리 주소의 타입을 반환
        /// <summary>
        /// 메모리 주소의 타입을 반환
        /// </summary>
        /// <param name="device">메모리 주소</param>
        /// <returns>MelsecTypeNumber 반환</returns>
        private MelsecTypeNumber CheckAddressType(string device)
        {
            string check = device.Substring(1, 2);
            int temp = 0;
            string type = string.Empty;
            if (int.TryParse(check, out temp))
            {
                type = device.Substring(0, 1);
            }
            else
            {
                type = device.Substring(0, 2);
            }

            MelsecTypeNumber result = MelsecTypeNumber.None;

            if (Enum.IsDefined(typeof(MelsecBitAddress), type))
            {
                result = MelsecTypeNumber.Bit;
            }
            else if (Enum.IsDefined(typeof(MelsecWordAddress), type))
            {
                result = MelsecTypeNumber.Word;
            }
            else if (Enum.IsDefined(typeof(MelsecDoubleWordAddress), type))
            {
                result = MelsecTypeNumber.DoubleWord;
            }

            return result;
        }
        #endregion

        #region Melsec으로부터 읽어온 Int 데이터를 binary, hex, ascii로 변환하여 반환
        /// <summary>
        /// Melsec으로부터 읽어온 Int 데이터를 binary, hex, ascii로 변환하여 반환
        /// </summary>
        /// <param name="data">int 데이터</param>
        /// <param name="convertType">반환 데이터</param>
        public string DataConvert(int data, ConvertType convertType)
        {
            Console.WriteLine($"OriginData - {data}");
            switch (convertType)
            {
                case ConvertType.Binary:
                    string binary = Convert.ToString(data, 2);
                    Console.WriteLine($"Binary - {binary}");
                    return binary;

                case ConvertType.Hex:
                    string hex = Convert.ToString(data, 16);
                    Console.WriteLine($"Hex - {hex}");
                    return hex;

                case ConvertType.ASCII:
                    string ascii = Encoding.ASCII.GetString(BitConverter.GetBytes(data));
                    Console.WriteLine($"ASCII - {ascii}");
                    return ascii;
            }
            return String.Empty;
        }
        #endregion

        #region Byte[]을 Short[]로 변환
        /// <summary>
        /// Byte[]을 Short[]로 변환
        /// </summary>
        /// <param name="byteArray">byte[] 데이터</param>
        /// <returns>short[] 데이터</returns>
        public short[] ByteToShort(byte[] byteArray)
        {
            short[] shortArray = new short[byteArray.Length / 2];

            for (int i = 0; i < shortArray.Length; i++)
            {
                shortArray[i] = BitConverter.ToInt16(byteArray, i * 2);
            }

            return shortArray;
        }
        #endregion

        #region Short[]을  Byte[]로 변환
        /// <summary>
        /// Short[]을  Byte[]로 변환
        /// </summary>
        /// <param name="shortArray">short[] 데이터</param>
        /// <returns>byte[] 데이터</returns>
        public byte[] ShortToByte(short[] shortArray)
        {
            byte[] byteArray = new byte[shortArray.Length * 2];

            Buffer.BlockCopy(shortArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }
        #endregion

        #region [ACK Response]
        public byte[] PLC_ACK_Response()
        {
            byte[] byteArray = new byte[8];
            return byteArray;
        }
        #endregion
    }
}
