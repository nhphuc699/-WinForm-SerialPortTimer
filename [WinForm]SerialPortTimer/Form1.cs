using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace _WinForm_SerialPortTimer
{
    public partial class Form1 : Form
    {
        private SerialPort sPort;

        public Form1()
        {
            InitializeComponent();
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
            sPort = new SerialPort();
            sPort.BaudRate = 9600;
            sPort.Parity = Parity.None;
            sPort.DataBits = 8;
            sPort.StopBits = StopBits.One;
            comboBox2.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                sPort.PortName = comboBox1.Text;

                sPort.DtrEnable = true;
                sPort.RtsEnable = true;
                sPort.Open();
                if (checkBox1.Checked)
                {
                    sPort.DataReceived += SPort_DataReceived;
                }
                else
                {
                    richTextBox1.AppendText(string.Format("[{0}][Connected] {1}{2}", DateTime.Now, comboBox1.Text, Environment.NewLine));
                    timer.Interval = int.Parse(textBox2.Text);
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(string.Format("[{0}][Error] {1}{2}", DateTime.Now, ex.Message, Environment.NewLine));
            }
        }

        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //getData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //sPort.DataReceived -= SPort_DataReceived;
                sPort.Close();
                timer.Enabled = false;
                sPort.DataReceived -= SPort_DataReceived;
                richTextBox1.AppendText(string.Format("[{0}][DisConnected] {1}{2}", DateTime.Now, comboBox1.Text, Environment.NewLine));
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(string.Format("[{0}][Error] {1}{2}", DateTime.Now, ex.Message, Environment.NewLine));
            }
        }

        private int countBytesToReadLength = 0;

        private void getData()
        {
            try
            {
                int bytes = sPort.BytesToRead;
                if (bytes > 0)
                {
                    byte[] buffer = new byte[bytes];
                    sPort.Read(buffer, 0, bytes);
                    //string s = BitConverter.ToString(buffer);
                    var s = Encoding.ASCII.GetString(buffer).Trim();

                    //var s = BitConverter.ToInt32(buffer, 0).ToString(); //getbuffer);
                    //textBox1.Text = s.Length.ToString() + "*" + s + "*";
                    if (comboBox2.SelectedIndex == 0)
                        if (s.Length == 6)
                        {
                            textBox1.Text = double.Parse(s).ToString();
                            countBytesToReadLength = 0;
                        }
                        else
                        {
                            countBytesToReadLength++;
                        }
                    if(comboBox2.SelectedIndex ==1)
                        if (s.Length >= 7)
                        {
                            s = s.Replace("w", string.Empty).Replace("n", string.Empty).Replace("k", string.Empty).Replace("g", string.Empty);
                            if (s.Length == 7)
                            {
                                textBox1.Text = double.Parse(s).ToString();
                                countBytesToReadLength = 0;
                            }
                            else
                            {
                                countBytesToReadLength++;
                            }
                        }
                        else
                        {
                            countBytesToReadLength++;
                        }
                    if (checkBox2.Checked)
                        richTextBox1.AppendText(bytes.ToString() + "-" + buffer.Length.ToString() + "-" + s.Length.ToString() + "=>" + s.ToString() + "<=" + Environment.NewLine);
                }
                else
                {
                    countBytesToReadLength++;
                }
                if (countBytesToReadLength >= int.Parse(textBox3.Text))
                {
                    textBox1.Text = "error";
                }
                label1.Text = countBytesToReadLength.ToString();
                if (countBytesToReadLength >= int.Parse(label2.Text))
                {
                    label2.Text = countBytesToReadLength.ToString();
                }
                //double TrongLuong = 0;
                //string data = sPort.ReadExisting();
                //richTextBox1.AppendText(data.Trim() + Environment.NewLine);
                //if (data.Length > 0)
                //{
                //    textBox1.Text = data.Length.ToString() + "*" + data.Trim() + "*";
                //}
                //data = data.Remove(0, 1);
                //if (data[0] == '+' || data[0] == '-')
                //{
                //    if (char.IsNumber(data[1]) && char.IsNumber(data[2]) && char.IsNumber(data[3]) && char.IsNumber(data[4]) && char.IsNumber(data[5]) && char.IsNumber(data[6]))
                //    {
                //        if (data[8] == '1')
                //        {
                //            if (data[0] == '-')
                //            {
                //                char[] c = { data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
                //                TrongLuong = Math.Round(double.Parse(new string(c)) / 1000, 3);
                //            }
                //            else
                //            {
                //                char[] c = { data[1], data[2], data[3], data[4], data[5], data[6] };
                //                TrongLuong = Math.Round(double.Parse(new string(c)) / 1000, 3);
                //            }
                //        }
                //    }
                //    textBox1.Text = TrongLuong.ToString();
                //}

                //StringBuilder rl = new StringBuilder(bytes * 2);
                //string HexAlphabet = "1234567890ABCDEF";
                //foreach(byte b in buffer)
                //{
                //    rl.Append(HexAlphabet[(int)(b >> 4)]);
                //    rl.Append(HexAlphabet[(int)(b & 0xF)]);
                //}
                //richTextBox1.AppendText(rl.ToString() + Environment.NewLine);

                //string s = sPort.ReadExisting();
                //richTextBox1.AppendText(((int)s[1]).ToString() +" "+ ((int)s[2]).ToString() + " " + ((int)s[3]).ToString() + " " + ((int)s[4]).ToString() + " " + ((int)s[5]).ToString() + " " + ((int)s[6]).ToString() + " " + ((int)s[7]).ToString() + " " + ((int)s[8]).ToString() + " " + ((int)s[9]).ToString() + " " + ((int)s[11]).ToString() + " " + ((int)s[10]).ToString() + Environment.NewLine);
                //if (s[0] == '#' && s[10] == '*')
                //{
                //    if ((int)s[8] == 1)
                //    {
                //        CongTac1 = "1";
                //        CongTac2 = "0";
                //        DangCanBon = "1";
                //    }
                //    if ((int)s[9] == 1)
                //    {
                //        CongTac1 = "0";
                //        CongTac2 = "1";
                //        DangCanBon = "2";
                //    }
                //    txtTinHieuMain.Text = ((int)s[1]).ToString() + ((int)s[2]).ToString() + ((int)s[3]).ToString() + ((int)s[4]).ToString() + ((int)s[5]).ToString() + ((int)s[6]).ToString() + ((int)s[7]).ToString() + ((int)s[8]).ToString() + ((int)s[9]).ToString();
                //    EpMain();
                //    await Task.Delay(10);
                //}
                //sPort.Write("#");
                //if (test[1] == '=')
                //{
                //   // double num = 0;
                //    char[] c = new char[] { test[6], test[7], test[8], test[9] };
                //    if (">;:73".IndexOf(test[2]) != -1)
                //        textBox1.Text = (-1* double.Parse(0 + new string(c))).ToString();
                //    else
                //        textBox1.Text = (double.Parse(0 + new string(c))).ToString();
                //    //textBox1.Text = new string(c);
                //}
                //richTextBox1.AppendText(test + Environment.NewLine);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message + Environment.NewLine);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //string test;
            //test = sPort.ReadLine();
            getData();
            //try
            //{
            //    int bytes = sPort.BytesToRead;
            //    byte[] buffer = new byte[bytes];
            //    sPort.Read(buffer, 0, bytes);
            //    //string s = BitConverter.ToString(buffer);
            //    var s = Encoding.ASCII.GetString(buffer).Trim();

            //    //var s = BitConverter.ToInt32(buffer, 0).ToString(); //getbuffer);
            //    //textBox1.Text = s.Length.ToString() + "*" + s + "*";
            //    if (s.Length == 6)
            //    {
            //        textBox1.Text = double.Parse(s).ToString();
            //    }
            //    richTextBox1.AppendText(s.Length.ToString()+ "=>" + s.ToString()+"<=" + Environment.NewLine);

            //    //double TrongLuong = 0;
            //    //string data = sPort.ReadExisting();
            //    //richTextBox1.AppendText(data.Trim() + Environment.NewLine);
            //    //if (data.Length > 0)
            //    //{
            //    //    textBox1.Text = data.Length.ToString() + "*" + data.Trim() + "*";
            //    //}
            //    //data = data.Remove(0, 1);
            //    //if (data[0] == '+' || data[0] == '-')
            //    //{
            //    //    if (char.IsNumber(data[1]) && char.IsNumber(data[2]) && char.IsNumber(data[3]) && char.IsNumber(data[4]) && char.IsNumber(data[5]) && char.IsNumber(data[6]))
            //    //    {
            //    //        if (data[8] == '1')
            //    //        {
            //    //            if (data[0] == '-')
            //    //            {
            //    //                char[] c = { data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            //    //                TrongLuong = Math.Round(double.Parse(new string(c)) / 1000, 3);
            //    //            }
            //    //            else
            //    //            {
            //    //                char[] c = { data[1], data[2], data[3], data[4], data[5], data[6] };
            //    //                TrongLuong = Math.Round(double.Parse(new string(c)) / 1000, 3);
            //    //            }
            //    //        }
            //    //    }
            //    //    textBox1.Text = TrongLuong.ToString();
            //    //}

            //    //StringBuilder rl = new StringBuilder(bytes * 2);
            //    //string HexAlphabet = "1234567890ABCDEF";
            //    //foreach(byte b in buffer)
            //    //{
            //    //    rl.Append(HexAlphabet[(int)(b >> 4)]);
            //    //    rl.Append(HexAlphabet[(int)(b & 0xF)]);
            //    //}
            //    //richTextBox1.AppendText(rl.ToString() + Environment.NewLine);

            //    //string s = sPort.ReadExisting();
            //    //richTextBox1.AppendText(((int)s[1]).ToString() +" "+ ((int)s[2]).ToString() + " " + ((int)s[3]).ToString() + " " + ((int)s[4]).ToString() + " " + ((int)s[5]).ToString() + " " + ((int)s[6]).ToString() + " " + ((int)s[7]).ToString() + " " + ((int)s[8]).ToString() + " " + ((int)s[9]).ToString() + " " + ((int)s[11]).ToString() + " " + ((int)s[10]).ToString() + Environment.NewLine);
            //    //if (s[0] == '#' && s[10] == '*')
            //    //{
            //    //    if ((int)s[8] == 1)
            //    //    {
            //    //        CongTac1 = "1";
            //    //        CongTac2 = "0";
            //    //        DangCanBon = "1";
            //    //    }
            //    //    if ((int)s[9] == 1)
            //    //    {
            //    //        CongTac1 = "0";
            //    //        CongTac2 = "1";
            //    //        DangCanBon = "2";
            //    //    }
            //    //    txtTinHieuMain.Text = ((int)s[1]).ToString() + ((int)s[2]).ToString() + ((int)s[3]).ToString() + ((int)s[4]).ToString() + ((int)s[5]).ToString() + ((int)s[6]).ToString() + ((int)s[7]).ToString() + ((int)s[8]).ToString() + ((int)s[9]).ToString();
            //    //    EpMain();
            //    //    await Task.Delay(10);
            //    //}
            //    //sPort.Write("#");
            //    //if (test[1] == '=')
            //    //{
            //    //   // double num = 0;
            //    //    char[] c = new char[] { test[6], test[7], test[8], test[9] };
            //    //    if (">;:73".IndexOf(test[2]) != -1)
            //    //        textBox1.Text = (-1* double.Parse(0 + new string(c))).ToString();
            //    //    else
            //    //        textBox1.Text = (double.Parse(0 + new string(c))).ToString();
            //    //    //textBox1.Text = new string(c);
            //    //}
            //    //richTextBox1.AppendText(test + Environment.NewLine);
            //}
            //catch (Exception ex)
            //{
            //    richTextBox1.AppendText(ex.Message + Environment.NewLine);
            //}
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            label2.Text = "0";
            textBox1.Text = "";
            label1.Text = "0";
            countBytesToReadLength = 0;

        }
    }
}