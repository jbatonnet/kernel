#include <Kernel/Types.h>
#include <Kernel/Multiboot.h>
#include <Kernel/Debugger.h>

#include <Screen/Screen.h>

void Test(u32 value)
{
    Screen::WriteLine("Break :)");
    Debugger::Break();
}

void Main(MultibootInfo* bootInfo)
{
    Debugger::Initialize();

    Screen::Clear();
    Screen::WriteLine("Hello World !");
    Screen::WriteLine();

    for (u32 n = 0;; n++)
        Test(n);
}