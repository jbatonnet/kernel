#ifndef _PDBLINENUMBER_H_
#define _PDBLINENUMBER_H_

#include "Pdb.h"
#include "PdbSymbol.h"
#include "PdbSourceFile.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

#define SymbolGetter(name, field) \
	property PdbSymbol^ name \
	{ \
		PdbSymbol^ get() \
		{ \
			IDiaSymbol* result = nullptr; \
			diaLineNumber->get_##field(&result); \
			return result ? gcnew PdbSymbol(result) : nullptr; \
		} \
	}

#define BoolGetter(name, field) \
	property bool name \
	{ \
		bool get() \
		{ \
			BOOL result; \
			diaLineNumber->get_##field(&result); \
			return !!result; \
		} \
	}

#define SourceGetter(name, field) \
	property PdbSourceFile^ name \
	{ \
		PdbSourceFile^ get() \
		{ \
			IDiaSourceFile* result = nullptr; \
			diaLineNumber->get_##field(&result); \
			return result ? gcnew PdbSourceFile(result) : nullptr; \
		} \
	}

#define Getter(type, name, field) \
	property type name \
	{ \
		type get() \
		{ \
			type result; \
			diaLineNumber->get_##field(&result); \
			return result; \
		} \
	}

namespace Tools
{
    namespace Pdb
    {
        public ref class PdbLineNumber
        {
        public:
            Getter(DWORD, AddressOffset, addressOffset);
            Getter(DWORD, AddressSection, addressSection);
            Getter(DWORD, ColumnNumber, columnNumber);
            Getter(DWORD, ColumnNumberEnd, columnNumberEnd);
            SymbolGetter(Compiland, compiland);
            Getter(DWORD, CompilandId, compilandId);
            Getter(DWORD, Length, length);
            Getter(DWORD, LineNumber, lineNumber);
            Getter(DWORD, LineNumberEnd, lineNumberEnd);
            Getter(DWORD, RelativeVirtualAddress, relativeVirtualAddress);
            SourceGetter(SourceFile, sourceFile);
            Getter(DWORD, SourceFileId, sourceFileId);
            BoolGetter(Statement, statement);
            Getter(ULONGLONG, VirtualAddress, virtualAddress);

        private:
            IDiaLineNumber* diaLineNumber;

        internal:
            PdbLineNumber(IDiaLineNumber* diaLineNumber)
            {
                this->diaLineNumber = diaLineNumber;
            }
        };

        public ref class PdbLineNumberCollection : public IEnumerable<PdbLineNumber^>
        {
            ref class Enumerator : public IEnumerator<PdbLineNumber^>
            {
            public:
                property PdbLineNumber^ Current
                {
                    virtual PdbLineNumber^ get()
                    {
                        return diaLineNumber ? gcnew PdbLineNumber(diaLineNumber) : nullptr;
                    };
                };
                property Object^ Current2
                {
                    virtual Object^ get() = System::Collections::IEnumerator::Current::get
                    {
                        return diaLineNumber ? gcnew PdbLineNumber(diaLineNumber) : nullptr;
                    };
                };

            private:
                IDiaEnumLineNumbers* diaEnumLineNumbers;
                IDiaLineNumber* diaLineNumber;

            internal:
                Enumerator(IDiaEnumLineNumbers* diaEnumLineNumbers);
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
                    diaEnumLineNumbers->get_Count(&result);
                    return result;
                }
            }

        private:
            IDiaEnumLineNumbers* diaEnumLineNumbers;

        internal:
            PdbLineNumberCollection(IDiaEnumLineNumbers* diaEnumLineNumbers);

        public:
            virtual IEnumerator<PdbLineNumber^>^ GetEnumerator();
            virtual System::Collections::IEnumerator^ GetEnumerator2() = System::Collections::IEnumerable::GetEnumerator;
        };
    }
}

#endif