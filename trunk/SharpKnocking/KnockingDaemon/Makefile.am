
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG "-main:SharpKnocking.KnockingDaemon.MainClass"

ASSEMBLY = ./bin/Debug/KnockingDaemon.exe
COMPILE_TARGET = exe
ASSEMBLY_WRAPPER = knockingdaemon
ASSEMBLY_WRAPPER_IN = knockingdaemon.in
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll \
	../NetfilterFirewall/bin/Debug/NetfilterFirewall.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/KnockingDaemon.exe
COMPILE_TARGET = exe
ASSEMBLY_WRAPPER = knockingdaemon
ASSEMBLY_WRAPPER_IN = knockingdaemon.in
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll \
	../NetfilterFirewall/bin/Release/NetfilterFirewall.dll
BUILD_DIR = ./bin/Release
endif

	
all: $(ASSEMBLY)

FILES = \
	./Main.cs \
	./AssemblyInfo.cs \
	./KnockingDaemonProcess.cs \
	./PacketFilter/TcpdumpMonitor.cs \
	./PacketFilter/PacketInfo.cs \
	./PacketFilter/PacketAssembler.cs \
	./SequenceDetection/SequenceDetectorManager.cs \
	./SequenceDetection/SequenceDetector.cs \
	./PacketFilter/PacketFilterEvents.cs \
	./SequenceDetection/SequenceDetectorEvents.cs \
	./FirewallAccessor/NetfilterAccessor.cs \
	./PortInverseResolver.cs \
	./CallsLoader.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	-r:System \
	-r:System.Runtime.Remoting \
	-r:System.Runtime.Serialization.Formatters.Soap \
	-r:System.Messaging \
	-r:System.Security \
	-r:Mono.Posix

DLL_REFERENCES = 

$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		

	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
