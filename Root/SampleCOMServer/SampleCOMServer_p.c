

/* this ALWAYS GENERATED file contains the proxy stub code */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Sun Feb 12 19:29:49 2012
 */
/* Compiler settings for SampleCOMServer.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#if !defined(_M_IA64) && !defined(_M_AMD64)


#pragma warning( disable: 4049 )  /* more than 64k source lines */
#if _MSC_VER >= 1200
#pragma warning(push)
#endif

#pragma warning( disable: 4211 )  /* redefine extern to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma warning( disable: 4024 )  /* array to pointer mapping*/
#pragma warning( disable: 4152 )  /* function/data pointer conversion in expression */
#pragma warning( disable: 4100 ) /* unreferenced arguments in x86 call */

#pragma optimize("", off ) 

#define USE_STUBLESS_PROXY


/* verify that the <rpcproxy.h> version is high enough to compile this file*/
#ifndef __REDQ_RPCPROXY_H_VERSION__
#define __REQUIRED_RPCPROXY_H_VERSION__ 475
#endif


#include "rpcproxy.h"
#ifndef __RPCPROXY_H_VERSION__
#error this stub requires an updated version of <rpcproxy.h>
#endif /* __RPCPROXY_H_VERSION__ */


#include "SampleCOMServer_i.h"

#define TYPE_FORMAT_STRING_SIZE   3                                 
#define PROC_FORMAT_STRING_SIZE   31                                
#define EXPR_FORMAT_STRING_SIZE   1                                 
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   0            

typedef struct _SampleCOMServer_MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } SampleCOMServer_MIDL_TYPE_FORMAT_STRING;

typedef struct _SampleCOMServer_MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } SampleCOMServer_MIDL_PROC_FORMAT_STRING;

typedef struct _SampleCOMServer_MIDL_EXPR_FORMAT_STRING
    {
    long          Pad;
    unsigned char  Format[ EXPR_FORMAT_STRING_SIZE ];
    } SampleCOMServer_MIDL_EXPR_FORMAT_STRING;


static const RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const SampleCOMServer_MIDL_TYPE_FORMAT_STRING SampleCOMServer__MIDL_TypeFormatString;
extern const SampleCOMServer_MIDL_PROC_FORMAT_STRING SampleCOMServer__MIDL_ProcFormatString;
extern const SampleCOMServer_MIDL_EXPR_FORMAT_STRING SampleCOMServer__MIDL_ExprFormatString;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ISampleApartmentThreadedClass_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ISampleApartmentThreadedClass_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ISampleFreeThreadedClass_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ISampleFreeThreadedClass_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ISampleTestInterface_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ISampleTestInterface_ProxyInfo;



#if !defined(__RPC_WIN32__)
#error  Invalid build platform for this stub.
#endif

#if !(TARGET_IS_NT50_OR_LATER)
#error You need Windows 2000 or later to run this stub because it uses these features:
#error   /robust command line switch.
#error However, your C/C++ compilation flags indicate you intend to run this app on earlier systems.
#error This app will fail with the RPC_X_WRONG_STUB_VERSION error.
#endif


static const SampleCOMServer_MIDL_PROC_FORMAT_STRING SampleCOMServer__MIDL_ProcFormatString =
    {
        0,
        {

	/* Procedure TestInterface */


	/* Procedure Test */


	/* Procedure Test */

			0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x7 ),	/* 7 */
/*  8 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 10 */	NdrFcShort( 0x0 ),	/* 0 */
/* 12 */	NdrFcShort( 0x8 ),	/* 8 */
/* 14 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x1,		/* 1 */
/* 16 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 18 */	NdrFcShort( 0x0 ),	/* 0 */
/* 20 */	NdrFcShort( 0x0 ),	/* 0 */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Return value */


	/* Return value */


	/* Return value */

/* 24 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 26 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 28 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const SampleCOMServer_MIDL_TYPE_FORMAT_STRING SampleCOMServer__MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */

			0x0
        }
    };


/* Object interface: IUnknown, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IDispatch, ver. 0.0,
   GUID={0x00020400,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: ISampleApartmentThreadedClass, ver. 0.0,
   GUID={0x0BC51E46,0xAC50,0x4B42,{0xA8,0x00,0xBA,0xD9,0x20,0x73,0x6F,0xCA}} */

#pragma code_seg(".orpc")
static const unsigned short ISampleApartmentThreadedClass_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0
    };

static const MIDL_STUBLESS_PROXY_INFO ISampleApartmentThreadedClass_ProxyInfo =
    {
    &Object_StubDesc,
    SampleCOMServer__MIDL_ProcFormatString.Format,
    &ISampleApartmentThreadedClass_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ISampleApartmentThreadedClass_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    SampleCOMServer__MIDL_ProcFormatString.Format,
    &ISampleApartmentThreadedClass_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(8) _ISampleApartmentThreadedClassProxyVtbl = 
{
    &ISampleApartmentThreadedClass_ProxyInfo,
    &IID_ISampleApartmentThreadedClass,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* IDispatch::GetTypeInfoCount */ ,
    0 /* IDispatch::GetTypeInfo */ ,
    0 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */ ,
    (void *) (INT_PTR) -1 /* ISampleApartmentThreadedClass::Test */
};


static const PRPC_STUB_FUNCTION ISampleApartmentThreadedClass_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    NdrStubCall2
};

CInterfaceStubVtbl _ISampleApartmentThreadedClassStubVtbl =
{
    &IID_ISampleApartmentThreadedClass,
    &ISampleApartmentThreadedClass_ServerInfo,
    8,
    &ISampleApartmentThreadedClass_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};


/* Object interface: ISampleFreeThreadedClass, ver. 0.0,
   GUID={0xB4B37799,0xC3D3,0x4563,{0xB0,0x44,0xFB,0x10,0xDC,0xEC,0xE8,0xB5}} */

#pragma code_seg(".orpc")
static const unsigned short ISampleFreeThreadedClass_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0
    };

static const MIDL_STUBLESS_PROXY_INFO ISampleFreeThreadedClass_ProxyInfo =
    {
    &Object_StubDesc,
    SampleCOMServer__MIDL_ProcFormatString.Format,
    &ISampleFreeThreadedClass_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ISampleFreeThreadedClass_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    SampleCOMServer__MIDL_ProcFormatString.Format,
    &ISampleFreeThreadedClass_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(8) _ISampleFreeThreadedClassProxyVtbl = 
{
    &ISampleFreeThreadedClass_ProxyInfo,
    &IID_ISampleFreeThreadedClass,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* IDispatch::GetTypeInfoCount */ ,
    0 /* IDispatch::GetTypeInfo */ ,
    0 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */ ,
    (void *) (INT_PTR) -1 /* ISampleFreeThreadedClass::Test */
};


static const PRPC_STUB_FUNCTION ISampleFreeThreadedClass_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    NdrStubCall2
};

CInterfaceStubVtbl _ISampleFreeThreadedClassStubVtbl =
{
    &IID_ISampleFreeThreadedClass,
    &ISampleFreeThreadedClass_ServerInfo,
    8,
    &ISampleFreeThreadedClass_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};


/* Object interface: ISampleTestInterface, ver. 0.0,
   GUID={0x85FDCD3F,0x9628,0x4394,{0xB1,0x32,0x30,0x97,0xFC,0x95,0x29,0xBA}} */

#pragma code_seg(".orpc")
static const unsigned short ISampleTestInterface_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0
    };

static const MIDL_STUBLESS_PROXY_INFO ISampleTestInterface_ProxyInfo =
    {
    &Object_StubDesc,
    SampleCOMServer__MIDL_ProcFormatString.Format,
    &ISampleTestInterface_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ISampleTestInterface_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    SampleCOMServer__MIDL_ProcFormatString.Format,
    &ISampleTestInterface_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(8) _ISampleTestInterfaceProxyVtbl = 
{
    &ISampleTestInterface_ProxyInfo,
    &IID_ISampleTestInterface,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* IDispatch::GetTypeInfoCount */ ,
    0 /* IDispatch::GetTypeInfo */ ,
    0 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */ ,
    (void *) (INT_PTR) -1 /* ISampleTestInterface::TestInterface */
};


static const PRPC_STUB_FUNCTION ISampleTestInterface_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    NdrStubCall2
};

CInterfaceStubVtbl _ISampleTestInterfaceStubVtbl =
{
    &IID_ISampleTestInterface,
    &ISampleTestInterface_ServerInfo,
    8,
    &ISampleTestInterface_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};

static const MIDL_STUB_DESC Object_StubDesc = 
    {
    0,
    NdrOleAllocate,
    NdrOleFree,
    0,
    0,
    0,
    0,
    0,
    SampleCOMServer__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x50002, /* Ndr library version */
    0,
    0x700022b, /* MIDL Version 7.0.555 */
    0,
    0,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0
    };

const CInterfaceProxyVtbl * const _SampleCOMServer_ProxyVtblList[] = 
{
    ( CInterfaceProxyVtbl *) &_ISampleTestInterfaceProxyVtbl,
    ( CInterfaceProxyVtbl *) &_ISampleApartmentThreadedClassProxyVtbl,
    ( CInterfaceProxyVtbl *) &_ISampleFreeThreadedClassProxyVtbl,
    0
};

const CInterfaceStubVtbl * const _SampleCOMServer_StubVtblList[] = 
{
    ( CInterfaceStubVtbl *) &_ISampleTestInterfaceStubVtbl,
    ( CInterfaceStubVtbl *) &_ISampleApartmentThreadedClassStubVtbl,
    ( CInterfaceStubVtbl *) &_ISampleFreeThreadedClassStubVtbl,
    0
};

PCInterfaceName const _SampleCOMServer_InterfaceNamesList[] = 
{
    "ISampleTestInterface",
    "ISampleApartmentThreadedClass",
    "ISampleFreeThreadedClass",
    0
};

const IID *  const _SampleCOMServer_BaseIIDList[] = 
{
    &IID_IDispatch,
    &IID_IDispatch,
    &IID_IDispatch,
    0
};


#define _SampleCOMServer_CHECK_IID(n)	IID_GENERIC_CHECK_IID( _SampleCOMServer, pIID, n)

int __stdcall _SampleCOMServer_IID_Lookup( const IID * pIID, int * pIndex )
{
    IID_BS_LOOKUP_SETUP

    IID_BS_LOOKUP_INITIAL_TEST( _SampleCOMServer, 3, 2 )
    IID_BS_LOOKUP_NEXT_TEST( _SampleCOMServer, 1 )
    IID_BS_LOOKUP_RETURN_RESULT( _SampleCOMServer, 3, *pIndex )
    
}

const ExtendedProxyFileInfo SampleCOMServer_ProxyFileInfo = 
{
    (PCInterfaceProxyVtblList *) & _SampleCOMServer_ProxyVtblList,
    (PCInterfaceStubVtblList *) & _SampleCOMServer_StubVtblList,
    (const PCInterfaceName * ) & _SampleCOMServer_InterfaceNamesList,
    (const IID ** ) & _SampleCOMServer_BaseIIDList,
    & _SampleCOMServer_IID_Lookup, 
    3,
    2,
    0, /* table of [async_uuid] interfaces */
    0, /* Filler1 */
    0, /* Filler2 */
    0  /* Filler3 */
};
#pragma optimize("", on )
#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#endif /* !defined(_M_IA64) && !defined(_M_AMD64)*/

