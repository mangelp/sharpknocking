
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

PROGRAMFILES = \
	SharpKnocking.Common.dll \
	SharpKnocking.Common.dll.mdb  
BINARIES = \
	knockingdaemoncontrol  

	
all: $(ASSEMBLY)

FILES = \
	./Main.cs \
	./AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	SharpKnocking.Common.dll \
	SharpKnocking.Common.dll.mdb \
	knockingdaemoncontrol.in 

REFERENCES =  \
	-r:System \
	-r:System.Runtime.Remoting \
	-r:System.Runtime.Serialization.Formatters.Soap

DLL_REFERENCES = 

$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		

	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
