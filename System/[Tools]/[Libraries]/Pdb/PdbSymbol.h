#ifndef _PDBSYMBOL_H_
#define _PDBSYMBOL_H_

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

#define SymbolGetter(name, field) \
	property PdbSymbol^ name \
	{ \
		PdbSymbol^ get() \
		{ \
			IDiaSymbol* result = nullptr; \
			diaSymbol->get_##field(&result); \
			return result ? gcnew PdbSymbol(result) : nullptr; \
		} \
	}

#define StringGetter(name, field) \
	property String^ name \
	{ \
		String^ get() \
		{ \
			BSTR result = nullptr; \
			diaSymbol->get_##field(&result); \
			return result ? Marshal::PtrToStringBSTR(IntPtr(result)) : nullptr; \
		} \
	}

#define BoolGetter(name, field) \
	property bool name \
	{ \
		bool get() \
		{ \
			BOOL result; \
			diaSymbol->get_##field(&result); \
			return !!result; \
		} \
	}

#define EnumGetter(type, name, field) \
	property type name \
	{ \
		type get() \
		{ \
			DWORD result; \
			diaSymbol->get_##field(&result); \
			return (type)result; \
		} \
	}

#define Getter(type, name, field) \
	property type name \
	{ \
		type get() \
		{ \
			type result; \
			diaSymbol->get_##field(&result); \
			return result; \
		} \
	}

namespace Tools
{
    namespace Pdb
    {
        public enum class PdbSymbolTag
        {
            Null,
            Exe,
            Compiland,
            CompilandDetails,
            CompilandEnv,
            Function,
            Block,
            Data,
            Annotation,
            Label,
            PublicSymbol,
            UDT,
            Enum,
            FunctionType,
            PointerType,
            ArrayType,
            BaseType,
            Typedef,
            BaseClass,
            Friend,
            FunctionArgType,
            FuncDebugStart,
            FuncDebugEnd,
            UsingNamespace,
            VTableShape,
            VTable,
            Custom,
            Thunk,
            CustomType,
            ManagedType,
            Dimension,
            CallSite,
            InlineSite,
            BaseInterface,
            VectorType,
            MatrixType,
            HLSLType,
            Caller,
            Callee,
            Export,
            HeapAllocationSite
        };
        public enum class PdbSymbolDataKind
        {
            Unknown,
            Local,
            StaticLocal,
            Param,
            ObjectPtr,
            FileStatic,
            Global,
            Member,
            StaticMember,
            Constant
        };
        public enum class PdbSymbolBaseType
        {
            NoType = 0,
            Void = 1,
            Char = 2,
            WChar = 3,
            Int = 6,
            UInt = 7,
            Float = 8,
            BCD = 9,
            Bool = 10,
            Long = 13,
            ULong = 14,
            Currency = 25,
            Date = 26,
            Variant = 27,
            Complex = 28,
            Bit = 29,
            BSTR = 30,
            Hresult = 31
        };

        public ref class PdbSymbol
        {
        public:

            property PdbSymbol^ LexicalParent
            {
                PdbSymbol^ get()
                {
                    IDiaSymbol* result = nullptr;
                    diaSymbol->get_lexicalParent(&result);
                    return result ? gcnew PdbSymbol(result) : nullptr;
                }
            }

            Getter(DWORD, SymIndexId, symIndexId);
            EnumGetter(PdbSymbolTag, SymTag, symTag);
            StringGetter(Name, name);
            //SymbolGetter(LexicalParent, lexicalParent);
            SymbolGetter(ClassParent, classParent);
            SymbolGetter(Type, type);
            EnumGetter(PdbSymbolDataKind, DataKind, dataKind);
            Getter(DWORD, LocationType, locationType);
            Getter(DWORD, AddressSection, addressSection);
            Getter(DWORD, AddressOffset, addressOffset);
            Getter(DWORD, RelativeVirtualAddress, relativeVirtualAddress);
            Getter(ULONGLONG, VirtualAddress, virtualAddress);
            Getter(DWORD, RegisterId, registerId);
            Getter(LONG, Offset, offset);
            Getter(ULONGLONG, Length, length);
            Getter(DWORD, Slot, slot);
            BoolGetter(VolatileType, volatileType);
            BoolGetter(ConstType, constType);
            BoolGetter(UnalignedType, unalignedType);
            Getter(DWORD, Access, access);
            StringGetter(LibraryName, libraryName);
            Getter(DWORD, Platform, platform);
            Getter(DWORD, Language, language);
            BoolGetter(EditAndContinueEnabled, editAndContinueEnabled);
            Getter(DWORD, FrontEndMajor, frontEndMajor);
            Getter(DWORD, FrontEndMinor, frontEndMinor);
            Getter(DWORD, FrontEndBuild, frontEndBuild);
            Getter(DWORD, BackEndMajor, backEndMajor);
            Getter(DWORD, BackEndMinor, backEndMinor);
            Getter(DWORD, BackEndBuild, backEndBuild);
            StringGetter(SourceFileName, sourceFileName);
            StringGetter(Unused, unused);
            Getter(DWORD, ThunkOrdinal, thunkOrdinal);
            Getter(LONG, ThisAdjust, thisAdjust);
            Getter(DWORD, VirtualBaseOffset, virtualBaseOffset);
            BoolGetter(Virtual, virtual);
            BoolGetter(Intro, intro);
            BoolGetter(Pure, pure);
            Getter(DWORD, CallingConvention, callingConvention);
            Getter(VARIANT, Value, value);
            EnumGetter(PdbSymbolBaseType, BaseType, baseType);
            Getter(DWORD, Token, token);
            Getter(DWORD, TimeStamp, timeStamp);
            Getter(GUID, Guid, guid);
            StringGetter(SymbolsFileName, symbolsFileName);
            BoolGetter(Reference, reference);
            Getter(DWORD, Count, count);
            Getter(DWORD, BitPosition, bitPosition);
            SymbolGetter(ArrayIndexType, arrayIndexType);
            BoolGetter(Packed, packed);
            BoolGetter(Constructor, constructor);
            BoolGetter(OverloadedOperator, overloadedOperator);
            BoolGetter(Nested, nested);
            BoolGetter(HasNestedTypes, hasNestedTypes);
            BoolGetter(HasAssignmentOperator, hasAssignmentOperator);
            BoolGetter(HasCastOperator, hasCastOperator);
            BoolGetter(Scoped, scoped);
            BoolGetter(VirtualBaseClass, virtualBaseClass);
            BoolGetter(IndirectVirtualBaseClass, indirectVirtualBaseClass);
            Getter(LONG, VirtualBasePointerOffset, virtualBasePointerOffset);
            SymbolGetter(VirtualTableShape, virtualTableShape);
            Getter(DWORD, LexicalParentId, lexicalParentId);
            Getter(DWORD, ClassParentId, classParentId);
            Getter(DWORD, TypeId, typeId);
            Getter(DWORD, ArrayIndexTypeId, arrayIndexTypeId);
            Getter(DWORD, VirtualTableShapeId, virtualTableShapeId);
            BoolGetter(Code, code);
            BoolGetter(Function, function);
            BoolGetter(Managed, managed);
            BoolGetter(Msil, msil);
            Getter(DWORD, VirtualBaseDispIndex, virtualBaseDispIndex);
            StringGetter(UndecoratedName, undecoratedName);
            Getter(DWORD, Age, age);
            Getter(DWORD, Signature, signature);
            BoolGetter(CompilerGenerated, compilerGenerated);
            BoolGetter(AddressTaken, addressTaken);
            Getter(DWORD, Rank, rank);
            SymbolGetter(LowerBound, lowerBound);
            SymbolGetter(UpperBound, upperBound);
            Getter(DWORD, LowerBoundId, lowerBoundId);
            Getter(DWORD, UpperBoundId, upperBoundId);
            Getter(DWORD, TargetSection, targetSection);
            Getter(DWORD, TargetOffset, targetOffset);
            Getter(DWORD, TargetRelativeVirtualAddress, targetRelativeVirtualAddress);
            Getter(ULONGLONG, TargetVirtualAddress, targetVirtualAddress);
            Getter(DWORD, MachineType, machineType);
            Getter(DWORD, OemId, oemId);
            Getter(DWORD, OemSymbolId, oemSymbolId);
            SymbolGetter(ObjectPointerType, objectPointerType);
            Getter(DWORD, UdtKind, udtKind);
            BoolGetter(NoReturn, noReturn);
            BoolGetter(CustomCallingConvention, customCallingConvention);
            BoolGetter(NoInline, noInline);
            BoolGetter(OptimizedCodeDebugInfo, optimizedCodeDebugInfo);
            BoolGetter(NotReached, notReached);
            BoolGetter(InterruptReturn, interruptReturn);
            BoolGetter(FarReturn, farReturn);
            BoolGetter(IsStatic, isStatic);
            BoolGetter(HasDebugInfo, hasDebugInfo);
            BoolGetter(IsLTCG, isLTCG);
            BoolGetter(IsDataAligned, isDataAligned);
            BoolGetter(HasSecurityChecks, hasSecurityChecks);
            Getter(BSTR, CompilerName, compilerName);
            BoolGetter(HasAlloca, hasAlloca);
            BoolGetter(HasSetJump, hasSetJump);
            BoolGetter(HasLongJump, hasLongJump);
            BoolGetter(HasInlAsm, hasInlAsm);
            BoolGetter(HasEH, hasEH);
            BoolGetter(HasSEH, hasSEH);
            BoolGetter(HasEHa, hasEHa);
            BoolGetter(IsNaked, isNaked);
            BoolGetter(IsAggregated, isAggregated);
            BoolGetter(IsSplitted, isSplitted);
            SymbolGetter(Container, container);
            BoolGetter(InlSpec, inlSpec);
            BoolGetter(NoStackOrdering, noStackOrdering);
            SymbolGetter(VirtualBaseTableType, virtualBaseTableType);
            BoolGetter(HasManagedCode, hasManagedCode);
            BoolGetter(IsHotpatchable, isHotpatchable);
            BoolGetter(IsCVTCIL, isCVTCIL);
            BoolGetter(IsMSILNetmodule, isMSILNetmodule);
            BoolGetter(IsCTypes, isCTypes);
            BoolGetter(IsStripped, isStripped);
            Getter(DWORD, FrontEndQFE, frontEndQFE);
            Getter(DWORD, BackEndQFE, backEndQFE);
            BoolGetter(WasInlined, wasInlined);
            BoolGetter(StrictGSCheck, strictGSCheck);
            BoolGetter(IsCxxReturnUdt, isCxxReturnUdt);
            BoolGetter(IsConstructorVirtualBase, isConstructorVirtualBase);
            BoolGetter(RValueReference, RValueReference);
            SymbolGetter(UnmodifiedType, unmodifiedType);
            BoolGetter(FramePointerPresent, framePointerPresent);
            BoolGetter(IsSafeBuffers, isSafeBuffers);
            BoolGetter(Intrinsic, intrinsic);
            BoolGetter(Sealed, sealed);
            BoolGetter(HfaFloat, hfaFloat);
            BoolGetter(HfaDouble, hfaDouble);
            Getter(DWORD, LiveRangeStartAddressSection, liveRangeStartAddressSection);
            Getter(DWORD, LiveRangeStartAddressOffset, liveRangeStartAddressOffset);
            Getter(DWORD, LiveRangeStartRelativeVirtualAddress, liveRangeStartRelativeVirtualAddress);
            Getter(DWORD, CountLiveRanges, countLiveRanges);
            Getter(ULONGLONG, LiveRangeLength, liveRangeLength);
            Getter(DWORD, OffsetInUdt, offsetInUdt);
            Getter(DWORD, ParamBasePointerRegisterId, paramBasePointerRegisterId);
            Getter(DWORD, LocalBasePointerRegisterId, localBasePointerRegisterId);
            BoolGetter(IsLocationControlFlowDependent, isLocationControlFlowDependent);
            Getter(DWORD, Stride, stride);
            Getter(DWORD, NumberOfRows, numberOfRows);
            Getter(DWORD, NumberOfColumns, numberOfColumns);
            BoolGetter(IsMatrixRowMajor, isMatrixRowMajor);
            BoolGetter(IsReturnValue, isReturnValue);
            BoolGetter(IsOptimizedAway, isOptimizedAway);
            Getter(DWORD, BuiltInKind, builtInKind);
            Getter(DWORD, RegisterType, registerType);
            Getter(DWORD, BaseDataSlot, baseDataSlot);
            Getter(DWORD, BaseDataOffset, baseDataOffset);
            Getter(DWORD, TextureSlot, textureSlot);
            Getter(DWORD, SamplerSlot, samplerSlot);
            Getter(DWORD, UavSlot, uavSlot);
            Getter(DWORD, SizeInUdt, sizeInUdt);
            Getter(DWORD, MemorySpaceKind, memorySpaceKind);
            Getter(DWORD, UnmodifiedTypeId, unmodifiedTypeId);
            Getter(DWORD, SubTypeId, subTypeId);
            SymbolGetter(SubType, subType);
            Getter(DWORD, NumberOfModifiers, numberOfModifiers);
            Getter(DWORD, NumberOfRegisterIndices, numberOfRegisterIndices);
            BoolGetter(IsHLSLData, isHLSLData);
            BoolGetter(IsPointerToDataMember, isPointerToDataMember);
            BoolGetter(IsPointerToMemberFunction, isPointerToMemberFunction);
            BoolGetter(IsSingleInheritance, isSingleInheritance);
            BoolGetter(IsMultipleInheritance, isMultipleInheritance);
            BoolGetter(IsVirtualInheritance, isVirtualInheritance);
            BoolGetter(RestrictedType, restrictedType);
            BoolGetter(IsPointerBasedOnSymbolValue, isPointerBasedOnSymbolValue);
            SymbolGetter(BaseSymbol, baseSymbol);
            Getter(DWORD, BaseSymbolId, baseSymbolId);
            StringGetter(ObjectFileName, objectFileName);
            BoolGetter(IsAcceleratorGroupSharedLocal, isAcceleratorGroupSharedLocal);
            BoolGetter(IsAcceleratorPointerTagLiveRange, isAcceleratorPointerTagLiveRange);
            BoolGetter(IsAcceleratorStubFunction, isAcceleratorStubFunction);
            Getter(DWORD, NumberOfAcceleratorPointerTags, numberOfAcceleratorPointerTags);
            BoolGetter(IsSdl, isSdl);
            BoolGetter(IsWinRTPointer, isWinRTPointer);
            BoolGetter(IsRefUdt, isRefUdt);
            BoolGetter(IsValueUdt, isValueUdt);
            BoolGetter(IsInterfaceUdt, isInterfaceUdt);
            BoolGetter(IsPGO, isPGO);
            BoolGetter(HasValidPGOCounts, hasValidPGOCounts);
            BoolGetter(IsOptimizedForSpeed, isOptimizedForSpeed);
            Getter(DWORD, PGOEntryCount, PGOEntryCount);
            Getter(DWORD, PGOEdgeCount, PGOEdgeCount);
            Getter(ULONGLONG, PGODynamicInstructionCount, PGODynamicInstructionCount);
            Getter(DWORD, StaticSize, staticSize);
            Getter(DWORD, FinalLiveStaticSize, finalLiveStaticSize);
            StringGetter(PhaseName, phaseName);
            BoolGetter(HasControlFlowCheck, hasControlFlowCheck);
            BoolGetter(ConstantExport, constantExport);
            BoolGetter(DataExport, dataExport);
            BoolGetter(PrivateExport, privateExport);
            BoolGetter(NoNameExport, noNameExport);
            BoolGetter(ExportHasExplicitlyAssignedOrdinal, exportHasExplicitlyAssignedOrdinal);
            BoolGetter(ExportIsForwarder, exportIsForwarder);
            Getter(DWORD, Ordinal, ordinal);
            Getter(DWORD, FrameSize, frameSize);
            Getter(DWORD, ExceptionHandlerAddressSection, exceptionHandlerAddressSection);
            Getter(DWORD, ExceptionHandlerAddressOffset, exceptionHandlerAddressOffset);
            Getter(DWORD, ExceptionHandlerRelativeVirtualAddress, exceptionHandlerRelativeVirtualAddress);
            Getter(ULONGLONG, ExceptionHandlerVirtualAddress, exceptionHandlerVirtualAddress);

        internal:
            IDiaSymbol* diaSymbol;

        internal:
            PdbSymbol(IDiaSymbol* diaSymbol);

        public:
            IEnumerable<PdbSymbol^>^ FindChildren(PdbSymbolTag tag);
            IEnumerable<PdbSymbol^>^ FindChildren(PdbSymbolTag tag, String^ name);

            // GetTypes
            // GetTypeIds
            // GetUndecoratedNameEx
            // GetNumericProperties
            // GetModifierValues

            // findInlineFramesByAddr
            // findInlineFramesByRVA
            // findInlineFramesByVA
            // findInlineeLines
            // findInlineeLinesByAddr
            // findInlineeLinesByRVA
            // findInlineeLinesByVA
            // findSymbolsForAcceleratorPointerTag
            // findSymbolsByRVAForAcceleratorPointerTag
            // get_acceleratorPointerTags
            // getSrcLineOnTypeDefn
            // findInputAssemblyFile

            String^ ToString() override
            {
                return Name;
            }
        };

        public ref class PdbSymbolCollection : public IEnumerable<PdbSymbol^>
        {
            ref class Enumerator : public IEnumerator<PdbSymbol^>
            {
            public:
                property PdbSymbol^ Current
                {
                    virtual PdbSymbol^ get()
                    {
                        return diaSymbol ? gcnew PdbSymbol(diaSymbol) : nullptr;
                    };
                };
                property Object^ Current2
                {
                    virtual Object^ get() = System::Collections::IEnumerator::Current::get
                    {
                        return diaSymbol ? gcnew PdbSymbol(diaSymbol) : nullptr;
                    };
                };

            private:
                IDiaEnumSymbols* diaEnumSymbols;
                IDiaSymbol* diaSymbol;

            internal:
                Enumerator(IDiaEnumSymbols* diaEnumSymbols);
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
                    diaEnumSymbols->get_Count(&result);
                    return result;
                }
            }

        private:
            IDiaEnumSymbols* diaEnumSymbols;

        internal:
            PdbSymbolCollection(IDiaEnumSymbols* diaEnumSymbols);

        public:
            virtual IEnumerator<PdbSymbol^>^ GetEnumerator();
            virtual System::Collections::IEnumerator^ GetEnumerator2() = System::Collections::IEnumerable::GetEnumerator;
        };
    }
}

#endif