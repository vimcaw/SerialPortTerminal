using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortTerminal
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxSerialPort.DataSource = SerialPortInfo.GetAllCOMs();
            comboBoxParity.SelectedIndex = 0;
            comboBoxStopBits.SelectedIndex = 1;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            comboBoxSerialPort.DataSource = SerialPortInfo.GetAllCOMs();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (buttonSend.Text == "打开")
            {
                radioButtonOpen.PerformClick();
            }
            else if (checkBoxAutoEnter.Checked)
            {
                serialPort.WriteLine(textBoxSender.Text);
            }
            else
            {
                serialPort.Write(textBoxSender.Text);
            }
        }

        private void radioButtonOpen_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                serialPort.Open();
                buttonSend.Text = "发送";
                toolStripStatusLabelSerialPortStatus.Text = "OPENED";
                toolStripStatusLabelSerialPortStatus.ForeColor = Color.Green;
            }
            catch
            {
                radioButtonClose.PerformClick();
                MessageBox.Show("无法打开串口，请检查串口连接是否正常、串口是否被其它程序占用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonClose_CheckedChanged(object sender, EventArgs e)
        {
            serialPort.Close();
            buttonSend.Text = "打开";
            toolStripStatusLabelSerialPortStatus.Text = "CLOSED";
            toolStripStatusLabelSerialPortStatus.ForeColor = Color.Red;
        }

        private void comboBoxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                radioButtonClose.PerformClick();
            }
            serialPort.PortName = Regex.Match(comboBoxSerialPort.SelectedItem.ToString(), @"COM[0-9]+").Groups[0].Value;
            toolStripStatusLabelSerialPortName.Text = comboBoxSerialPort.SelectedItem.ToString();
        }

        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                radioButtonClose.PerformClick();
            }
            serialPort.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            textBoxReceiver.AppendText(serialPort.ReadLine() + "\n");
        }

        private void textBoxReceiver_TextChanged(object sender, EventArgs e)
        {
            textBoxReceiver.Select(textBoxReceiver.TextLength, 0);
            textBoxReceiver.ScrollToCaret();
        }

        private void checkBoxCloseWhenBlur_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (checkBoxCloseWhenBlur.Checked)
            {
                radioButtonClose.PerformClick();
            }
        }

        private void comboBoxDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                radioButtonClose.PerformClick();
            }
            serialPort.DataBits = int.Parse(sender.ToString());
        }

        private void comboBoxStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopBits stopBits;
            
            switch (sender.ToString())
            {
                case "None": stopBits = StopBits.None; break;
                case "1": stopBits = StopBits.One; break;
                case "1.5": stopBits = StopBits.OnePointFive; break;
                case "2": stopBits = StopBits.Two; break;
                default: stopBits = StopBits.One; break;
            }

            if (serialPort.IsOpen)
            {
                radioButtonClose.PerformClick();
            }

            serialPort.StopBits = stopBits;
        }

        private void comboBoxParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parity parity;

            switch (sender)
            {
                case "None": parity = Parity.None; break;
                case "Even": parity = Parity.Even; break;
                case "Mark": parity = Parity.Mark; break;
                case "Odd": parity = Parity.Odd; break;
                case "Space": parity = Parity.Space; break;
                default: parity = Parity.None; break;
            }

            serialPort.Parity = parity;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxReceiver.Clear();
        }
    }
}
