
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/Doorman.exe
COMPILE_TARGET = exe
ASSEMBLY_WRAPPER = doorman
ASSEMBLY_WRAPPER_IN = doorman.in
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll \
	../CommonWidgets/bin/Debug/CommonWidgets.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/Doorman.exe
COMPILE_TARGET = exe
ASSEMBLY_WRAPPER = doorman
ASSEMBLY_WRAPPER_IN = doorman.in
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll \
	../CommonWidgets/bin/Release/CommonWidgets.dll
BUILD_DIR = ./bin/Release
endif

	
all: $(ASSEMBLY)

FILES = \
	./Main.cs \
	./MainWindow.cs \
	./CallNode.cs \
	./Remoting/DaemonCommunication.cs \
	./ExportCallFileChooserDialog.cs \
	./TrayIcon.cs \
	./Remoting/DaemonCommunicationEvents.cs 

DATA_FILES = 

RESOURCES = \
	./gui.glade 

EXTRAS = 

REFERENCES =  \
	$(GTK_SHARP_20_LIBS) \
	$(GLADE_SHARP_20_LIBS) \
	$(GLIB_SHARP_20_LIBS) \
	-r:System \
	-r:System.Xml \
	-r:System.Runtime.Remoting

DLL_REFERENCES = 

$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		

	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
