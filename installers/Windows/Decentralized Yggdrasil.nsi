!include "MUI2.nsh"
!include "x64.nsh"
Name "Decentralized Yggdrasil"
OutFile "Decentralized Yggdrasil Setup.exe"
Unicode true

InstallDir "$PROGRAMFILES\ZachDeibert\DecentralizedYggdrasil"
InstallDirRegKey HKLM "Software\ZachDeibert\DecentralizedYggdrasil" ""

!define MUI_ABORTWARNING

!insertmacro MUI_PAGE_LICENSE "License.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!include "English.nsh"

Section $(Sec_Core_Title) Sec_Core
	SetOutPath "$INSTDIR"
	WriteRegStr HKLM "Software\ZachDeibert\DecentralizedYggdrasil" "" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DecentralizedYggdrasil" "DisplayName" "Decentralized Yggdrasil"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DecentralizedYggdrasil" "UninstallString" '"$INSTDIR\Uninstall.exe"'
	WriteUninstaller $INSTDIR\Uninstall.exe
	File ..\..\bin\Release\batch-launcher.exe
	File ..\..\bin\Release\BouncyCastle.Crypto.dll
	File ..\..\bin\Release\decentralized-yggdrasil.exe
	File ..\..\bin\Release\decentralized-yggdrasil.jar
	File ..\..\bin\Release\Newtonsoft.Json.dll
	File ..\..\bin\Release\ssl-endpoint.exe
SectionEnd

Section $(Sec_Shortcuts_Title) Sec_Shortcuts
	CreateDirectory "$SMPROGRAMS\ZachDeibert"
	CreateShortcut "$SMPROGRAMS\ZachDeibert\DecentralizedYggdrasil.lnk" "$INSTDIR\decentralized-yggdrasil.exe"
SectionEnd

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${Sec_Core} $(Sec_Core_Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${Sec_Shortcuts} $(Sec_Shortcuts_Desc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Section "Uninstall"
	RMDir /r "$INSTDIR"
	RMDir /r "$SMPROGRAMS\ZachDeibert\DecentralizedYggdrasil.lnk"
	DeleteRegKey /ifempty HKLM "Software\ZachDeibert\DecentralizedYggdrasil"
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DecentralizedYggdrasil"
SectionEnd
