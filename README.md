# Kernel

This solution is the basic structure used in my operating system project, developped from scratch. I mainly use a Windows powered environment, and I chose to use Visual Studio and VMware to build and test my kernel.

It is part of a larger project of rebuilding and rethinking the operating system with modern choices. I have added some drivers (Mouse, Keyboard, ATA, FAT, Serial, VESA, VMware graphic card, ...) and a custom designed user interface to provide the best experience to developers and users. You may have some news about this project soon.

This project also illustrates the ability to use the power of Visual Studio without relying on any Microsoft standard libraries.

### File structure details

* **Root** : The directory representing the hard drive of the virtual machine
    * **System/Kernel** : The kernel built with Visual Studio
    * **System/Boot/Bootloader** : GRUB 0.97 bootloader
    * **System/Boot/Menu** : GRUB menu configuration file (as the built kernel is multiboot compliant, this file is kept very simple)
* **Tools** : Some utilities, tools and scripts to help manipulating your kernel
    * **Grub** : GRUB 0.97 reference, with a prebuilt floppy image
    * **VMware** : The VDDK (Virtual Disk Development Kit) provided by VMware, to manipulate virtual hard drives
    * **Clean.bat** : Clean some Visual Studio temporary files
    * **Shrink.bat** : Defragment and shrink the VMware virtual hard drive
    * **Mount.bat**, **Unmount.bat** : Mount and unmount the virtual hard drive as Z: 
* **VMware** : The VMware virtual machine, with the linked virtual hard drive
* **System** : The root directory of the source code
    * **[Tools]** : The tools used to build and debug your kernel
    * **Kernel** : Your kernel source code
    * **Screen** : A sample library exposing several methods to print on screen

### Environment setup

The provided source code is build with the final version of **Visual Studio 2015 Community**. You may try to use earlier versions as I started with Visual Studio 2012, but you will surely have some troubles with C# projects using last C# 6 features.

To manipulate the virtual hard drive, you will need to install the **Virtual Disk Devlopment Kit** by VMware. You can easily find it by searching the package "**VMware-vix-disklib-5.1.0-774844.i386**" on you favorite search engine. This is the package I personnaly use.

And finally, to run your kernel, you will need **VMware Player** to start the virtual machine. I only provide the code for VMware virtual machine, but any virtual machine with a built-in GDB stub would work. Just need to write some lines of code to control it.  

### Usage

Here are the steps to quickly build and run your Kernel :
* Open the **System.sln** solution in Visual Studio
* Set the **Debugger** as startup project
* **Build** and **Run** the solution
    * *Screen* will be built
    * *Kernel* will be built
    * *Generate.PostBuild* will be executed as *Kernel* post build event. It will deploy the newly generated kernel in the virtual hard drive
    * *Debugger* will be executed as startup project, and it will start the virtual machine
    
When the virtual machine is up and running, you can use the **Debugger** to stop, catch breakpoints, show the current callstack and variables and show the virtual machine memory. It will help you to debug your kernel features.  

### Limitations / Known issues

* The need of VDDK to be installed, and the need of third-party binaries. I tried several virtual hard drive manipulation libraries, but they are far from complete or just buggy right now. As this solution is working right now, this is not my highest priority, but it would be much better to use a full C# library to manipulate the VMDK.
* I provide a quick breakpoint implementation, with the ability for the Debugger to "break" the virtual machine at a certain line of code. Sometimes, the Debugger will break the virtual machine too fast, before the screen refreshes itself. You may need to step one cycle to correctly display what you need (typically if you break just after a Write).
* With Visual Studio 2013 and 2015, it is not possible anymore to correctly use the link order setting. You may need to manually specify the kernel entry point. See FAQ for more details.
* The debugger is young, and it has some bugs. For example, if you reset the virtual machine directly from VMware, it will surely throw an exception. Not every cases are handled, but I hope to have a working Visual Studio debugger soon. This Debugger is only temporary and is a "Proof of Concept" of the usage of GDB and PDB to debug a remote target. 
* I currently work on building a debugger with the Visual Studio SDK. It would allow Visual Studio to directly debug the kernel, without having to use an external debugger. We would be able to use the power of Visual Studio debugging tools to save a lot of time.

### Want to contribute ?

This project is mainly a sharing of my tools to help interested developpers to build or test an operating system/kernel made from scratch.

Anyway, if you are interested, I may need help with these topics :  
* **Debugger** : The goal is to provide a nearly native debugging experience directly from Visual Studio. I have seen some difficulties with fully managed Visual Studio interoperabilities to register the custom Debugger. Once this is done, it will be easy to implement every features, as they are working in the external debugger. It would be nice to develop this debugger only using GDB protocol and current project symbols (PDB). It would allow Visual Studio to debug any remote machine/program with a built-in GDB stub. If I manage to have a half-working sample, I will share it on GitHub. 
* **Kernel** : This is the most trickier part of building an operating system from scratch. Feel free to contact me if you want more details or if you are interested in such topics. 

## FAQ

### The virtual machine displays "Error 13: Invalid or unsupported executable format"

If you add some global symbols in your Kernel, you may disturb the internal structure of the final binary, and therefore move the Kernel entry point. As for Visual Studio 2012, it was possible to specify a build order override file, to manually set some symbols positions. With Visual Studio 2013 and 2015, this option seems to be broken, and you may need to manually setup the entry point address.

If GRUB is not able to find the entry point, your virtual machine will displays "Error 13: Invalid or unsupported executable format". At this point, you will need to manually specify the new entry point :
* Open the **Kernel.map** file
* Find the line containing the symbol **?multiboot_entry@@YAXXZ** 00XXXXXX
* Copy the address on the right in the file **Config.h**, redefining the macro **KERNEL_ENTRY**
* **Build** and **Start** your kernel