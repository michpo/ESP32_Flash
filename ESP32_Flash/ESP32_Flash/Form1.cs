using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;
using System.IO.Ports;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Diagnostics;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        USBComList UsbComList;
        //List<string> ComList;
        List<string> PrevComPorts;
        List<string> CurrComPotrs;
        List<string> ConectedPort;
  
        public Form1()
        {
            UsbComList = new USBComList();
            
            


            PrevComPorts = new List<string>();
            CurrComPotrs = new List<string>();
            ConectedPort = new List<string>();

            AddStringsToList(PrevComPorts, SerialPort.GetPortNames());

           InitializeComponent();
        }


        const int WM_DEVICECHANGE = 0x0219;
        const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004; //device was removed
        const int DBT_DEVNODES_CHANGED = 0x0007; //device changed
        //const int DBT_DEVTYP_VOLUME = 0x00000002; // logical volume
        const int DBT_DEVTYP_PORT = 0x0003;

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            int devType;
            //if (m.Msg == WM_DEVICECHANGE && m.WParam.ToInt32() == DBT_DEVICEARRIVAL
            //    || m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE
            //    || m.WParam.ToInt32() == DBT_DEVNODES_CHANGED)
            //{
            if (m.Msg == WM_DEVICECHANGE  && m.WParam.ToInt32() == DBT_DEVICEARRIVAL)
            {
                 devType = Marshal.ReadInt32(m.LParam, 4);

                if (devType == DBT_DEVTYP_PORT)
                {
                    // ComList = UsbComList.GetUsbComListbyVidPid("303A", "1001");
                    var ports = SerialPort.GetPortNames();
                    AddStringsToList(CurrComPotrs, ports);
                    var port = CurrComPotrs.Where(i => !PrevComPorts.Contains(i)).ToList();
                    ConectedPort.Add(port[0]);
                   
                    PrevComPorts.Clear();
                }           
            }
            else if (m.Msg == WM_DEVICECHANGE && m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE)
            {
                devType = Marshal.ReadInt32(m.LParam, 4);
                if (devType == DBT_DEVTYP_PORT)
                {
                    var ports = SerialPort.GetPortNames();
                    PrevComPorts.Clear();

                    AddStringsToList(PrevComPorts, ports);
                
                    CurrComPotrs.Clear();
                }
            }

            base.WndProc(ref m);
        }

        private void CompareLists(string prev, string curr)
        {
           
        }

        private void AddStringsToList(List<string> list ,string[] array)
        {
            foreach (string str in array)
            {
                list.Add(str);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread FindComPortTread = new Thread(new ThreadStart(FindComPort));
            FindComPortTread.Start();
            //LastDevice();
            //ComList = UsbComList.GetUsbComListbyVidPid("303A", "1001");
            //ComList.Add("");
            //ComList = new List<string>();
        }

        private void FindComPort()
        {
            while (true)
            {
                if (ConectedPort.Count() > 0)
                {
                    string comport = UsbComList.FindConectedComPort(ConectedPort.ToArray(), "303A", "1001");

                    this.Invoke((MethodInvoker)delegate () { COM_statusStrip.Items[0].Text = comport;  });
                    ExecuteCommand("", "", comport);

                    ConectedPort.Clear();
                }
                Application.DoEvents(); // Allow the UI to remain responsive
                Thread.Sleep(100);
            }
        }

        public void ExecuteCommand(string filname, string command, string COM_Port)
        {
 
            using (Process sortProcess = new Process())
            {
                sortProcess.StartInfo.FileName = "cmd.exe"; // esptool.exe";
                sortProcess.StartInfo.Arguments = "";// " -p " + COM_Port + " -b 460800 --before=default_reset --after=hard_reset  write_flash --flash_mode dio --flash_freq 80m --flash_size 4MB 0x0 bootloader.bin 0x10000 boot_app.bin 0x8000 partition-table.bin";

                sortProcess.StartInfo.CreateNoWindow = true;
                sortProcess.StartInfo.UseShellExecute = false;
                sortProcess.StartInfo.RedirectStandardOutput = true;
                
                // Set event handler
                sortProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

                try
                {
                    // Start the process.
                    sortProcess.Start();

                    //// Start the asynchronous read
                    sortProcess.BeginOutputReadLine();

                    sortProcess.WaitForExit();
                }
                catch(Exception ex)
                {

                }
                finally
                {
                    sortProcess.Close();
                   
                }
            }

        }

        void SortOutputHandler(object sender, DataReceivedEventArgs e)
        {
           // Trace.WriteLine(e.Data);
            this.BeginInvoke(new MethodInvoker(() =>
            {
                
                if (e.Data !=null)
                {
                    string[] splited = e.Data.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    foreach (string str in splited)
                    {
                        textBox1.AppendText(str + "\r\n");
                    }
                    //textBox1.AppendText(e.Data ?? string.Empty);
                }
            }));
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }

    //class USBDeviceInfo
    //{
    //    public string Name { get; private set; }
    //    public string DeviceID { get; private set; }
    //    public string PnpDeviceID { get; private set; }
    //    public string Description { get; private set; }

    //    public USBDeviceInfo() { }
    //    public USBDeviceInfo(string name, string deviceID, string pnpDeviceID, string description)
    //    {
    //        this.Name = name;
    //        this.DeviceID = deviceID;
    //        this.PnpDeviceID = pnpDeviceID;
    //        this.Description = description;
    //    }


    //    public List<string> GetUSBDevices()
    //    {
    //        List<string> devices = new List<string>();

    //        ManagementObjectCollection collection;
    //        using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity where DeviceID Like ""USB%"""))
    //            collection = searcher.Get();

    //        foreach (var d in collection)
    //        {

    //            //devices.Add(new USBDeviceInfo(
    //            ////(string)device.GetPropertyValue("Name"),
    //            ////(string)device.GetPropertyValue("DeviceID"),
    //            ////(string)device.GetPropertyValue("PNPDeviceID"),
    //            ////(string)device.GetPropertyValue("Description")
    //            //));

    //            devices.Add(d.);

    //        }

    //        collection.Dispose();
    //        return devices;
    //    }
    //}

}

        //try
        //{
        //    ManagementObjectSearcher searcher =
        //        new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSSerial_PortName");
        //    foreach (ManagementObject queryObj in searcher.Get())
        //    {
        //        //If the serial port's instance name contains USB 
        //        //it must be a USB to serial device
        //        if (queryObj["InstanceName"].ToString().Contains("USB"))
        //        {
        //            Console.WriteLine("-----------------------------------");
        //            Console.WriteLine("MSSerial_PortName instance");
        //            Console.WriteLine("-----------------------------------");
        //            Console.WriteLine("InstanceName: {0}", queryObj["InstanceName"]);
        //            Console.WriteLine(queryObj["PortName"] + "is a USB to SERIAL adapter/converter");
        //            string port = queryObj["PortName"].ToString();
        //            SerialPort p = new SerialPort(port);
        //            //p.PortName = "COM50";
        //            return port ;
        //        }
        //    }

        //    throw new Exception(Messages.PINPAD_NOT_FOUND);
        //}

