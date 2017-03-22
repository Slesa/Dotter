@echo off

SETLOCAL
SET FAKE_PATH=packages\FAKE\tools\Fake.exe

cls

.paket\paket.bootstrapper.exe
if errorlevel 1 (
  exit /b %errorlevel%
)

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

IF NOT EXIST build.fsx (
  .paket\paket.exe update
  packages\FAKE\tools\FAKE.exe init.fsx
)

:Build

IF [%1]==[] (
    %FAKE_PATH% "build.fsx" "Default"
) ELSE (
    %FAKE_PATH% "build.fsx" %*
)

rem Bail if we're running a TeamCity build.
if defined TEAMCITY_PROJECT_NAME goto Quit
if defined APPVEYOR goto Quit

rem Loop the build script.
set CHOICE=nothing
echo (Q)uit, (Enter) runs the build again
set /P CHOICE= 
if /i "%CHOICE%"=="Q" goto :Quit

GOTO Build

:Quit
exit /b %errorlevel%
