#ifndef _SCREEN_SCREEN_H_
#define _SCREEN_SCREEN_H_

class Screen
{
public:
    static void Clear();

    static void Write(const char* text);
    static void WriteLine(const char* text);
    static void WriteLine();
};

#endif