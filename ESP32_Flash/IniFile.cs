
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Ini_File
{
	public class ReadWriteINIfile
	{
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string name, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		string Path;
		string EXE = Assembly.GetExecutingAssembly().GetName().Name;

		public bool IniFile(string IniPath = null)
		{
			Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
			return File.Exists(Path);
		}


		public void WriteINI(string name, string key, string value)
		{
			WritePrivateProfileString(name, key, value, this.Path);
		}
		public string ReadINI(string name, string key)
		{
			StringBuilder sb = new StringBuilder(255);
			int ini = GetPrivateProfileString(name, key, "", sb, 255, this.Path);
			return sb.ToString();
		}

		public bool IniFileExists()
		{
			return File.Exists("Path");
		}
		public string GetIniFilename()
		{
			return EXE + ".ini";
		}
		public string ReadConfigValue(string section, string key)
        {
			ReadWriteINIfile iniFile = new ReadWriteINIfile();
			iniFile.IniFile();
			return iniFile.ReadINI(section, key);
		}

		public string[] ReadConfigFromIniFile()
		{
	
			List<string> values = new List<string>();
			ReadWriteINIfile iniFile = new ReadWriteINIfile();
			iniFile.IniFile();

        	values.Add(iniFile.ReadINI("esptool", "Erase"));
			values.Add(iniFile.ReadINI("esptool", "Flash"));

			return values.ToArray();
		}

		public void CreateINI_File()
		{

			var dict = 	new Dictionary<string, Dictionary<string, string>>() 
			{
                {
					"usbcom", new Dictionary<string, string>
					{
						{"vid", "303A"},
						{"pid", "1001"}
					}
                }
				,
				{	"esptool",new Dictionary<string, string>
							{
								{ "Erase", "esptool.exe -p COM7 -b 460800 erase_flash" },
								{ "Flash","esptool.exe -p COM7 -b 460800 --before=default_reset --after=hard_reset  write_flash --flash_mode dio --flash_freq 80m --flash_size 4MB 0x0 bootloader.bin 0x10000 boot_app.bin 0x8000 partition-table.bin"},
							}
				},
				{
					"path", new Dictionary<string, string>
							{
								{ "Dir", "C:\\" }
							}
				}
			};

			ReadWriteINIfile iniFile = new ReadWriteINIfile();
			if (iniFile.IniFile() == false)
			{
				foreach (string section in dict.Keys)
				{
					foreach (var inner in dict[section])
					{
						iniFile.WriteINI(section, inner.Key, inner.Value);
					}
				}
			}
		}
	}
}
