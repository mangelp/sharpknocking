dnl Warning: This is an automatically generated file, do not edit!
dnl Process this file with autoconf to produce a configure script.
AC_PREREQ([2.54])
AC_INIT([ManagedSource], [0.1])
AM_INIT_AUTOMAKE([foreign])

dnl pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
        AC_MSG_ERROR([You need to install pkg-config])
fi

AC_PROG_INSTALL

AC_PATH_PROG(MCS, mcs, no)
if test "x$MCS" = "xno"; then
        AC_MSG_ERROR([mcs Not found])
fi


PKG_CHECK_MODULES([GTK_SHARP_20], [gtk-sharp-2.0])
PKG_CHECK_MODULES([GLIB_SHARP_20], [glib-sharp-2.0])
PKG_CHECK_MODULES([GLADE_SHARP_20], [glade-sharp-2.0])


AC_ARG_ENABLE(debug,
	AC_HELP_STRING([--enable-debug],
		[Use 'Debug' Configuration [default=YES]]),
		enable_debug=yes, enable_debug=no)
AM_CONDITIONAL(ENABLE_DEBUG, test x$enable_debug = xyes)
if test "x$enable_debug" = "xyes" ; then
	COMMON_DEBUG_LIB='SharpKnocking.Common.dll'
	AC_SUBST(COMMON_DEBUG_LIB)
	COMMONWIDGETS_DEBUG_LIB='CommonWidgets.dll'
	AC_SUBST(COMMONWIDGETS_DEBUG_LIB)
	NETFILTERFIREWALL_DEBUG_LIB='NetfilterFirewall.dll'
	AC_SUBST(NETFILTERFIREWALL_DEBUG_LIB)
	DEBUG_CONFIG_LIBRARIES=' ${pkglibdir}/SharpKnocking.Common.dll ${pkglibdir}/CommonWidgets.dll ${pkglibdir}/NetfilterFirewall.dll'
	DEBUG_CONFIG_LIBS=' -r:${pkglibdir}/SharpKnocking.Common.dll -r:${pkglibdir}/CommonWidgets.dll -r:${pkglibdir}/NetfilterFirewall.dll'
	AC_SUBST(DEBUG_CONFIG_LIBRARIES)
	AC_SUBST(DEBUG_CONFIG_LIBS)
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release,
	AC_HELP_STRING([--enable-release],
		[Use 'Release' Configuration [default=NO]]),
		enable_release=yes, enable_release=no)
AM_CONDITIONAL(ENABLE_RELEASE, test x$enable_release = xyes)
if test "x$enable_release" = "xyes" ; then
	COMMON_RELEASE_LIB='SharpKnocking.Common.dll'
	AC_SUBST(COMMON_RELEASE_LIB)
	COMMONWIDGETS_RELEASE_LIB='CommonWidgets.dll'
	AC_SUBST(COMMONWIDGETS_RELEASE_LIB)
	NETFILTERFIREWALL_RELEASE_LIB='NetfilterFirewall.dll'
	AC_SUBST(NETFILTERFIREWALL_RELEASE_LIB)
	RELEASE_CONFIG_LIBRARIES=' ${pkglibdir}/SharpKnocking.Common.dll ${pkglibdir}/CommonWidgets.dll ${pkglibdir}/NetfilterFirewall.dll'
	RELEASE_CONFIG_LIBS=' -r:${pkglibdir}/SharpKnocking.Common.dll -r:${pkglibdir}/CommonWidgets.dll -r:${pkglibdir}/NetfilterFirewall.dll'
	AC_SUBST(RELEASE_CONFIG_LIBRARIES)
	AC_SUBST(RELEASE_CONFIG_LIBS)
	CONFIG_REQUESTED="yes"
fi
if test -z "$CONFIG_REQUESTED" ; then
	COMMON_DEBUG_LIB='SharpKnocking.Common.dll'
	AC_SUBST(COMMON_DEBUG_LIB)
	COMMONWIDGETS_DEBUG_LIB='CommonWidgets.dll'
	AC_SUBST(COMMONWIDGETS_DEBUG_LIB)
	NETFILTERFIREWALL_DEBUG_LIB='NetfilterFirewall.dll'
	AC_SUBST(NETFILTERFIREWALL_DEBUG_LIB)
	DEBUG_CONFIG_LIBRARIES=' ${pkglibdir}/SharpKnocking.Common.dll ${pkglibdir}/CommonWidgets.dll ${pkglibdir}/NetfilterFirewall.dll'
	DEBUG_CONFIG_LIBS=' -r:${pkglibdir}/SharpKnocking.Common.dll -r:${pkglibdir}/CommonWidgets.dll -r:${pkglibdir}/NetfilterFirewall.dll'
	AC_SUBST(DEBUG_CONFIG_LIBRARIES)
	AC_SUBST(DEBUG_CONFIG_LIBS)
	AM_CONDITIONAL(ENABLE_DEBUG, true)
fi


AC_CONFIG_FILES([
./PortKnocker/ManagedSource.PortKnocker.pc
./NetfilterFirewall/ManagedSource.NetfilterFirewall.pc
./PortKnocker/portknocker
./Common/ManagedSource.Common.pc
./KnockingDaemonControl/ManagedSource.KnockingDaemonControl.pc
managedsource.pc
./NetfilterFirewall/Makefile
./KnockingDaemonControl/knockingdaemoncontrol
./PortKnocker/Makefile
./KnockingDaemonControl/Makefile
./KnockingDaemon/ManagedSource.KnockingDaemon.pc
./Makefile
./KnockingDaemon/Makefile
./CommonWidgets/Makefile
./KnockingDaemon/knockingdaemon
./CommonWidgets/ManagedSource.CommonWidgets.pc
./Doorman/Makefile
./Doorman/doorman
./Doorman/ManagedSource.Doorman.pc
./Common/Makefile

])

AC_OUTPUT
