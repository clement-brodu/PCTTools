# PCTTools

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

Library to provide additional tools to [PCT](https://github.com/Riverside-Software/pct).

## Features

### AssemblyCatalog

Generate Json File with documentation for all `.dll` used by Openedge session.

The catalog format has some differences with [current AssemblyCatalog Task](https://github.com/Riverside-Software/pct/wiki/AssemblyCatalog).
See the sample `DemoAssemblyCatalog.json` (and `DemoAssemblyCatalog-full.json` with full schema).

Improvements:

- faster (for my situation, 20s instead of 8min)
- better with Generics Types
- provide protected members
- provide fields
- provide enum values as fields
- provide constructors
- provide event handler types and invoke signatures
- allow to scan only one type or one assembly
- catalog file without default values

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

See Openedge sources in Openedge directory.
