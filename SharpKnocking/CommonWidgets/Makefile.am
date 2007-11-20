
EXTRA_DIST = 

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG
ASSEMBLY = bin/Debug/CommonWidgets.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug


endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/CommonWidgets.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release


endif


LINUX_PKGCONFIG = \
	$(COMMONWIDGETS_PC)  


	
all: $(ASSEMBLY) $(LINUX_PKGCONFIG) 

FILES = \
	ImageResources.cs \
	CallEditDialog.cs \
	ImportCallFileChooserDialog.cs \
	PortDialog.cs \
	RandomPortGeneratorDialog.cs \
	CommonDialogs/OkDialog.cs \
	CommonDialogs/ConfirmDialog.cs \
	CommonDialogs/AppInfoDialog.cs \
	NodeStoreFilter.cs \
	AssemblyInfo.cs \
	gtk-gui/generated.cs 

DATA_FILES = 

RESOURCES = \
	gui.glade \
	resources/sharpknocking22.png \
	resources/sharpknocking96.png \
	gtk-gui/gui.stetic \
	gtk-gui/objects.xml 

EXTRAS = \
	commonwidgets.pc.in 

REFERENCES =  \
	System \
	$(GTK_SHARP_20_LIBS) \
	$(GLIB_SHARP_20_LIBS) \
	$(GLADE_SHARP_20_LIBS) \
	Mono.Posix

DLL_REFERENCES = 

COMMONWIDGETS_PC = $(BUILD_DIR)/commonwidgets.pc

$(COMMONWIDGETS_PC): commonwidgets.pc
	mkdir -p $(BUILD_DIR)
	cp '$<' '$@'



$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(build_resx_resources) : %.resources: %.resx
	resgen '$<' '$@'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list)
	mkdir -p $(dir $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include