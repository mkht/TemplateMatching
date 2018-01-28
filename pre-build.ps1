$targetDir = Join-Path $PSScriptRoot '\TemplateMatching\PSModule\TemplateMatching'

$private:module = [ordered]@{}
$module.Name = 'TemplateMatching'
$module.Path = Join-Path $targetDir ('{0}.psd1' -f $module.Name)
$module.guid = 'd383087a-54f4-417e-a342-ed2fb970b8d2'
$module.description = 'Invoke template matching for PowerShell'
$module.Author = 'mkht'
$module.Copyright = '(c) 2018 mkht. All rights reserved.'
$module.RequiredModules = @()
$module.PowerShellVersion = '5.0'
$module.DotNetFrameworkVersion = '4.5'
$module.RootModule = ('{0}.dll' -f $module.Name)
$module.moduleVersion = '1.0.0'
$module.CmdletToExport = @(
    'Invoke-TemplateMatching'
)
$module.FunctionsToExport = @()
$module.AliasesToExport = @()

$private:moduleManifest = @{
    Path                   = $module.Path
    Guid                   = $module.guid
    Author                 = $module.Author
    Copyright              = $module.Copyright
    ModuleVersion          = $module.moduleVersion
    description            = $module.description
    PowerShellVersion      = $module.PowerShellVersion
    DotNetFrameworkVersion = $module.DotNetFrameworkVersion
    RootModule             = $module.RootModule
    CmdletsToExport        = $module.CmdletToExport
    FunctionsToExport      = $module.FunctionsToExport
    AliasesToExport        = $module.AliasesToExport
    RequiredAssemblies     = $module.RequreAssemblies
}
New-ModuleManifest @moduleManifest -Verbose