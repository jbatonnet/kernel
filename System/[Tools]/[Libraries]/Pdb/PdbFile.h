#ifndef _PDBFILE_H_
#define _PDBFILE_H_

#include "PdbSession.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace Tools
{
    namespace Pdb
    {
        public ref class PdbFile
        {
        public:
            property String^ Path
            {
                String^ get()
                {
                    return path;
                }
            }

        private:
            String^ path;
            IDiaDataSource* diaDataSource;

        public:
            PdbFile(String^ path);

            PdbSession^ OpenSession(ULONGLONG address);
        };
    }
}

#endif