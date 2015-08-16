#include "Pdb.h"
#include "PdbLineNumber.h"

using namespace Tools::Pdb;

PdbLineNumberCollection::Enumerator::Enumerator(IDiaEnumLineNumbers* diaEnumLineNumbers) : diaLineNumber(nullptr)
{
    this->diaEnumLineNumbers = diaEnumLineNumbers;
}
PdbLineNumberCollection::Enumerator::~Enumerator() { }

bool PdbLineNumberCollection::Enumerator::MoveNext()
{
    ULONG celt = 0;
	IDiaLineNumber* diaLineNumber = nullptr;

	diaEnumLineNumbers->Next(1, &diaLineNumber, &celt);
    if (celt == 0)
		diaLineNumber = nullptr;

    this->diaLineNumber = diaLineNumber;
    return celt == 1;
}
void PdbLineNumberCollection::Enumerator::Reset()
{
	diaEnumLineNumbers->Reset();
}

PdbLineNumberCollection::PdbLineNumberCollection(IDiaEnumLineNumbers* diaEnumLineNumbers)
{
    this->diaEnumLineNumbers = diaEnumLineNumbers;
}

IEnumerator<PdbLineNumber^>^ PdbLineNumberCollection::GetEnumerator()
{
    return gcnew Enumerator(diaEnumLineNumbers);
}
System::Collections::IEnumerator^ PdbLineNumberCollection::GetEnumerator2()
{
    return gcnew Enumerator(diaEnumLineNumbers);
}