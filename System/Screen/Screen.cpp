#include "Screen.h"
#include "Types.h"

#define SCREEN_COLOR 0x07

u16 cursorX = 0;
u16 cursorY = 7;

union ScreenCharacter
{
    struct
    {
        char Character;
        u8 Attributes;
    };
    u16 Value;
};
ScreenCharacter* screen = (ScreenCharacter*)0x0B8000;

u8 IO_Read8(u16 port)
{
    u8 value;
    _asm
    {
        mov dx, word ptr[port]
        in al, dx
            mov byte ptr[value], al
    }
    return value;
}
void IO_Write8(u16 port, u8 value)
{
    _asm
    {
        mov al, byte ptr[value]
        mov dx, word ptr[port]
            out dx, al
    }
}

void SetCursorPosition()
{
    u16 cursorPosition = cursorY * 80 + cursorX;

    IO_Write8(0x03D4, 14);
    IO_Write8(0x03D5, (u8)(cursorPosition >> 8));
    IO_Write8(0x03D4, 15);
    IO_Write8(0x03D5, (u8)cursorPosition);
}
void GetCursorPosition()
{
    u16 cursorPosition;

    IO_Write8(0x3D4, 14);
    cursorPosition = (u16)IO_Read8(0x03D5) << 8;
    IO_Write8(0x3D4, 15);
    cursorPosition |= (u16)IO_Read8(0x03D5);

    cursorX = cursorPosition % 80;
    cursorY = cursorPosition / 80;
}

char lastCharacter = 0;
bool initialized = false;

void Screen::Clear()
{
    for (u32 i = 0; i < 80 * 25; i++)
    {
        screen[i].Character = ' ';
        screen[i].Attributes = 0x07;
    }

    cursorX = 0;
    cursorY = 0;

    SetCursorPosition();
}

void Screen::Write(const char* text)
{
    GetCursorPosition();

    for (u32 i = 0; text[i]; i++)
    {
        char value = text[i];

        switch (value)
        {
            case 'â': value = (char)0x83; break;
            case 'ä': value = (char)0x84; break;
            case 'à': value = (char)0x85; break;

            case 'é': value = (char)0x82; break;
            case 'ê': value = (char)0x88; break;
            case 'ë': value = (char)0x89; break;
            case 'è': value = (char)0x8A; break;

            case 'ï': value = (char)0x8B; break;
            case 'î': value = (char)0x8C; break;

            case 'ô': value = (char)0x93; break;
            case 'ö': value = (char)0x94; break;

            case 'û': value = (char)0x96; break;
            case 'ü': value = (char)0x81; break;
        }

        switch (value)
        {
            case 0:
                break;

            // New line
            case '\n':
                cursorX = 0;
                if (++cursorY == 25)
                {
                    for (u32 y = 0; y < 24; y++)
                        for (u32 x = 0; x < 80; x++)
                            screen[(y * 80 + x)] = screen[((y + 1) * 80 + x)];

                    for (int j = 0; j < 80; j++)
                    {
                        screen[80 * 24 + j].Character = ' ';
                        screen[80 * 24 + j].Attributes = SCREEN_COLOR;
                    }

                    cursorY = 24;
                }
                break;

            // Carriage return
            case '\r':
                cursorX = 0;
                break;

            // Backspace
            case '\b':
                if (--cursorX < 0)
                {
                    cursorX = 79;
                    if (--cursorY < 0)
                        cursorX = cursorY = 0;
                }

                screen[cursorY * 80 + cursorX].Character = ' ';
                screen[cursorY * 80 + cursorX].Attributes = SCREEN_COLOR;

                break;

            // Tab
            case '\t':
                do
                    Write(" ");
                while (cursorX % 4);
                break;

            // Other
            default:
                screen[cursorY * 80 + cursorX].Character = value;
                screen[cursorY * 80 + cursorX].Attributes = SCREEN_COLOR;

                if (++cursorX == 80)
                {
                    cursorX = 0;
                    if (++cursorY == 25)
                    {
                        for (u32 y = 0; y < 24; y++)
                            for (u32 x = 0; x < 80; x++)
                                screen[(y * 80 + x)] = screen[((y + 1) * 80 + x)];

                        for (int j = 0; j < 80; j++)
                        {
                            screen[80 * 24 + j].Character = ' ';
                            screen[80 * 24 + j].Attributes = SCREEN_COLOR;
                        }

                        cursorY = 24;
                    }
                }
                break;
        }
    }

    SetCursorPosition();
}
void Screen::WriteLine(const char* text)
{
    Write(text);
    WriteLine();
}
void Screen::WriteLine()
{
    Write("\n");
}