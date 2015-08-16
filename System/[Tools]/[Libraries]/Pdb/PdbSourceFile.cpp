#include "Pdb.h"
#include "PdbSourceFile.h"

using namespace Tools::Pdb;

PdbSourceFileCollection::Enumerator::Enumerator(IDiaEnumSourceFiles* diaEnumSourceFiles) : diaSourceFile(nullptr)
{
    this->diaEnumSourceFiles = diaEnumSourceFiles;
}
PdbSourceFileCollection::Enumerator::~Enumerator() { }

bool PdbSourceFileCollection::Enumerator::MoveNext()
{
    ULONG celt = 0;
    IDiaSourceFile* diaSourceFile = nullptr;

    diaEnumSourceFiles->Next(1, &diaSourceFile, &celt);
    if (celt == 0)
        diaSourceFile = nullptr;

    this->diaSourceFile = diaSourceFile;
    return celt == 1;
}
void PdbSourceFileCollection::Enumerator::Reset()
{
    diaEnumSourceFiles->Reset();
}

PdbSourceFileCollection::PdbSourceFileCollection(IDiaEnumSourceFiles* diaEnumSourceFiles)
{
    this->diaEnumSourceFiles = diaEnumSourceFiles;
}

IEnumerator<PdbSourceFile^>^ PdbSourceFileCollection::GetEnumerator()
{
    return gcnew Enumerator(diaEnumSourceFiles);
}
System::Collections::IEnumerator^ PdbSourceFileCollection::GetEnumerator2()
{
    return gcnew Enumerator(diaEnumSourceFiles);
}