
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/CommonWidgets.dll
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/CommonWidgets.dll
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Release
endif

PROGRAMFILES = \
	$(SHARPKNOCKING_COMMON_DLL) \
	$(SHARPKNOCKING_COMMON_DLL_MDB)  
LINUX_PKGCONFIG = \
	$(COMMONWIDGETS_PC)  

	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	./ImageResources.cs \
	./CallEditDialog.cs \
	./ImportCallFileChooserDialog.cs \
	./PortDialog.cs \
	./RandomPortGeneratorDialog.cs \
	./CommonDialogs/OkDialog.cs \
	./CommonDialogs/ConfirmDialog.cs \
	./CommonDialogs/AppInfoDialog.cs \
	./NodeStoreFilter.cs \
	./AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = \
	./gui.glade \
	./resources/sharpknocking22.png \
	./resources/sharpknocking96.png 

EXTRAS = \
	./commonwidgets.pc.in 

REFERENCES =  \
	-r:System \
	$(GTK_SHARP_20_LIBS) \
	$(GLIB_SHARP_20_LIBS) \
	$(GLADE_SHARP_20_LIBS)

DLL_REFERENCES = 

SHARPKNOCKING_COMMON_DLL = $(BUILD_DIR)/SharpKnocking.Common.dll
$(SHARPKNOCKING_COMMON_DLL): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll $(SHARPKNOCKING_COMMON_DLL)

SHARPKNOCKING_COMMON_DLL_MDB = $(BUILD_DIR)/SharpKnocking.Common.dll.mdb
$(SHARPKNOCKING_COMMON_DLL_MDB): $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb
	mkdir -p $(BUILD_DIR)
	cp $(top_builddir)/Common/bin/Debug/SharpKnocking.Common.dll.mdb $(SHARPKNOCKING_COMMON_DLL_MDB)

COMMONWIDGETS_PC = $(BUILD_DIR)/commonwidgets.pc
$(COMMONWIDGETS_PC): commonwidgets.pc
	mkdir -p $(BUILD_DIR)
	cp commonwidgets.pc $(COMMONWIDGETS_PC)



$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
