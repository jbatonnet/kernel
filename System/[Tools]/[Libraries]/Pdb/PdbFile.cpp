#include "Pdb.h"
#include "PdbFile.h"

using namespace Tools::Pdb;

PdbFile::PdbFile(String^ path)
{
    this->path = path;

    if (!File::Exists(path))
        throw gcnew IOException();

    IDiaDataSource* diaDataSourcePointer = NULL;
    ::CoCreateInstance(CLSID_DiaSource, NULL, CLSCTX_INPROC_SERVER, IID_IDiaDataSource, (LPVOID*)&diaDataSourcePointer);
    diaDataSource = diaDataSourcePointer;

    LPCOLESTR pathStr = (LPCOLESTR)(Marshal::StringToHGlobalUni(path).ToPointer());
    diaDataSource->loadDataFromPdb(pathStr);
}

PdbSession^ PdbFile::OpenSession(ULONGLONG address)
{
    IDiaSession* diaSession = NULL;
    diaDataSource->openSession(&diaSession);

    diaSession->put_loadAddress(address);

    return gcnew PdbSession(diaSession);
}