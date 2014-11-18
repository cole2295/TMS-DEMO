using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace Vancl.WuliuSys.ClientLib.WeighComp
{

    public partial class WeighComponent : Component
    {
        // 创建一个委托，返回类型为void，两个参数
        public delegate void WeighHandler(object sender, WeighEventArgs e);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event WeighHandler onWeighChange;

        private string _portName;
        private DataMode CurrentDataMode;
        private bool isRecieveData = false;
        private int sendCount = 0;
        private string weight = "0";

        private string PortName
        {
            set { _portName = value; }
            get
            {
                if (string.IsNullOrEmpty(_portName))
                    _portName = WeighSettings.Default.PortName;
                return _portName;
            }
        }


        public WeighComponent()
        {
            InitializeComponent();
        }

        public WeighComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 当前重量
        /// </summary>
        public decimal CurrentWeight
        {
            get
            {
                decimal value;
                if (!decimal.TryParse(weight, out value))
                {
                    value = 0;
                    LastError = new Exception(string.Format("不能将接收到的【{0}】转化为重量！", weight));
                }
                return value;
            }
        }
        /// <summary>
        /// 最后异常错误
        /// </summary>
        public Exception LastError { get; set; }

        public WeighStatus Status { get; set; }

        //private void SetComPort(string portName)
        //{
        //    PortName = portName;
        //}

        /// <summary>
        /// 打开串口，开始读数
        /// </summary>
        public bool Start()
        {
            // Set the port's settings
            CurrentDataMode = DataMode.Hex;
            comport.BaudRate = WeighSettings.Default.BaudRate;
            comport.DataBits = WeighSettings.Default.DataBits;
            comport.StopBits = WeighSettings.Default.StopBits;
            comport.Parity = WeighSettings.Default.Parity;
            comport.PortName = PortName;

            try
            {
                // Open the port
                comport.Open();
                this.Status = WeighStatus.Running;
            }
            catch (Exception e)
            {
                Stop();
                LastError = e;
                this.Status = WeighStatus.Stopped;
                return false;
            }

            if (this.Status == WeighStatus.Running)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    while (true)
                    {
                        if (this.Status == WeighStatus.Running)
                        {
                            sendData();
                        }
                        else
                        {
                            comport.Close();
                            break;
                        }
                        Thread.Sleep(100);
                    }
                });
            }
            return true;
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public bool Stop()
        {
            this.Status = WeighStatus.Stopped;
            weight = "0";
            return true;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        private void sendData()
        {
            // true时重置
            if (isRecieveData)
            {
                sendCount = 0;
                isRecieveData = false;
            }

            // 20次接收不到表示串口有问题
            if (++sendCount == 20)
            {
                LastError = new Exception(string.Format("获取重量失败，请检查电子称和电脑是否连接正确，电子称是否已打开"));
            }

            // 发送代码0512
            string command = "0512";

            // 模式
            if (CurrentDataMode == DataMode.Text)
            {
                // Send the user's text straight out the port
                //txtSendData.Text = Keys.Control.ToString() + Keys.E.ToString() + Keys.R.ToString();
                comport.Write(command);
            }
            else
            {
                try
                {
                    // Convert the user's string of hex digits (ex: B4 CA E2) to a byte array
                    byte[] data = HexStringToByteArray(command);

                    // Send the binary data out the port
                    comport.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    LastError = e;
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {   //Console.WriteLine(string.Format("received.{0}", ++i));
            if (comport.IsOpen && this.Status == WeighStatus.Running)
            {
                string data = comport.ReadExisting();
                string pattern = @"\d{1,}\.\d{1,}";
                // Display the text to the user in the terminal
                data = Regex.Match(data, pattern, RegexOptions.Compiled).Value;
                if (!string.IsNullOrEmpty(data))
                {
                    // 获取重量
                    weight = data;
                    isRecieveData = true;
                }
            }
            else
            {
                weight = "0";
            }
            if (onWeighChange != null)
            {
                WeighEventArgs args = new WeighEventArgs { LastError = LastError, Status = this.Status, Weight = this.CurrentWeight, };
                onWeighChange(this, args);
            }
        }

        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            var buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        private string ByteArrayToHexString(byte[] data)
        {
            var sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

    }


    public enum DataMode
    {
        Text,
        Hex
    }

    public enum LogMsgType
    {
        Incoming,
        Outgoing,
        Normal,
        Warning,
        Error
    } ;

    /// <summary>
    /// 运行状态
    /// </summary>
    public enum WeighStatus
    {
        /// <summary>
        /// 已停止
        /// </summary>
        Stopped,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
    }

    public class WeighEventArgs : EventArgs
    {
        public Decimal Weight { get; set; }
        public WeighStatus Status { get; set; }
        public Exception LastError { get; set; }
    }
}
