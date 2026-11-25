> **[Versão em Português](./README.md)**

# Piercer COM / OLE Inspector (C#)

**C# (.NET Framework 4.8)** utility that inspects `HKCR\CLSID` in the Windows Registry, lists registered COM/ActiveX objects (`InprocServer32` / `LocalServer32`), detects orphaned/missing DLLs on disk, and exports results to CSV.

## How to Build & Run

1. Run `build.bat` to compile `piercer_com_ole_inspector.exe` via `csc.exe`.
2. Run `piercer_com_ole_inspector.exe [optional_filter] [output.csv]`.

---
Author: **piercer**
