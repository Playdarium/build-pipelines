# Get started

---

All resources for build must be located in the Editor resources folder.

1. Create from `Create -> BuildPipeline -> BuildProjectPipeline`.
2. Setup pipeline name.
3. Setup build target.
4. Configure your own pipeline steps.
5. Launch build you can from menu `Tools -> Build Pipelines`

## BuildProjectPipeline

| Field name    | Description                                                  |
|---------------|--------------------------------------------------------------|
| Pipeline Name | This name should be unique and using for invoke remote build |
| Build Target  | Using for build grouping in Build Pipelines window           |
| Steps         | Pipeline steps for configure and build project               |

## Predefined steps

| Step name                     | Description                                            |
|-------------------------------|--------------------------------------------------------|
| ProductName                   | Set product name                                       |
| PackageName                   | Set package name for selected platform                 |
| KeystoreAndAlias              | Set keystore and alias                                 |
| Define                        | Set defines for selected platform                      |
| LogLevel                      | Set log level                                          |
| BuildAppBundle                | Build apk or aab (Android)                             |
| ScriptingBackendAndroidMono   | Set scripting backend android to mono                  |
| ScriptingBackendAndroidIL2CPP | Set scripting backend android to IL2CPP                |
| BuildAndStore                 | Setup configuration in scriptable object BuildSettings |
| AddBuildCountToVersion        | Used in Android for build Debug version                |
| SetIOSBuildNumber             | Used for change build number on build agent            |
| UnityLogo                     | Enable or disable unity logo in build                  |
| SceneToBuild                  | Provide scene path to build                            |
| AndroidBuild                  | Execute build Android project                          | 
| iOSBuild                      | Export iOS project for XCode                           |

# Configure default pipelines

---

Create directories:

- `BuildPipleines -> Editor -> Resporces -> Pipelines`
- `BuildPipleines -> Editor -> Resporces -> Steps`

In `Resources` create scriptable object `Create -> Settings -> BuildSettings`

## Android pipelines

---

Create 3 different pipelines `Create -> BuildPipeline -> BuildProjectPipeline`

- AndroidDebug
- AndroidReleaseAPK
- AndroidReleaseAAB

Add to this pipelines steps `Create -> BuildPipeline -> Steps`

### AndroidDebug

| Step Name                     | Value                                                        |
|-------------------------------|--------------------------------------------------------------|
| BuildAndStore                 | BuildType: Debug <br/> StoreType: GooglePlay                 |
| KeystoreAndAlias              | Empty                                                        |
| BuildAppBundle                | False                                                        |
| ScriptingBackendAndroidMono   | -                                                            |
| AddBuildCountToVersion        | -                                                            |
| UnityLogo                     | False                                                        |
| SceneToBuild                  | Scenes in project                                            |
| AndroidBuild                  | ProjectPrefix: [custom_value]<br/> BuildOptions: Development |

 ---

### AndroidReleaseAPK

| Step Name                     | Value                                                 |
|-------------------------------|-------------------------------------------------------|
| BuildAndStore                 | BuildType: Release <br/> StoreType: GooglePlay        |
| KeystoreAndAlias              | Set your values                                       |
| BuildAppBundle                | False                                                 |
| ScriptingBackendAndroidIL2CPP | -                                                     |
| UnityLogo                     | False                                                 |
| SceneToBuild                  | Scenes in project                                     |
| AndroidBuild                  | ProjectPrefix: [custom_value]<br/> BuildOptions: None |

---

### AndroidReleaseAAB

| Step Name                     | Value                                                 |
|-------------------------------|-------------------------------------------------------|
| BuildAndStore                 | BuildType: Release <br/> StoreType: GooglePlay        |
| KeystoreAndAlias              | Set your values                                       |
| BuildAppBundle                | True                                                  |
| ScriptingBackendAndroidIL2CPP | -                                                     |
| UnityLogo                     | False                                                 |
| SceneToBuild                  | Scenes in project                                     |
| AndroidBuild                  | ProjectPrefix: [custom_value]<br/> BuildOptions: None |

---

## iOS pipelines

---

### Prepare Unity project

---

#### Disable Unity Cloud Diagnostics

For avoid exception when XCode build project:

```text
Please provide an auth token with USYM_UPLOAD_AUTH_TOKEN environment variable
```

1. On the menu bar, select **Window > General**, then select **Services**.
2. In the **Services** window, select **Cloud Diagnostics**.
3. Disable **Cloud Diagnostics**.

### Create pipelines

Create 3 different pipelines `Create -> BuildPipeline -> BuildProjectPipeline`

- iOSDebugBuild
- iOSReleaseBuild

Add to this pipelines steps `Create -> BuildPipeline -> Steps`

### iOSDebugBuild

| Step Name         | Value                                                                                                                                                                                                                                                           |
|-------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| BuildAndStore     | BuildType: Debug <br/> StoreType: iOS                                                                                                                                                                                                                           |
| SetIOSBuildNumber | -                                                                                                                                                                                                                                                               |
| UnityLogo         | False                                                                                                                                                                                                                                                           |
| SceneToBuild      | Scenes in project                                                                                                                                                                                                                                               |
| iOSBuild          | ProjectPrefix: [custom_value]<br/> BuildOptions: Development<br/> BuildType: Debug<br/> ExportOptionsEnabled: True<br/> CompileBitcode: False<br/> StripSwiftSymbols: True<br/> DevelopmentProvision: [path_to_file]<br/> DistributionProvision: [path_to_file] |

---

### iOSReleaseBuild

| Step Name         | Value                                                                                                                                                                                                                                                     |
|-------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| BuildAndStore     | BuildType: Release <br/> StoreType: iOS                                                                                                                                                                                                                   |
| SetIOSBuildNumber | -                                                                                                                                                                                                                                                         |
| UnityLogo         | False                                                                                                                                                                                                                                                     |
| SceneToBuild      | Scenes in project                                                                                                                                                                                                                                         |
| iOSBuild          | ProjectPrefix: [custom_value]<br/> BuildOptions: None<br/> BuildType: Release<br/> ExportOptionsEnabled: True<br/> CompileBitcode: True<br/> StripSwiftSymbols: True<br/> DevelopmentProvision: [path_to_file]<br/> DistributionProvision: [path_to_file] |

---

# Customize pipeline steps

For create you own build pipeline step you must implement `APipelineStep` and
override method:  
`public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)`

Your pipeline step can execute a recompile project and `BuildPipelineExecutor` restore
a building process after assembly reload.

If you need provide some variables between steps use `BuildParameterHolder`.  
For serialization `BuildParameterHolder`
use [BinaryFormatter](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.formatters.binary.binaryformatter?view=net-5.0)
.

# Execute build pipeline from builder

Launch unity with arguments:

```
-executeMethod Playdarium.BuildPipelines.BuildPipelineExecutor.Build -pipelineName [PipelineName] -buildPath [BuildPath]
```
