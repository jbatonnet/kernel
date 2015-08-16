#include "Pdb.h"
#include "PdbSymbol.h"

using namespace Tools::Pdb;

PdbSymbol::PdbSymbol(IDiaSymbol* diaSymbol)
{
    this->diaSymbol = diaSymbol;
}

IEnumerable<PdbSymbol^>^ PdbSymbol::FindChildren(PdbSymbolTag tag)
{
    IDiaEnumSymbols* diaEnumSymbols = nullptr;
    diaSymbol->findChildren((enum SymTagEnum)tag, nullptr, nsNone, &diaEnumSymbols);

    return diaEnumSymbols ? gcnew PdbSymbolCollection(diaEnumSymbols) : nullptr;
}
IEnumerable<PdbSymbol^>^ PdbSymbol::FindChildren(PdbSymbolTag tag, String^ name)
{
    LPCOLESTR nameStr = name == nullptr ? nullptr : (LPCOLESTR)(Marshal::StringToHGlobalUni(name).ToPointer());

    IDiaEnumSymbols* diaEnumSymbols = nullptr;
    diaSymbol->findChildren((enum SymTagEnum)tag, nameStr, nsfCaseInsensitive | nsfUndecoratedName, &diaEnumSymbols);

    return diaEnumSymbols ? gcnew PdbSymbolCollection(diaEnumSymbols) : nullptr;
}

PdbSymbolCollection::Enumerator::Enumerator(IDiaEnumSymbols* diaEnumSymbols) : diaSymbol(nullptr)
{
	this->diaEnumSymbols = diaEnumSymbols;
}
PdbSymbolCollection::Enumerator::~Enumerator() { }

bool PdbSymbolCollection::Enumerator::MoveNext()
{
	ULONG celt = 0;
	IDiaSymbol* diaSymbol = nullptr;

	diaEnumSymbols->Next(1, &diaSymbol, &celt);
	if (celt == 0)
		diaSymbol = nullptr;

	this->diaSymbol = diaSymbol;
	return celt == 1;
}
void PdbSymbolCollection::Enumerator::Reset()
{
	diaEnumSymbols->Reset();
}

PdbSymbolCollection::PdbSymbolCollection(IDiaEnumSymbols* diaEnumSymbols)
{
	this->diaEnumSymbols = diaEnumSymbols;
}

IEnumerator<PdbSymbol^>^ PdbSymbolCollection::GetEnumerator()
{
	return gcnew Enumerator(diaEnumSymbols);
}
System::Collections::IEnumerator^ PdbSymbolCollection::GetEnumerator2()
{
	return gcnew Enumerator(diaEnumSymbols);
}