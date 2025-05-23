TOP=../../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk

NAMES+=NoneE NoneF UnknownD SomewhatUnknownD SkipInstallNameTool
NOPREFIX+=NoLibPrefix

NO_INSTALL_NAME_TOOL_SkipInstallNameTool=1

# a few lookup tables, because the data we have is not always in the format we need it

COMMON_ARGS=-g -gdwarf-2 -fms-extensions shared.c -Wall
iphonesimulator_COMPILER_FLAGS=$(COMMON_ARGS) -mios-simulator-version-min=$(DOTNET_MIN_IOS_SDK_VERSION) -isysroot $(iossimulator_SDK)
iphoneos_COMPILER_FLAGS=$(COMMON_ARGS) -miphoneos-version-min=$(DOTNET_MIN_IOS_SDK_VERSION) -isysroot $(ios_SDK)
tvsimulator_COMPILER_FLAGS=$(COMMON_ARGS) -mtvos-simulator-version-min=$(DOTNET_MIN_TVOS_SDK_VERSION) -isysroot $(tvossimulator_SDK)
tvos_COMPILER_FLAGS=$(COMMON_ARGS) -mtvos-version-min=$(DOTNET_MIN_TVOS_SDK_VERSION) -isysroot $(tvos_SDK)
maccatalyst_COMPILER_FLAGS=$(COMMON_ARGS) $(maccatalyst_CFLAGS)
mac_COMPILER_FLAGS=$(COMMON_ARGS) -mmacosx-version-min=$(MIN_OSX_VERSION_FOR_MAC) -isysroot $(XCODE_DEVELOPER_ROOT)/Platforms/MacOSX.platform/Developer/SDKs/MacOSX$(OSX_SDK_VERSION).sdk

maccatalyst-x64_COMPILER_FLAGS=$(maccatalyst-x64_CFLAGS)
maccatalyst-arm64_COMPILER_FLAGS=$(maccatalyst-arm64_CFLAGS)

iossimulator-x64_ARCHITECTURES=x86_64
ios-arm64_ARCHITECTURES=arm64
tvossimulator-x64_ARCHITECTURES=x86_64
tvos-arm64_ARCHITECTURES=arm64
osx-x64_ARCHITECTURES=x86_64
osx-arm64_ARCHITECTURES=arm64
maccatalyst-x64_ARCHITECTURES=x86_64
maccatalyst-arm64_ARCHITECTURES=arm64

define DotNetTemplate

$(1)_$(3)_TARGETS += \
	.libs/$(3)/$(4)$(1).a \
	.libs/$(3)/$(4)$(1).o \
	.libs/$(3)/$(4)$(1).dylib \
	.libs/$(3)/$(4)$(1).so \

$(3)_TARGETS += \
	$$($(1)_$(3)_TARGETS) \

all-local:: $$($(3)_TARGETS)

.libs/$(3)/$(4)$(1).dylib: shared.c | .libs/$(3)
	$$(call Q_2,CC,    [$(3)]) $$(CLANG) -o $$@ $$(foreach arch,$$($(3)_ARCHITECTURES),-arch $$(arch)) $$($(2)_COMPILER_FLAGS) $$($(3)_COMPILER_FLAGS) -DNAME=$(1) -shared -framework Foundation -lz
ifneq ($(NO_INSTALL_NAME_TOOL_$(1)),1)
	$$(Q) $(XCODE_DEVELOPER_ROOT)/Toolchains/XcodeDefault.xctoolchain/usr/bin/install_name_tool -id @rpath/$(4)$(1).dylib $$@
endif

.libs/$(3)/$(4)$(1).o: shared.c | .libs/$(3)
	$$(call Q_2,CC,    [$(3)]) $$(CLANG) -o $$@ $$(foreach arch,$$($(3)_ARCHITECTURES),-arch $$(arch)) $$($(2)_COMPILER_FLAGS) $$($(3)_COMPILER_FLAGS) -DNAME=$(1) -c

.libs/$(3)/$(4)$(1).a: .libs/$(3)/$(4)$(1).o
	$$(call Q_2,AR     [$(1)]) $(XCODE_DEVELOPER_ROOT)/Toolchains/XcodeDefault.xctoolchain/usr/bin/ar cru $$@ $$<

.libs/$(3)/$(4)$(1).so: .libs/$(3)/$(4)$(1).dylib
	$$(Q) $(CP) .libs/$(3)/$(4)$(1).dylib .libs/$(3)/$(4)$(1).so

$$($(3)_$(1)_DIRECTORIES):
	$$(Q) mkdir -p $$@

endef

# 1: name
# 2: sdk platform (iphoneos, iphonesimulator, tvos, tvsimulator, maccatalyst, mac)
# 3: runtime identifier
$(foreach name,$(NAMES),$(foreach platform,$(DOTNET_PLATFORMS),$(foreach rid,$(DOTNET_$(platform)_RUNTIME_IDENTIFIERS),$(eval $(call DotNetTemplate,$(name),$(DOTNET_$(rid)_SDK_PLATFORM),$(rid),lib)))))
$(foreach name,$(NOPREFIX),$(foreach platform,$(DOTNET_PLATFORMS),$(foreach rid,$(DOTNET_$(platform)_RUNTIME_IDENTIFIERS),$(eval $(call DotNetTemplate,$(name),$(DOTNET_$(rid)_SDK_PLATFORM),$(rid))))))
