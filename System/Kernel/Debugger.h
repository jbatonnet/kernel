#ifndef _KERNEL_DEBUGGER_H_
#define _KERNEL_DEBUGGER_H_

class Debugger
{
private:
    static bool attached;

public:
    static void Initialize();
    static void Break();
};

#endif