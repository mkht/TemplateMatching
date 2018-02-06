Param(
    [string] $NuGetApiKey
)

$ErrorActionPreference = 'Stop'

$ModuleRoot = Join-Path $PSScriptRoot '\TemplateMatching\PSModule\TemplateMatching'
$ModuleManifest = Join-Path $ModuleRoot '\TemplateMatching.psd1'

if (-Not (Test-Path $ModuleManifest)) {
    Write-Error "Can not find module manifest file in $ModuleRoot"
    return
}
else {
    Test-ModuleManifest $ModuleManifest >$null
}

if (-Not $NuGetApiKey) {
    $NuGetApiKey = Read-Host 'Enter NuGet Api Key'
}

$Paramter = @{
    Path        = $ModuleRoot
    NuGetApiKey = $NuGetApiKey
}

#Publish
Publish-Module @Paramter -Verbose