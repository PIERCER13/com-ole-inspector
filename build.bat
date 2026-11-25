@echo off
cd /d "%~dp0"
set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if exist "%CSC%" (
  "%CSC%" /nologo /optimize+ /out:piercer_com_ole_inspector.exe Program.cs
  echo Compilado: piercer_com_ole_inspector.exe
) else (
  echo csc.exe nao encontrado.
)
