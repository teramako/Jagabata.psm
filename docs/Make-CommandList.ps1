<#
.SYNOPSIS
Create Command List files
#>
using namespace System.Management.Automation
using namespace System.Collections.Generic

[CmdletBinding()]
param(
    [Parameter()]
    [string] $Locale = "en-US"
)

$ErrorActionPreference = 'Stop'

$moduleName = "Jagabata.psm"
$modulePath = Join-Path -Path $PSScriptRoot -ChildPath .. -AdditionalChildPath out, $moduleName
$module = Import-Module $modulePath -PassThru -Verbose:$false
$cmds = Get-Command -Module $moduleName

function Out-CommandList 
{
    [CmdletBinding()]
    [OutputType([System.IO.FileInfo])]
    param(
        [Parameter(Mandatory)]
        [string] $GroupName
        ,
        [Parameter(Mandatory, ValueFromPipeline)]
        [CmdletInfo[]] $Command
    )
    begin
    {
        $path = Join-Path -Path $PSScriptRoot -ChildPath $Locale -AdditionalChildPath ("CommandListBy{0}.md" -f $GroupName)
        $Commands = [List[CmdletInfo]]::new()
    }
    process
    {
        $Commands.AddRange($Command)
    }
    end
    {
        $Commands |
            Group-Object -Property $GroupName |
            ForEach-Object -Begin {
                "# Command List By $GroupName", "" | Write-Output
            } -Process {
                ("## {0}" -f ($_.Name -replace ("^" + $module.Prefix), "")),
                "",
                (
                    $_.Group | ForEach-Object { "- [{0}](./cmdlets/{0}.md)" -f $_.Name }
                ),
                "" | Write-Output
            }  |
            Out-File -FilePath $path -Encoding utf8NoBOM

        Write-Verbose ("Created command list: GroupBy = {0} to '{1}'" -f $GroupName, $path)
        return Get-Item $path
    }
}

$cmds | Out-CommandList -GroupName Noun
$cmds | Out-CommandList -GroupName Verb

