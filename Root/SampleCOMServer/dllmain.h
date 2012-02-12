// dllmain.h : Declaration of module class.

class CSampleCOMServerModule : public ATL::CAtlDllModuleT< CSampleCOMServerModule >
{
public :
	DECLARE_LIBID(LIBID_SampleCOMServerLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_SAMPLECOMSERVER, "{16CBA31F-E33E-496E-86B9-0BD8F966A772}")
};

extern class CSampleCOMServerModule _AtlModule;
