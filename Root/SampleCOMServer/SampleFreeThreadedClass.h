// SampleFreeThreadedClass.h : Declaration of the CSampleFreeThreadedClass

#pragma once
#include "resource.h"       // main symbols



#include "SampleCOMServer_i.h"



using namespace ATL;


// CSampleFreeThreadedClass

class ATL_NO_VTABLE CSampleFreeThreadedClass :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CSampleFreeThreadedClass, &CLSID_SampleFreeThreadedClass>,
	public IDispatchImpl<ISampleFreeThreadedClass, &IID_ISampleFreeThreadedClass, &LIBID_SampleCOMServerLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IDispatchImpl<ISampleTestInterface, &__uuidof(ISampleTestInterface), &LIBID_SampleCOMServerLib, /* wMajor = */ 1, /* wMinor = */ 0>
{
public:
	CSampleFreeThreadedClass()
	{
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_SAMPLEFREETHREADEDCLASS)


	BEGIN_COM_MAP(CSampleFreeThreadedClass)
		COM_INTERFACE_ENTRY(ISampleFreeThreadedClass)
		COM_INTERFACE_ENTRY2(IDispatch, ISampleTestInterface)
		COM_INTERFACE_ENTRY(ISampleTestInterface)
	END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:



	STDMETHOD(Test)(void);

	// ISampleTestInterface Methods
public:
	STDMETHOD(TestInterface)()
	{
		// Add your function implementation here.
		return E_NOTIMPL;
	}
};

OBJECT_ENTRY_AUTO(__uuidof(SampleFreeThreadedClass), CSampleFreeThreadedClass)
