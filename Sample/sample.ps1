$Module = Join-Path (Split-Path $PSScriptRoot -Parent) '\TemplateMatching\PSModule\TemplateMatching\TemplateMatching.psd1'
$TargetImage = Join-Path $PSScriptRoot '\target.png'
$TemplateImage = Join-Path $PSScriptRoot '\template.png'

if(-Not (Test-Path $Module)){
    Write-Warning 'Module not found.'
    return
}

Import-Module $Module -Force -Verbose

Invoke-TemplateMatching -Target $TargetImage -Template $TemplateImage