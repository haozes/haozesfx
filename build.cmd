@echo off

rd /S /Q %~dp0\release\ 
set builderPath="C:\Windows\Microsoft.NET\Framework\v4.0.30319"

%builderPath%\MSBuild.exe HaozesFx.sln /t:Rebuild /p:OutDir=%~dp0/release\  /p:Configuration=Release

%builderPath%\MSBuild.exe FxPlugin/HaozesFxPlugin.sln /t:Rebuild /p:OutDir=%~dp0/release\plugin\  /p:Configuration=Release

Copy %~dp0\reference\SQLite.Interop.DLL  %~dp0\release\SQLite.Interop.DLL  /Y

@pause