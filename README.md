> **[English Version](./README.en.md)**

# Piercer COM / OLE Inspector (C#)

Utilitário em **C# (.NET Framework 4.8)** que lê `HKCR\CLSID` no Registro do Windows, lista objetos COM/ActiveX (`InprocServer32` / `LocalServer32`), detecta entradas órfãs (DLL ausente no disco) e exporta tudo para CSV.

## Como Compilar e Executar

1. Execute `build.bat` para gerar `piercer_com_ole_inspector.exe` via `csc.exe`.
2. Rode `piercer_com_ole_inspector.exe [filtro_opcional] [saida.csv]`.

---
Autor: **piercer**
