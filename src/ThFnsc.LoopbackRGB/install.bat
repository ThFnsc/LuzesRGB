@echo off
cd "%~dp0%"
set exename=ThFnsc.LoopbackRGB
sc.exe stop    %exename%
sc.exe delete  %exename%
dotnet publish . --output .\bin\publish --configuration Release || goto :error
sc.exe create  %exename% "binPath=%~dp0bin\publish\%exename%.exe" start=auto
sc.exe failure %exename% reset= 86400 actions= restart/60000/restart/60000//1000
sc.exe start   %exename%
pause
goto :EOF

:error
echo ERROR #%errorlevel%.
pause
exit /b %errorlevel%