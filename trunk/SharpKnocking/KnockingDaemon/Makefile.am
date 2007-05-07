
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG "-main:SharpKnocking.KnockingDaemon.MainClass"

ASSEMBLY = ./bin/Debug/KnockingDaemon.exe
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll \
	../NetfilterFirewall/bin/Debug/NetfilterFirewall.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/KnockingDaemon.exe
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll \
	../NetfilterFirewall/bin/Release/NetfilterFirewall.dll
BUILD_DIR = ./bin/Release
endif

BINARIES = \
	$(KNOCKINGDAEMON)  
PROGRAMFILES = \
	$(SHARPKNOCKING_COMMON_DLL) \
	$(SHARPKNOCKING_COMMON_DLL_MDB) \
	$(NETFILTERFIREWALL_DLL) \
	$(NETFILTERFIREWALL_DLL_MDB) \
	$(SHARPKNOCKING_COMMON_DLL_1) \
	$(SHARPKNOCKING_COMMON_DLL_MDB_1)  

	
all: $(ASSEMBLY) $(BINARIES) $(PROGRAMFILES) 

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

EXTRAS = \
	./knockingdaemon.in 

REFERENCES =  \
	-r:System \
	-r:System.Runtime.Remoting \
	-r:System.Runtime.Serialization.Formatters.Soap \
	-r:System.Messaging \
	-r:System.Security \
	-r:Mono.Posix

DLL_REFERENCES = 

SHARPKNOCKING_COMMON_DLL = $(BUILD_DIR)/SharpKnocking.Common.dll
$(SHARPKNOCKING_COMMON_DLL): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll $(SHARPKNOCKING_COMMON_DLL)

SHARPKNOCKING_COMMON_DLL_MDB = $(BUILD_DIR)/SharpKnocking.Common.dll.mdb
$(SHARPKNOCKING_COMMON_DLL_MDB): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb $(SHARPKNOCKING_COMMON_DLL_MDB)

NETFILTERFIREWALL_DLL = $(BUILD_DIR)/NetfilterFirewall.dll
$(NETFILTERFIREWALL_DLL): $(top_builddir)/NetfilterFirewall/bin/Debug/NetfilterFirewall.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/NetfilterFirewall/bin/Debug/NetfilterFirewall.dll $(NETFILTERFIREWALL_DLL)

NETFILTERFIREWALL_DLL_MDB = $(BUILD_DIR)/NetfilterFirewall.dll.mdb
$(NETFILTERFIREWALL_DLL_MDB): $(top_builddir)/NetfilterFirewall/bin/Debug/NetfilterFirewall.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/NetfilterFirewall/bin/Debug/NetfilterFirewall.dll.mdb $(NETFILTERFIREWALL_DLL_MDB)

SHARPKNOCKING_COMMON_DLL_1 = $(BUILD_DIR)/SharpKnocking.Common.dll
$(SHARPKNOCKING_COMMON_DLL_1): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll $(SHARPKNOCKING_COMMON_DLL_1)

SHARPKNOCKING_COMMON_DLL_MDB_1 = $(BUILD_DIR)/SharpKnocking.Common.dll.mdb
$(SHARPKNOCKING_COMMON_DLL_MDB_1): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb $(SHARPKNOCKING_COMMON_DLL_MDB_1)

KNOCKINGDAEMON = $(BUILD_DIR)/knockingdaemon
$(KNOCKINGDAEMON): knockingdaemon
	mkdir -p $(BUILD_DIR)
	cp knockingdaemon $(KNOCKINGDAEMON)



$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
