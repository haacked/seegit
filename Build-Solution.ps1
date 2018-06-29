param(
    [ValidateSet('FullBuild', 'RunUnitTests', 'Build', 'Clean')]
    [string]
    $build = "FullBuild"
    ,
    [ValidateSet('Debug', 'Release')]
    [string]
    $config = "Debug"
    ,
    [string]
    $MSBuildVerbosity = "normal"
)

$scriptPath = Split-Path $MyInvocation.MyCommand.Path
$projFile = join-path $scriptPath SeeGit.msbuild
$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

& $msbuildExe $projFile /t:$build /p:Configuration=$config /verbosity:$MSBuildVerbosity
