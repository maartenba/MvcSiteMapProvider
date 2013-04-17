@echo off

set config=%1
if "%config%" == "" (
   set config=Release
)

REM set version=
REM if not "%PackageVersion%" == "" (
REM    set version=%PackageVersion%
REM )
set version=4.0.0

powershell -Command "& { Import-Module .\build\psake.psm1; $psake.use_exit_on_error = $true; Invoke-psake .\build\build.ps1 -framework 4.0x64 -properties @{\"version\"=\"%version%\";\"configuration\"=\"%config%"\";\"packageVersion\"=\"%PackageVersion%\"} }"
