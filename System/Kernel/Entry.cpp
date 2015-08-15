#include <Kernel/Multiboot.h>
#include <Kernel/Config.h>

#define dd(x) __asm _emit (x) & 0xff \
              __asm _emit (x) >> 8  & 0xff \
              __asm _emit (x) >> 16 & 0xff \
              __asm _emit (x) >> 24 & 0xff

#define MULTIBOOT_MAGIC    0x1BADB002
#define MULTIBOOT_FLAGS    0x00010000
#define MULTIBOOT_CHECKSUM -(MULTIBOOT_MAGIC + MULTIBOOT_FLAGS)

#define KERNEL_HEADER KERNEL_ENTRY
#define KERNEL_CODE   KERNEL_ENTRY + 0x20

extern void InitializeConstructors();
extern void Exit();

extern void Main(MultibootInfo* bootInfo);

void __declspec(naked) multiboot_entry()
{
    _asm // Multiboot header
    {
        dd(MULTIBOOT_MAGIC)
        dd(MULTIBOOT_FLAGS)
        dd(MULTIBOOT_CHECKSUM)

        dd(KERNEL_HEADER)
        dd(KERNEL_BASE)
        dd(0x00000000)
        dd(0x00000000)
        dd(KERNEL_CODE)
    }

    _asm // Stack and flags
    {
        mov esp, KERNEL_STACK
        mov ebp, esp

        push 0
        popfd
    }

    _asm
    {
        push ebx        
        call Main
    }

    _asm
    {
        cli
        hlt
    }

    for (;;);
}