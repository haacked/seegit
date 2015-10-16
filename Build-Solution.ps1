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
 
& "$(get-content env:programfiles)\MSBuild\14.0\Bin\msbuild.exe" $projFile /t:$build /p:Configuration=$config /verbosity:$MSBuildVerbosity
