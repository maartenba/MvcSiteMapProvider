@echo off
GOTO endcommentblock
:: -----------------------------------------------------------------------------------
:: This file will build the MvcSiteMapProvider and package the Nuget builds.
::
:: Syntax:
::   build[.bat] [<options>]
::
:: Available Options:
::
::   -Version:<Version>
::   -v:<Version> - Assembly version number. Default is empty string.
::
::   -PackageVersion:<PackageVersion>
::   -pv:<PackageVersion> - Nuget package version. Default is 4.0.0.
::
::   -Configuration:<Configuration>
::   -config:<Configuration> - MSBuild configuration for the build.
::
::   All options are case insensitive.
::
::   To escape any of the options, put double quotes around the entire value, like this:
::   "-config:Release"
::
:: -----------------------------------------------------------------------------------
:endcommentblock
setlocal enabledelayedexpansion enableextensions

REM Default values
IF "%version%" == "" (
	set version=
)
IF "%PackageVersion%" == "" (
	IF "%version%" == "" (
		set PackageVersion=4.0.0
	) ELSE (
		set PackageVersion=%version%
	)
)
set configuration=Release
IF NOT "%config%" == "" (
	set configuration=%config%
)

FOR %%a IN (%*) DO (
	FOR /f "useback tokens=*" %%a in ('%%a') do (
		set value=%%~a

		set test=!value:~0,3!
		IF /I !test! EQU -v: (
			set version=!value:~3!
		)

		set test=!value:~0,9!
		IF /I !test! EQU -version: (
			set version=!value:~9!
		)
		
		set test=!value:~0,4!
		IF /I !test!==-pv: (
			set packageversion=!value:~4!
		)

		set test=!value:~0,16!
		IF /I !test!==-packageversion: (
			set packageversion=!value:~16!
		)

		set test=!value:~0,8!
		IF /I !test!==-config: (
			set configuration=!value:~8!
		)

		set test=!value:~0,15!
		IF /I !test!==-configuration: (
			set configuration=!value:~15!
		)
	)
)

powershell -Command "& { Import-Module .\build\psake.psm1; $psake.use_exit_on_error = $true; Invoke-psake .\build\runbuild.ps1 -framework 4.0x64 -properties @{\"version\"=\"%version%\";\"configuration\"=\"%configuration%"\";\"packageVersion\"=\"%PackageVersion%"\"} }"
