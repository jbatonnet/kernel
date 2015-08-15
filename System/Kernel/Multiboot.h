#ifndef _KERNEL_MULTIBOOT_H_
#define _KERNEL_MULTIBOOT_H_

#include <Kernel/Types.h>

struct MultibootInfo
{
    u32 Flags;
    u32 MemoryLo;
    u32 MemoryHi;
    u32 BootDevice;
    u32 CommandLine;
    u32 ModsCount;
    u32 ModsAddr;
    u32 Syms0;
    u32 Syms1;
    u32 Syms2;
    u32 Syms3;
    u32 MemoryMapLength;
    u32 MemoryMapAddress;
    u32 DrivesLength;
    u32 DrivesAddress;
    u32 ConfigTable;
    u32 BootloaderName;
    u32 ApmTable;
    u32 VbeControlInfo;
    u32 VbeModeInfo;
    u16 VbeMode;
    u16 VbeInterfaceSegment;
    u16 VbeInterfaceOffset;
    u16 VbeInterfaceLength;
};

#endif