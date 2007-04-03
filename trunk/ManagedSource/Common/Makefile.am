
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/SharpKnocking.Common.dll
COMPILE_TARGET = library
PC_FILES_IN = ManagedSource.Common.pc.in
PROJECT_REFERENCES = 
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/SharpKnocking.Common.dll
COMPILE_TARGET = library
PC_FILES_IN = ManagedSource.Common.pc.in
PROJECT_REFERENCES = 
BUILD_DIR = ./bin/Release
endif

	
all: $(ASSEMBLY)

FILES = \
	./TypeUtil.cs \
	./AssemblyInfo.cs \
	./AliasAttribute.cs \
	./Debug.cs \
	./Definitions.cs \
	./Net20.cs \
	./Net/IpAddressRange.cs \
	./Net/NetMask.cs \
	./Calls/CallSequence.cs \
	./DoormanConfig.cs \
	./Remoting/Definitions.cs \
	./WhichWrapper.cs \
	./Remoting/IRemoteEnd.cs \
	./Remoting/RemoteEnd.cs \
	./Remoting/RemoteEndEventArgs.cs \
	./Remoting/RemoteEndService.cs \
	./UnixNative.cs \
	./Remoting/IRemoteManager.cs \
	./Remoting/IRemoteDaemon.cs \
	./Remoting/RemoteManager.cs \
	./Remoting/RemoteDaemon.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	-r:System \
	-r:System.Xml \
	-r:System.Runtime.Remoting \
	-r:System.Runtime.Serialization.Formatters.Soap \
	-r:Mono.Posix

DLL_REFERENCES = 

$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		

	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
