# Mgcb-Importer

Tool packaging functionality from the Monogame Content Editor to allow importing legacy XNA project files from the command line.

## Adding to a project

To install as a [local tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools#install-a-local-tool), run:

  ```cmd
  dotnet tool install mgcb-importer --create-manifest-if-needed
  ```

Run as a pre-build step from a .csproj file as follows:

  ```xml
  <Target Name="ImportContent" BeforeTargets="CollectContentReferences">
    <Exec Command="&quot;$(DotnetCommand)&quot; tool restore"/>
    <Exec Command="&quot;$(DotnetCommand)&quot; mgcb-importer &quot;..\MyContentProject\MyContent.contentproj&quot; &quot;..\MyContentProject\MyContent.mgcb&quot;" />
  </Target>
  ```

## Version History

### 3.8.3.5

* Fix imported item link paths not being escaped correctly.

### 3.8.3.4

* Include tool timestamp in out-of-date check

### 3.8.3.3

* Restore original command line format (`mgcb-importer <input-file> <output-file>`)

### 3.8.3.2

* Output file is no longer updated unless input file has a newer timestamp
* Compiled for release

### 3.8.3.1

* Initial version
