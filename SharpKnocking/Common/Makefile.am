
EXTRA_DIST = 

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG
ASSEMBLY = bin/Debug/SharpKnocking.Common.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug


endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/SharpKnocking.Common.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release


endif


LINUX_PKGCONFIG = \
	$(COMMON_PC)  


	
all: $(ASSEMBLY) $(LINUX_PKGCONFIG) 

FILES = \
	TypeUtil.cs \
	AssemblyInfo.cs \
	AliasAttribute.cs \
	Debug.cs \
	Definitions.cs \
	Net20.cs \
	Net/IpAddressRange.cs \
	Net/NetMask.cs \
	Calls/CallSequence.cs \
	Remoting/Definitions.cs \
	WhichWrapper.cs \
	Remoting/RemoteEnd.cs \
	Remoting/RemoteEndEventArgs.cs \
	Remoting/RemoteEndService.cs \
	UnixNative.cs \
	Remoting/RemoteManager.cs \
	Remoting/RemoteDaemon.cs \
	Remoting/RemotingCommunicator.cs \
	Remoting/RemotingCommunicatorEvents.cs \
	Calls/CallHitData.cs \
	Net/Structs.cs \
	Config/DoormanConfig.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	common.pc.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Runtime.Remoting \
	System.Runtime.Serialization.Formatters.Soap \
	Mono.Posix

DLL_REFERENCES = 

COMMON_PC = $(BUILD_DIR)/common.pc

$(COMMON_PC): common.pc
	mkdir -p $(BUILD_DIR)
	cp '$<' '$@'



$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(build_resx_resources) : %.resources: %.resx
	resgen '$<' '$@'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list)
	mkdir -p $(dir $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include