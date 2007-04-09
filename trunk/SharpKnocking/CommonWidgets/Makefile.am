
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/CommonWidgets.dll
COMPILE_TARGET = library
PC_FILES_IN = SharpKnocking.CommonWidgets.pc.in
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/CommonWidgets.dll
COMPILE_TARGET = library
PC_FILES_IN = SharpKnocking.CommonWidgets.pc.in
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Release
endif

	
all: $(ASSEMBLY)

FILES = \
	./ImageResources.cs \
	./CallEditDialog.cs \
	./ImportCallFileChooserDialog.cs \
	./PortDialog.cs \
	./RandomPortGeneratorDialog.cs \
	./CommonDialogs/OkDialog.cs \
	./CommonDialogs/ConfirmDialog.cs \
	./CommonDialogs/AppInfoDialog.cs \
	./NodeStoreFilter.cs 

DATA_FILES = 

RESOURCES = \
	./gui.glade \
	./resources/sharpknocking22.png \
	./resources/sharpknocking96.png 

EXTRAS = 

REFERENCES =  \
	-r:System \
	$(GTK_SHARP_20_LIBS) \
	$(GLIB_SHARP_20_LIBS) \
	$(GLADE_SHARP_20_LIBS)

DLL_REFERENCES = 

$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		

	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
