// SampleApartmentThreadedClass.h : Declaration of the CSampleApartmentThreadedClass

#pragma once
#include "resource.h"       // main symbols



#include "SampleCOMServer_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CSampleApartmentThreadedClass

class ATL_NO_VTABLE CSampleApartmentThreadedClass :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CSampleApartmentThreadedClass, &CLSID_SampleApartmentThreadedClass>,
	public IDispatchImpl<ISampleApartmentThreadedClass, &IID_ISampleApartmentThreadedClass, &LIBID_SampleCOMServerLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IDispatchImpl<ISampleTestInterface, &__uuidof(ISampleTestInterface), &LIBID_SampleCOMServerLib, /* wMajor = */ 1, /* wMinor = */ 0>
{
public:
	CSampleApartmentThreadedClass()
	{
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_SAMPLEAPARTMENTTHREADEDCLASS)


	BEGIN_COM_MAP(CSampleApartmentThreadedClass)
		COM_INTERFACE_ENTRY(ISampleApartmentThreadedClass)
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

OBJECT_ENTRY_AUTO(__uuidof(SampleApartmentThreadedClass), CSampleApartmentThreadedClass)
