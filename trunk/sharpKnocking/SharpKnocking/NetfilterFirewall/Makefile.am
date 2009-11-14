
EXTRA_DIST = 

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG
ASSEMBLY = bin/Debug/NetfilterFirewall.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug


endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/NetfilterFirewall.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release


endif


LINUX_PKGCONFIG = \
	$(NETFILTERFIREWALL_PC)  


	
all: $(ASSEMBLY) $(LINUX_PKGCONFIG) 

FILES =  \
	AssemblyInfo.cs \
	Commands/AppendRuleCommand.cs \
	Commands/DeleteChainCommand.cs \
	Commands/DeleteRuleCommand.cs \
	Commands/FlushChainCommand.cs \
	Commands/GenericCommand.cs \
	Commands/InsertRuleCommand.cs \
	Commands/ListChainCommand.cs \
	Commands/NewChainCommand.cs \
	Commands/RenameChainCommand.cs \
	Commands/ReplaceRuleCommand.cs \
	Commands/RuleCommandFactory.cs \
	Commands/SetChainPolicyCommand.cs \
	Commands/ZeroChainCommand.cs \
	Definitions.cs \
	DuplicateElementExcepcion.cs \
	FirewallManager.cs \
	GenericParameter.cs \
	GenericParameterList.cs \
	GenericParameterListEventArgs.cs \
	IpTablesCmd.cs \
	MatchExtensions/Definitions.cs \
	MatchExtensions/IcmpMatchExtension.cs \
	MatchExtensions/MatchExtensionEventArgs.cs \
	MatchExtensions/MatchExtensionFactory.cs \
	MatchExtensions/MatchExtensionHandler.cs \
	MatchExtensions/MatchExtensionList.cs \
	MatchExtensions/MatchExtensionParameter.cs \
	MatchExtensions/MatchExtensionParameterEventArgs.cs \
	MatchExtensions/MatchExtensionParameterList.cs \
	MatchExtensions/ReadOnlyMatchExtensionListAdapter.cs \
	MatchExtensions/StateMatchExtension.cs \
	MatchExtensions/TcpMatchExtension.cs \
	MatchExtensions/UdpMatchExtension.cs \
	NetfilterChain.cs \
	NetfilterException.cs \
	NetfilterRule.cs \
	NetfilterRuleList.cs \
	NetfilterRuleSet.cs \
	NetfilterTable.cs \
	NetfilterTarget.cs \
	Options/DestinationOption.cs \
	Options/FragmentOption.cs \
	Options/GenericOption.cs \
	Options/GenericOptionListEventArgs.cs \
	Options/GotoOption.cs \
	Options/InInterfaceOption.cs \
	Options/JumpOption.cs \
	Options/MatchExtensionOption.cs \
	Options/OutInterfaceOption.cs \
	Options/ProtocolOption.cs \
	Options/RuleOptionFactory.cs \
	Options/RuleOptionList.cs \
	Options/SetCountersOption.cs \
	Options/SourceOption.cs \
	ProtocolTypeEnum.cs \
	RuleException.cs \
	RuleParser.cs \
	TargetExtensions/BalanceTargetExtension.cs \
	TargetExtensions/ClassifyTargetExtension.cs \
	TargetExtensions/ClusterIPTargetExtension.cs \
	TargetExtensions/Definitions.cs \
	TargetExtensions/DnatTargetExtension.cs \
	TargetExtensions/DscpTargetExtension.cs \
	TargetExtensions/EcnTargetExtension.cs \
	TargetExtensions/IpMarkTargetExtension.cs \
	TargetExtensions/ReadOnlyTargetExtensionListAdapter.cs \
	TargetExtensions/RejectTargetExtension.cs \
	TargetExtensions/TargetExtensionEventArgs.cs \
	TargetExtensions/TargetExtensionFactory.cs \
	TargetExtensions/TargetExtensionHandler.cs \
	TargetExtensions/TargetExtensionList.cs \
	TargetExtensions/TargetExtensionParameter.cs \
	TargetExtensions/TargetExtensionParameterEventArgs.cs \
	TargetExtensions/TargetExtensionParameterList.cs \
	TargetExtensions/UlogTargetExtension.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	netfilterfirewall.pc.in 

REFERENCES =  \
	-r:Mono.Posix \
	-r:System

DLL_REFERENCES = 

NETFILTERFIREWALL_PC = $(BUILD_DIR)/netfilterfirewall.pc

$(NETFILTERFIREWALL_PC): netfilterfirewall.pc
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