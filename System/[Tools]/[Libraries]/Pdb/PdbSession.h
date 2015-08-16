#ifndef _PDBSESSION_H_
#define _PDBSESSION_H_

#include "PdbSymbol.h"
#include "PdbSourceFile.h"
#include "PdbLineNumber.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace Tools
{
    namespace Pdb
    {
        public ref class PdbSession
        {
        public:
            property PdbSymbol^ Global
            {
                PdbSymbol^ get()
                {
                    IDiaSymbol* result = nullptr;
                    diaSession->get_globalScope(&result);
                    return result ? gcnew PdbSymbol(result) : nullptr;
                }
            }
            property IEnumerable<PdbSymbol^>^ Symbols
            {
                IEnumerable<PdbSymbol^>^ get();
            }
            property IEnumerable<PdbSourceFile^>^ SourceFiles
            {
                IEnumerable<PdbSourceFile^>^ get();
            }

        internal:
            IDiaSession* diaSession;

            PdbSession(IDiaSession* diaSession);

        public:
            PdbSymbol^ GetSymbolAtAddress(PdbSymbolTag tag, DWORD section, DWORD offset);
            PdbSymbol^ GetSymbolAtVirtualAddress(PdbSymbolTag tag, ULONGLONG address);
            PdbSymbol^ GetSymbolAtRelativeVirtualAddress(PdbSymbolTag tag, DWORD address);

            void GetAddressFromVirtualAddress(ULONGLONG address, DWORD% section, DWORD% offset);
            void GetAddressFromRelativeVirtualAddress(DWORD address, DWORD% section, DWORD% offset);

            IEnumerable<PdbLineNumber^>^ FindLinesByAddress(DWORD section, DWORD offset, DWORD length);
            IEnumerable<PdbLineNumber^>^ FindLinesByVirtualAddress(ULONGLONG address, DWORD length);
            IEnumerable<PdbLineNumber^>^ FindLinesByRelativeVirtualAddress(DWORD address, DWORD length);

            IEnumerable<PdbLineNumber^>^ FindLinesByLine(PdbSymbol^ compiland, PdbSourceFile^ sourceFile, DWORD line, DWORD column);
        };
    }
}

#endif