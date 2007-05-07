
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/KnockingDaemonControl.exe
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/KnockingDaemonControl.exe
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Release
endif

BINARIES = \
	$(KNOCKINGDAEMONCONTROL)  
PROGRAMFILES = \
	$(SHARPKNOCKING_COMMON_DLL) \
	$(SHARPKNOCKING_COMMON_DLL_MDB)  

	
all: $(ASSEMBLY) $(BINARIES) $(PROGRAMFILES) 

FILES = \
	./Main.cs \
	./AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	./knockingdaemoncontrol.in 

REFERENCES =  \
	-r:System \
	-r:System.Runtime.Remoting \
	-r:System.Runtime.Serialization.Formatters.Soap

DLL_REFERENCES = 

SHARPKNOCKING_COMMON_DLL = $(BUILD_DIR)/SharpKnocking.Common.dll
$(SHARPKNOCKING_COMMON_DLL): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll $(SHARPKNOCKING_COMMON_DLL)

SHARPKNOCKING_COMMON_DLL_MDB = $(BUILD_DIR)/SharpKnocking.Common.dll.mdb
$(SHARPKNOCKING_COMMON_DLL_MDB): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb $(SHARPKNOCKING_COMMON_DLL_MDB)

KNOCKINGDAEMONCONTROL = $(BUILD_DIR)/knockingdaemoncontrol
$(KNOCKINGDAEMONCONTROL): knockingdaemoncontrol
	mkdir -p $(BUILD_DIR)
	cp knockingdaemoncontrol $(KNOCKINGDAEMONCONTROL)



$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
