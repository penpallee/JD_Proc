using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;

namespace JD_Proc.TempController
{
    class SerialCommunication
    {
        public ArrayList Serial_Buf_Arr = new ArrayList();
        private SerialPort Sp = new SerialPort();

        //--------------------------------------------------------
        //        Comport 열기        
        //--------------------------------------------------------
        public bool Comport_Open(string port)
        {
            try
            {
                Sp.PortName = port;
                Sp.BaudRate = 9600;
                Sp.DataBits = 8;
                Sp.Parity = Parity.None;
                Sp.StopBits = StopBits.Two;

                if (!Sp.IsOpen)
                {
                    Sp.Open();
                }
                if (Sp.IsOpen)
                {
                    return true;
                }

                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--------------------------------------------------------
        //      수신 처리          
        //--------------------------------------------------------
        public void Reference(int number)
        {
            byte[] buf = new byte[number];
        }

        public string Response_String;
        public byte[] Response;
        public void RcvSerialComm()
        {

            try
            {
                if (Sp.IsOpen)
                {
                    int nbyte = Sp.BytesToRead;
                    byte[] rbuff = new byte[nbyte];
                    if (nbyte > 0)
                    {
                        Sp.Read(rbuff, 0, nbyte);
                    }
                    Response = rbuff;
                    Response_String = BitConverter.ToString(rbuff);

                    for (int i = 0; i < nbyte; i++)
                    {
                        Serial_Buf_Arr.Add(rbuff[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Serial_Buf_Arr.Clear();
                throw ex;
            }
        }

        //--------------------------------------------------------
        //      포트 상태 체크         
        //--------------------------------------------------------
        public bool IsOpened()
        {
            return Sp.IsOpen;
        }

        //--------------------------------------------------------
        //      수신 개수 읽기         
        //--------------------------------------------------------
        public int RcvCnt()
        {

            return Serial_Buf_Arr.Count;

        }

        //--------------------------------------------------------
        //     수신버퍼 클리어         
        //--------------------------------------------------------
        public void RcvBuffClear()
        {

            Serial_Buf_Arr.Clear();
        }

        //--------------------------------------------------------
        //    송신 처리         
        //--------------------------------------------------------
        public void SendSerialComm(byte[] SendComm_Packet, int len)
        {
            try
            {
                if (Sp.IsOpen)
                    Sp.Write(SendComm_Packet, 0, len);
                //Console.WriteLine("Sending Completed");
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--------------------------------------------------------
        //    포트 닫기         
        //--------------------------------------------------------
        public void CloseSerialComm()

        {
            try
            {
                if (Sp != null)
                    Sp.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--------------------------------------------------------
        //    시리얼포트 해제         
        //--------------------------------------------------------
        public void Dispose()
        {
            if (Serial_Buf_Arr.Count > 0)
            {
                Serial_Buf_Arr.Clear();
            }
            if (Sp != null)
                Sp.Dispose();
        }

        public string GetCurrentTemperature(SerialCommunication GoSerial)
        {
            //--------------------현재 값 표시
            byte[] Query_con_p = new byte[8] { 0x01, 0x04, 0x03, 0xE8, 0x00, 0x01, 0xB1, 0xBA };
            GoSerial.SendSerialComm(Query_con_p, Query_con_p.Length);
            GoSerial.RcvSerialComm();
            string Bytecnt_dec_p = Convert.ToInt32(GoSerial.Response[2]).ToString();
            int Bytecnt_num_p = int.Parse(Bytecnt_dec_p);
            byte[] PV_arr = new byte[Bytecnt_num_p];
            Array.Copy(GoSerial.Response, 3, PV_arr, 0, Bytecnt_num_p);
            string PV_arr_str = BitConverter.ToString(PV_arr).Replace("-", "");
            string PV_dec = Convert.ToInt32(PV_arr_str, 16).ToString();
            Debug.WriteLine("현재 값(PV) : " + PV_dec);
            return PV_dec;
        }

        public string GetCurrentTargetTemperature(SerialCommunication GoSerial)
        {
            //--------------------설정 값 표시
            byte[] Query_con_s = new byte[8] { 0x01, 0x04, 0x03, 0xEB, 0x00, 0x01, 0x41, 0xBA };
            GoSerial.SendSerialComm(Query_con_s, Query_con_s.Length);
            GoSerial.RcvSerialComm();
            string Bytecnt_dec_s = Convert.ToInt32(GoSerial.Response[2]).ToString();
            int Bytecnt_num_s = int.Parse(Bytecnt_dec_s);
            byte[] SV_arr = new byte[Bytecnt_num_s];
            Array.Copy(GoSerial.Response, 3, SV_arr, 0, Bytecnt_num_s);
            string SV_arr_str = BitConverter.ToString(SV_arr).Replace("-", "");
            string SV_dec = Convert.ToInt32(SV_arr_str, 16).ToString();
            Debug.WriteLine("설정 값(SV) : " + SV_dec);

            return SV_dec;
            //--------------------설정 값 표시
        }

        public void SetTargetTemperature(SerialCommunication GoSerial, int SV_Value)
        {

            int SV_Dec;
            SV_Dec = SV_Value;
            byte[] SV_byte = BitConverter.GetBytes(SV_Dec);
            byte[] ForCRC = { 0x01, 0x06, 0x00, 0x00, SV_byte[1], SV_byte[0] };
            byte[] CRC_Cal = BitConverter.GetBytes(CRC16_GEN.ComputeChecksum(ForCRC));
            byte[] Query = new byte[8] { 0x01, 0x06, 0x00, 0x00, SV_byte[1], SV_byte[0], CRC_Cal[0], CRC_Cal[1]};
            string Query_Check = BitConverter.ToString(Query).Replace("-", " ");
            Console.WriteLine(Query_Check);
            GoSerial.SendSerialComm(Query, Query.Length);
            GoSerial.RcvSerialComm();
            if (GoSerial.Response[1] == 0x86)
            {
                MessageBox.Show("Temperature Controller Set Target Temp Error");
            }
        }
    }
}
