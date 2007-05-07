
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/Doorman.exe
COMPILE_TARGET = exe
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
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll \
	../CommonWidgets/bin/Release/CommonWidgets.dll
BUILD_DIR = ./bin/Release
endif

BINARIES = \
	$(DOORMAN)  
PROGRAMFILES = \
	$(SHARPKNOCKING_COMMON_DLL) \
	$(SHARPKNOCKING_COMMON_DLL_MDB) \
	$(COMMONWIDGETS_DLL) \
	$(COMMONWIDGETS_DLL_MDB) \
	$(SHARPKNOCKING_COMMON_DLL_1) \
	$(SHARPKNOCKING_COMMON_DLL_MDB_1)  

	
all: $(ASSEMBLY) $(BINARIES) $(PROGRAMFILES) 

FILES = \
	./Main.cs \
	./MainWindow.cs \
	./CallNode.cs \
	./Remoting/DaemonCommunication.cs \
	./ExportCallFileChooserDialog.cs \
	./TrayIcon.cs \
	./Remoting/DaemonCommunicationEvents.cs \
	./AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = \
	./gui.glade 

EXTRAS = \
	./doorman.in 

REFERENCES =  \
	$(GTK_SHARP_20_LIBS) \
	$(GLADE_SHARP_20_LIBS) \
	$(GLIB_SHARP_20_LIBS) \
	-r:System \
	-r:System.Xml \
	-r:System.Runtime.Remoting

DLL_REFERENCES = 

SHARPKNOCKING_COMMON_DLL = $(BUILD_DIR)/SharpKnocking.Common.dll
$(SHARPKNOCKING_COMMON_DLL): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll $(SHARPKNOCKING_COMMON_DLL)

SHARPKNOCKING_COMMON_DLL_MDB = $(BUILD_DIR)/SharpKnocking.Common.dll.mdb
$(SHARPKNOCKING_COMMON_DLL_MDB): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb $(SHARPKNOCKING_COMMON_DLL_MDB)

COMMONWIDGETS_DLL = $(BUILD_DIR)/CommonWidgets.dll
$(COMMONWIDGETS_DLL): $(top_builddir)/CommonWidgets/bin/Debug/CommonWidgets.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/CommonWidgets/bin/Debug/CommonWidgets.dll $(COMMONWIDGETS_DLL)

COMMONWIDGETS_DLL_MDB = $(BUILD_DIR)/CommonWidgets.dll.mdb
$(COMMONWIDGETS_DLL_MDB): $(top_builddir)/CommonWidgets/bin/Debug/CommonWidgets.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/CommonWidgets/bin/Debug/CommonWidgets.dll.mdb $(COMMONWIDGETS_DLL_MDB)

SHARPKNOCKING_COMMON_DLL_1 = $(BUILD_DIR)/SharpKnocking.Common.dll
$(SHARPKNOCKING_COMMON_DLL_1): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll $(SHARPKNOCKING_COMMON_DLL_1)

SHARPKNOCKING_COMMON_DLL_MDB_1 = $(BUILD_DIR)/SharpKnocking.Common.dll.mdb
$(SHARPKNOCKING_COMMON_DLL_MDB_1): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb $(SHARPKNOCKING_COMMON_DLL_MDB_1)

DOORMAN = $(BUILD_DIR)/doorman
$(DOORMAN): doorman
	mkdir -p $(BUILD_DIR)
	cp doorman $(DOORMAN)



$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
