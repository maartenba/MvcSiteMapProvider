@echo off

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

powershell -Command "& { Import-Module .\build\psake.psm1; $psake.use_exit_on_error = $true; Invoke-psake .\build\build.ps1 -framework 4.0x64 -properties @{\"version\"=\"%version%\";\"configuration\"=\"%config%"\"} }"