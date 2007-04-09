
# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -debug -d:DEBUG
ASSEMBLY = ./bin/Debug/NetfilterFirewall.dll
COMPILE_TARGET = library
PC_FILES_IN = SharpKnocking.NetfilterFirewall.pc.in
PROJECT_REFERENCES =  \
	../Common/bin/Debug/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Debug
endif
if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = mcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4
ASSEMBLY = ./bin/Release/NetfilterFirewall.dll
COMPILE_TARGET = library
PC_FILES_IN = SharpKnocking.NetfilterFirewall.pc.in
PROJECT_REFERENCES =  \
	../Common/bin/Release/SharpKnocking.Common.dll
BUILD_DIR = ./bin/Release
endif

	
all: $(ASSEMBLY)

FILES = \
	./AssemblyInfo.cs \
	./Definitions.cs \
	./NetfilterRule.cs \
	./NetfilterTable.cs \
	./NetfilterChain.cs \
	./DuplicateElementExcepcion.cs \
	./NetfilterException.cs \
	./NetfilterRuleSet.cs \
	./RuleParser.cs \
	./MatchExtensions/TcpMatchExtension.cs \
	./MatchExtensions/UdpMatchExtension.cs \
	./MatchExtensions/IcmpMatchExtension.cs \
	./MatchExtensions/MatchExtensionHandler.cs \
	./MatchExtensions/MatchExtensionFactory.cs \
	./MatchExtensions/Definitions.cs \
	./GenericParameter.cs \
	./RuleException.cs \
	./NetfilterRuleList.cs \
	./MatchExtensions/StateMatchExtension.cs \
	./GenericParameterList.cs \
	./GenericParameterListEventArgs.cs \
	./MatchExtensions/MatchExtensionList.cs \
	./MatchExtensions/MatchExtensionEventArgs.cs \
	./MatchExtensions/MatchExtensionParameter.cs \
	./MatchExtensions/MatchExtensionParameterList.cs \
	./MatchExtensions/MatchExtensionParameterEventArgs.cs \
	./Options/ProtocolOption.cs \
	./Options/SourceOption.cs \
	./Options/DestinationOption.cs \
	./Options/JumpOption.cs \
	./Options/InInterfaceOption.cs \
	./Options/OutInterfaceOption.cs \
	./Options/GotoOption.cs \
	./Options/FragmentOption.cs \
	./Options/SetCountersOption.cs \
	./Options/MatchExtensionOption.cs \
	./Options/GenericOption.cs \
	./Options/RuleOptionList.cs \
	./Options/GenericOptionListEventArgs.cs \
	./Options/RuleOptionFactory.cs \
	./Commands/GenericCommand.cs \
	./Commands/RuleCommandFactory.cs \
	./Commands/AppendRuleCommand.cs \
	./Commands/DeleteRuleCommand.cs \
	./Commands/DeleteChainCommand.cs \
	./Commands/ListChainCommand.cs \
	./Commands/FlushChainCommand.cs \
	./Commands/ZeroChainCommand.cs \
	./Commands/NewChainCommand.cs \
	./Commands/RenameChainCommand.cs \
	./Commands/SetChainPolicyCommand.cs \
	./Commands/InsertRuleCommand.cs \
	./Commands/ReplaceRuleCommand.cs \
	./NetfilterTarget.cs \
	./MatchExtensions/ReadOnlyMatchExtensionListAdapter.cs \
	./ProtocolTypeEnum.cs \
	./TargetExtensions/TargetExtensionFactory.cs \
	./TargetExtensions/TargetExtensionHandler.cs \
	./TargetExtensions/TargetExtensionList.cs \
	./TargetExtensions/TargetExtensionParameterList.cs \
	./TargetExtensions/ReadOnlyTargetExtensionListAdapter.cs \
	./TargetExtensions/TargetExtensionEventArgs.cs \
	./TargetExtensions/TargetExtensionParameterEventArgs.cs \
	./TargetExtensions/Definitions.cs \
	./TargetExtensions/TargetExtensionParameter.cs \
	./TargetExtensions/BalanceTargetExtension.cs \
	./TargetExtensions/ClassifyTargetExtension.cs \
	./TargetExtensions/ClusterIPTargetExtension.cs \
	./TargetExtensions/DnatTargetExtension.cs \
	./TargetExtensions/DscpTargetExtension.cs \
	./TargetExtensions/EcnTargetExtension.cs \
	./TargetExtensions/IpMarkTargetExtension.cs \
	./TargetExtensions/RejectTargetExtension.cs \
	./TargetExtensions/UlogTargetExtension.cs \
	./FirewallManager.cs \
	./IpTablesCmd.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	-r:System \
	-r:Mono.Posix

DLL_REFERENCES = 

$(ASSEMBLY) $(ASSEMBLY).mdb: $(build_sources) $(build_resources) $(build_datafiles) 
	mkdir -p $(dir $(ASSEMBLY))		

	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$@ -target:$(COMPILE_TARGET) $(build_sources) $(build_resources_embed) $(build_references_ref)

include $(top_srcdir)/Makefile.include
