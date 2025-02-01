using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Management;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace USB_ComList
{

    public class USBComList
    {

        //const int WM_DEVICECHANGE = 0x0219;
        //const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
        //const int DBT_DEVICEREMOVECOMPLETE = 0x8004; //device was removed
        //const int DBT_DEVNODES_CHANGED = 0x0007; //device changed
        ////const int DBT_DEVTYP_VOLUME = 0x00000002; // logical volume

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_DEVICECHANGE
        //        && m.WParam.ToInt32() == DBT_DEVICEARRIVAL
        //        || m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE
        //        || m.WParam.ToInt32() == DBT_DEVNODES_CHANGED)
        //    {
        //        if (m.WParam.ToInt32() != DBT_DEVNODES_CHANGED)
        //        {
        //            //  int devType = Marshal.ReadInt32(m.LParam, 4);
        //            //  if (devType == DBT_DEVTYP_VOLUME)
        //            //  {
        //            //Poll drives
        //            List<string> UsbComList = GetUsbComListbyVidPid("303A", "1001");
        //            //  }
        //        }
        //        else
        //        {
        //            //Poll drives
        //        }
        //    }

        //    base.WndProc(ref m);
        //}

        public List<string> GetUsbComListbyVidPid(string vid, string pid)
        {
            List<string> USBCOMlist = new List<string>();
            try
            {
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                //var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID, Name FROM Win32_PnPEntity WHERE Name LIKE '%COM%'");
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, DeviceID FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //if (queryObj["Caption"] != null && queryObj["Caption"].ToString().Contains("(COM"))
                    //{

                    //   string Caption = queryObj["Caption"].ToString();
                    //    int CaptionIndex = Caption.IndexOf("(COM");
                    //    string CaptionInfo = Caption.Substring(CaptionIndex + 1).TrimEnd(')'); // make the trimming more correct                 

                    string deviceName = queryObj["Name"]?.ToString() ?? "";
                    string deviceId = queryObj["DeviceID"]?.ToString() ?? "";
                    //string ComPort = comFullDeviceName.Substring("(");
                    //  string deviceId = queryObj["deviceid"].ToString(); //"DeviceID"
 

                    string vidPattern = "VID[_&](?<VID>[0-9A-Fa-f]+)";
                    string pidPattern = "PID[_&](?<PID>[0-9A-Fa-f]+)";

                        Match vidMatch = Regex.Match(deviceId, vidPattern);
                        Match pidMatch = Regex.Match(deviceId, pidPattern);


                        if (vidMatch.Success && pidMatch.Success)
                        {
                            string vidCurrent = vidMatch.Groups["VID"].Value;
                            string pidCurrent = pidMatch.Groups["PID"].Value;

                            if (vid == vidCurrent && pid == pidCurrent)
                            {
                                Match match = Regex.Match(deviceName, @"\(COM(\d+)\)");
                                string comPort = "COM" + match.Groups[1].Value;
                                USBCOMlist.Add(comPort);
                            }
                        
                        }

                        // for win10
                        //int vidIndex = deviceId.IndexOf("VID_");
                        //int pidIndex = deviceId.IndexOf("PID_");


                        //if (vidIndex != -1 && pidIndex != -1)
                        //{
                        //    string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"                    
                        //    string vidCurrent = startingAtVid.Substring(0, 4); // vid is four characters long
                        //                                                       //Console.WriteLine("VID: " + vid);
                        //    string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"                    
                        //    string pidCurrent = startingAtPid.Substring(0, 4); // pid is four characters long
                        //    if (vid == vidCurrent && pid == pidCurrent)
                        //    {
                        //        USBCOMlist.Add(CaptionInfo);
                        //    }
                        //}


                    //}
                }
                return USBCOMlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        public string FindConectedComPort(string[] ports, string vid, string pid)
        {
            List<string> ComList = GetUsbComListbyVidPid(vid, pid);
            foreach(string com in ComList)
            { 
                if(com != null)
                {
                    return com;
                }
            }
            return string.Empty;
        }

        string ChangeComPortNumber(string comPortName)
        //  void ChangeComPortNumber(string oldPortNumber, string newPortNumber)
        {

            // Registry key path for COM port settings
            // string keyPath = $@"HKEY_LOCAL_MACHINE\HARDWARE\DEVICEMAP\SERIALCOMM\{comPortName}";

            string keyPath = "HARDWARE\\DEVICEMAP\\SERIALCOMM";//\\Device";//\\USBSER000";
            RegistryKey comKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
            if (comKey != null)
            {
                string[] sub_names = comKey.GetValueNames();
                object obj = comKey.GetValue(sub_names[0]);
                //string name = string.Empty;
                foreach (string name in sub_names)
                {
                    if (comKey.GetValue(name).ToString() == comPortName)
                    {

                        string str = Regex.Match(name, @"\d+").Value;
                        int val = Int32.Parse(str);
                        return name;
                    }

                }

                //comKey.SetValue(sub_names[1], $"COM{newPortNumber}", RegistryValueKind.String);
                comKey.Close();
            }

            return string.Empty;

        }

        public void GetUsbList()
        { 
            List<List<string>> USBCOMlist = new List<List<string>>();
            try
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] != null && queryObj["Caption"].ToString().Contains("(COM"))
                    {
                        List<string> DevInfo = new List<string>();

                        string Caption = queryObj["Caption"].ToString();
                        int CaptionIndex = Caption.IndexOf("(COM");
                        string CaptionInfo = Caption.Substring(CaptionIndex + 1).TrimEnd(')'); // make the trimming more correct                 

                        DevInfo.Add(CaptionInfo);

                        string deviceId = queryObj["deviceid"].ToString(); //"DeviceID"

                        int vidIndex = deviceId.IndexOf("VID_");
                        int pidIndex = deviceId.IndexOf("PID_");
                        string vid = "", pid = "";

                        if (vidIndex != -1 && pidIndex != -1)
                        {
                            string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"                    
                            vid = startingAtVid.Substring(0, 4); // vid is four characters long
                                                                    //Console.WriteLine("VID: " + vid);
                            string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"                    
                            pid = startingAtPid.Substring(0, 4); // pid is four characters long
                            if (vid == "303A" && pid == "1001")
                            {

                                string deviseNum = ChangeComPortNumber(CaptionInfo);
                                // SerialPort p = new SerialPort(CaptionInfo);
                                // p.PortName = "COM50";
                                DevInfo.Add(vid);
                                DevInfo.Add(pid);
                                USBCOMlist.Add(DevInfo);
                                MessageBox.Show(CaptionInfo);
                            }
                        }

                        //DevInfo.Add(vid);
                        //DevInfo.Add(pid);

                        // USBCOMlist.Add(DevInfo);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


