# PCTTools

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

Library to provide additional tools for [PCT](https://github.com/Riverside-Software/pct).

## Installation

- Option 1 (recommended): Download the latest `PCTTools-{version}.zip` from [GitHub Releases](https://github.com/clement-brodu/PCTTools/releases)
- Option 2: Download `PCTTools.dll` from [GitHub Releases](https://github.com/clement-brodu/PCTTools/releases)
  - **Important**: Unlock the file (see Troubleshooting section below)

## Features

### AssemblyCatalog

Generate a JSON file with documentation for all `.dll` files used by OpenEdge session.

The catalog format has some differences with [current AssemblyCatalog Task](https://github.com/Riverside-Software/pct/wiki/AssemblyCatalog).
See the sample `DemoAssemblyCatalog.json` (and `DemoAssemblyCatalog-full.json` with full schema).

Improvements:

- Faster (for my situation, 20s instead of 8min)
- Better handling of Generic Types
- Provides protected members
- Provides fields
- Provides enum values as fields
- Provides constructors
- Provides event handler types and invoke signatures
- Allows scanning only one type or one assembly
- Catalog file without default values

#### Usage with ANT

```xml
<PCTRUn 
    dlcHome="${DLC}" 
    assemblies="path/to/dir/with/assemblies.xml"
    procedure="Openedge/NetAssemblyCatalog.p" >
  <propath>
    <!-- Add Propath for the PCTTextWriter-->
    <pathelement path="Openedge" />
  </propath>
  <Parameter name="destFile" value="catalog.json" />
  <Parameter name="pctTools" value="path/to/PCTTools.dll" />
</PCTRUn>
```

See OpenEdge sources in the Openedge directory.

## Troubleshooting

### Error: "Could not load file or assembly" or Security Issues

When downloading `PCTTools.dll` directly from GitHub (without using the ZIP package), Windows may block the file for security reasons. You'll need to unlock it:

#### Method 1: Using File Properties (GUI)

1. Right-click on `PCTTools.dll`
2. Select "Properties"
3. In the "General" tab, check if there's a "Security" section at the bottom
4. If you see "This file came from another computer and might be blocked to help protect this computer"
5. Check the "Unblock" checkbox
6. Click "OK"

#### Method 2: Using PowerShell

```powershell
Unblock-File -Path "path\to\PCTTools.dll"
```

## License

[MIT](https://choosealicense.com/licenses/mit/)
