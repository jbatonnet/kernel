#include "Pdb.h"
#include "PdbSession.h"

using namespace Tools::Pdb;

IEnumerable<PdbSymbol^>^ PdbSession::Symbols::get()
{
    IDiaEnumTables* diaEnumTables = nullptr;
    if (FAILED(diaSession->getEnumTables(&diaEnumTables)))
        return nullptr;
    if (diaEnumTables == nullptr)
        return nullptr;

    ULONG celt = 0;
    IDiaTable* diaTable = nullptr;
    while (SUCCEEDED(diaEnumTables->Next(1, &diaTable, &celt)) && celt == 1)
    {
        IDiaEnumSymbols* diaEnumSymbols;
        if (SUCCEEDED(diaTable->QueryInterface(_uuidof(IDiaEnumSymbols), (void**)&diaEnumSymbols)))
            return diaEnumSymbols ? gcnew PdbSymbolCollection(diaEnumSymbols) : nullptr;
    }

    return nullptr;

    // } else if ( SUCCEEDED( pTable->QueryInterface( _uuidof( IDiaEnumSourceFiles ), (void**)&pSourceFiles ) ) ) {
}
IEnumerable<PdbSourceFile^>^ PdbSession::SourceFiles::get()
{
    IDiaEnumTables* diaEnumTables = nullptr;
    if (FAILED(diaSession->getEnumTables(&diaEnumTables)))
        return nullptr;
    if (diaEnumTables == nullptr)
        return nullptr;

    ULONG celt = 0;
    IDiaTable* diaTable = nullptr;
    while (SUCCEEDED(diaEnumTables->Next(1, &diaTable, &celt)) && celt == 1)
    {
        IDiaEnumSourceFiles* diaEnumSourceFiles;
        if (SUCCEEDED(diaTable->QueryInterface(_uuidof(IDiaEnumSourceFiles), (void**)&diaEnumSourceFiles)))
            return diaEnumSourceFiles ? gcnew PdbSourceFileCollection(diaEnumSourceFiles) : nullptr;
    }

    return nullptr;
}

PdbSession::PdbSession(IDiaSession* diaSession)
{
    this->diaSession = diaSession;
}

PdbSymbol^ PdbSession::GetSymbolAtAddress(PdbSymbolTag tag, DWORD section, DWORD offset)
{
    IDiaSymbol* diaSymbol = nullptr;
    diaSession->findSymbolByAddr(section, offset, (enum SymTagEnum)tag, &diaSymbol);
    return diaSymbol ? gcnew PdbSymbol(diaSymbol) : nullptr;
}
PdbSymbol^ PdbSession::GetSymbolAtVirtualAddress(PdbSymbolTag tag, ULONGLONG address)
{
    IDiaSymbol* diaSymbol = nullptr;
    diaSession->findSymbolByVA(address, (enum SymTagEnum)tag, &diaSymbol);
    return diaSymbol ? gcnew PdbSymbol(diaSymbol) : nullptr;
}
PdbSymbol^ PdbSession::GetSymbolAtRelativeVirtualAddress(PdbSymbolTag tag, DWORD address)
{
    IDiaSymbol* diaSymbol = nullptr;
    diaSession->findSymbolByRVA(address, (enum SymTagEnum)tag, &diaSymbol);
    return diaSymbol ? gcnew PdbSymbol(diaSymbol) : nullptr;
}

void PdbSession::GetAddressFromVirtualAddress(ULONGLONG address, DWORD% section, DWORD% offset)
{
    DWORD _section, _offset;

    diaSession->addressForVA(address, &_section, &_offset);

    section = _section;
    offset = _offset;
}
void PdbSession::GetAddressFromRelativeVirtualAddress(DWORD address, DWORD% section, DWORD% offset)
{
    DWORD _section, _offset;

    diaSession->addressForRVA(address, &_section, &_offset);

    section = _section;
    offset = _offset;
}

IEnumerable<PdbLineNumber^>^ PdbSession::FindLinesByAddress(DWORD section, DWORD offset, DWORD length)
{
	IDiaEnumLineNumbers* diaEnumLineNumbers = nullptr;
	diaSession->findLinesByAddr(section, offset, length, &diaEnumLineNumbers);
	return diaEnumLineNumbers ? gcnew PdbLineNumberCollection(diaEnumLineNumbers) : nullptr;
}
IEnumerable<PdbLineNumber^>^ PdbSession::FindLinesByVirtualAddress(ULONGLONG address, DWORD length)
{
	IDiaEnumLineNumbers* diaEnumLineNumbers = nullptr;
	diaSession->findLinesByVA(address, length, &diaEnumLineNumbers);
	return diaEnumLineNumbers ? gcnew PdbLineNumberCollection(diaEnumLineNumbers) : nullptr;
}
IEnumerable<PdbLineNumber^>^ PdbSession::FindLinesByRelativeVirtualAddress(DWORD address, DWORD length)
{
	IDiaEnumLineNumbers* diaEnumLineNumbers = nullptr;
	diaSession->findLinesByRVA(address, length, &diaEnumLineNumbers);
	return diaEnumLineNumbers ? gcnew PdbLineNumberCollection(diaEnumLineNumbers) : nullptr;
}

IEnumerable<PdbLineNumber^>^ PdbSession::FindLinesByLine(PdbSymbol^ compiland, PdbSourceFile^ sourceFile, DWORD line, DWORD column)
{
    IDiaEnumLineNumbers* diaEnumLineNumbers = nullptr;
    diaSession->findLinesByLinenum(compiland->diaSymbol, sourceFile->diaSourceFile, line, column, &diaEnumLineNumbers);
    return diaEnumLineNumbers ? gcnew PdbLineNumberCollection(diaEnumLineNumbers) : nullptr;
}