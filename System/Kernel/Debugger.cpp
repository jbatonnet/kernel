#include <Kernel/Debugger.h>

bool Debugger::attached = false;

void Debugger::Initialize()
{
	while (!attached);
}

#pragma optimize("g", on)

void Debugger::Break()
{
    _asm int 3
}

#pragma optimize("g", off)