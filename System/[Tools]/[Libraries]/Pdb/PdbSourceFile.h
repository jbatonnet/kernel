#ifndef _PDBSOURCEFILE_H_
#define _PDBSOURCEFILE_H_

#include "Pdb.h"
#include "PdbSymbol.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace Tools
{
    namespace Pdb
    {
        public ref class PdbSourceFile
        {
        public:
            property String^ FileName
            {
                String^ get()
                {
                    BSTR result = nullptr;
                    diaSourceFile->get_fileName(&result);
                    return result ? Marshal::PtrToStringBSTR(IntPtr(result)) : nullptr;
                }
            }
            property IEnumerable<PdbSymbol^>^ Compilands
            {
                IEnumerable<PdbSymbol^>^ get()
                {
                    IDiaEnumSymbols* result = nullptr;
                    diaSourceFile->get_compilands(&result);
                    return result ? gcnew PdbSymbolCollection(result) : nullptr;
                }
            }
            property DWORD UniqueId
            {
                DWORD get()
                {
                    DWORD result;
                    diaSourceFile->get_uniqueId(&result);
                    return result;
                }
            }

        internal:
            IDiaSourceFile* diaSourceFile;

        internal:
            PdbSourceFile(IDiaSourceFile* diaSourceFile)
            {
                this->diaSourceFile = diaSourceFile;
            }

        public:
            String^ ToString() override
            {
                return FileName;
            }
        };

        public ref class PdbSourceFileCollection : public IEnumerable<PdbSourceFile^>
        {
            ref class Enumerator : public IEnumerator<PdbSourceFile^>
            {
            public:
                property PdbSourceFile^ Current
                {
                    virtual PdbSourceFile^ get()
                    {
                        return diaSourceFile ? gcnew PdbSourceFile(diaSourceFile) : nullptr;
                    };
                };
                property Object^ Current2
                {
                    virtual Object^ get() = System::Collections::IEnumerator::Current::get
                    {
                        return diaSourceFile ? gcnew PdbSourceFile(diaSourceFile) : nullptr;
                    };
                };

            private:
                IDiaEnumSourceFiles* diaEnumSourceFiles;
                IDiaSourceFile* diaSourceFile;

            internal:
                Enumerator(IDiaEnumSourceFiles* diaEnumSourceFiles);
                ~Enumerator();

            public:
                virtual bool MoveNext();
                virtual void Reset();
            };

        public:
            property int Count
            {
                virtual int get()
                {
                    LONG result;
                    diaEnumSourceFiles->get_Count(&result);
                    return result;
                }
            }

        private:
            IDiaEnumSourceFiles* diaEnumSourceFiles;

        internal:
            PdbSourceFileCollection(IDiaEnumSourceFiles* diaEnumSourceFiles);

        public:
            virtual IEnumerator<PdbSourceFile^>^ GetEnumerator();
            virtual System::Collections::IEnumerator^ GetEnumerator2() = System::Collections::IEnumerable::GetEnumerator;
        };
    }
}

#endif