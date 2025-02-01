**Overview**

This ESP32 Flash project allows you to burn ESP32 COM during plug and play.
To do this, you must first run the ESP32-Flash utility and then insert the USB connector.
For burning, ESP32_Flash uses the standard eSPtool of Еsprеssive. 
Every time you insert a new device into your computer, it gives you a new COM port that you need to enter in the esptool command line.
The ESP32_Flash utility detects this automatically.
This allows you to quickly burn a number of devices without finding out which COM port the computer has allocated for work.
Also suitable for production purposes.

![image](https://github.com/user-attachments/assets/64d4104f-b477-46b2-9f3f-5a61a74f91a9)

**Configuration**

This allows you to quickly burn a number of devices without finding out which COM port the computer has allocated for work.
Also suitable for production purposes.
For configuration, you need to open the ESP32-Flash.ini file and change the VID and PID parameters for your device.
and make a copy paste of the command line with which you burn your device.

```ini
[usbcom]
vid = 10C4
pid = EA60

[esptool]
//Erase=esptool.exe -p COM7 -b 460800 erase_flash
Flash=esptool.exe -p COM9 -b 460800 --before=default_reset --after=hard_reset  write_flash --flash_mode dio --flash_freq 80m --flash_size 4MB 0x0 bootloader.bin 0x10000 app.bin 0x8000 partition-table.bin

[path]
Dir=C:\
