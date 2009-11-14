
EXTRA_DIST = 

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG "-main:SharpKnocking.KnockingDaemon.MainClass"

ASSEMBLY = bin/Debug/KnockingDaemon.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../NetfilterFirewall/bin/Debug/NetfilterFirewall.dll
BUILD_DIR = bin/Debug

NETFILTERFIREWALL_DLL_MDB_SOURCE=../NetfilterFirewall/bin/Debug/NetfilterFirewall.dll.mdb
NETFILTERFIREWALL_DLL_MDB=$(BUILD_DIR)/NetfilterFirewall.dll.mdb
NETFILTERFIREWALL_DLL_SOURCE=../NetfilterFirewall/bin/Debug/NetfilterFirewall.dll

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/KnockingDaemon.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../NetfilterFirewall/bin/Release/NetfilterFirewall.dll
BUILD_DIR = bin/Release

NETFILTERFIREWALL_DLL_MDB=
NETFILTERFIREWALL_DLL_SOURCE=../NetfilterFirewall/bin/Release/NetfilterFirewall.dll

endif


PROGRAMFILES = \
	$(NETFILTERFIREWALL_DLL_MDB) \
	$(NETFILTERFIREWALL_DLL)  

BINARIES = \
	$(KNOCKINGDAEMON)  


	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES =  \
	AssemblyInfo.cs \
	CallsLoader.cs \
	FirewallAccessor/NetfilterAccessor.cs \
	KnockingDaemonProcess.cs \
	Main.cs \
	PacketFilter/PacketAssembler.cs \
	PacketFilter/PacketFilterEvents.cs \
	PacketFilter/PacketInfo.cs \
	PacketFilter/TcpdumpMonitor.cs \
	PortInverseResolver.cs \
	SequenceDetection/SequenceDetector.cs \
	SequenceDetection/SequenceDetectorEvents.cs \
	SequenceDetection/SequenceDetectorManager.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	knockingdaemon.in 

REFERENCES =  \
	-r:../NetfilterFirewall/bin/Debug/NetfilterFirewall.dll \
	-r:Mono.Posix \
	-r:System \
	-r:System.Messaging \
	-r:System.Runtime.Remoting \
	-r:System.Runtime.Serialization.Formatters.Soap \
	-r:System.Security

DLL_REFERENCES = 

KNOCKINGDAEMON = $(BUILD_DIR)/knockingdaemon
NETFILTERFIREWALL_DLL = $(BUILD_DIR)/NetfilterFirewall.dll

$(NETFILTERFIREWALL_DLL_MDB): $(NETFILTERFIREWALL_DLL_MDB_SOURCE)
	mkdir -p $(BUILD_DIR)
	cp '$<' '$@'

$(KNOCKINGDAEMON): knockingdaemon
	mkdir -p $(BUILD_DIR)
	cp '$<' '$@'

$(NETFILTERFIREWALL_DLL): $(NETFILTERFIREWALL_DLL_SOURCE)
	mkdir -p $(BUILD_DIR)
	cp '$<' '$@'



$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(build_resx_resources) : %.resources: %.resx
	resgen2 '$<' '$@'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list)
	mkdir -p $(dir $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include