#
# Module manifest for module 'Jagabata.psm'
#
# Generated by: teramako
#
# Generated on: 2024/06/15
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'Jagabata.psm.dll'

# Version number of this module.
ModuleVersion = '0.0.1'

# Supported PSEditions
CompatiblePSEditions = @('Core')

# ID used to uniquely identify this module
GUID = 'a60acf94-7e42-4b77-bf97-1cdaf17b822b'

# Author of this module
Author = 'teramako'

# Company or vendor of this module
CompanyName = 'Unknown'

# Copyright statement for this module
Copyright = '(c) teramako. All rights reserved.'

# Description of the functionality provided by this module
Description = 'PowerShell module to operate AWX/AnsibleTower using Rest API.'

# Minimum version of the PowerShell engine required by this module
PowerShellVersion = '7.4'

# Name of the PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# ClrVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
# RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @('types.ps1xml')

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @('formats.ps1xml')

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @()

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = @(
    'Approve-WorkflowApprovalRequest',
    'Deny-WorkflowApprovalRequest',
    'Disable-NotificationTemplate',
    'Enable-NotificationTemplate',
    'Find-AccessList',
    'Find-ActivityStream',
    'Find-AdHocCommandJob',
    'Find-Application',
    'Find-Credential',
    'Find-CredentialInputSource',
    'Find-CredentialType',
    'Find-ExecutionEnvironment',
    'Find-Group',
    'Find-Host',
    'Find-HostMetric',
    'Find-Instance',
    'Find-InstanceGroup',
    'Find-Inventory',
    'Find-InventorySource',
    'Find-InventoryUpdateJob',
    'Find-Job',
    'Find-JobEvent',
    'Find-JobHostSummary',
    'Find-JobTemplate',
    'Find-Label',
    'Find-Notification',
    'Find-NotificationTemplate',
    'Find-NotificationTemplateForApproval',
    'Find-NotificationTemplateForError',
    'Find-NotificationTemplateForStarted',
    'Find-NotificationTemplateForSuccess',
    'Find-ObjectRole',
    'Find-Organization',
    'Find-Project',
    'Find-ProjectUpdateJob',
    'Find-Role',
    'Find-Schedule',
    'Find-SystemJob',
    'Find-SystemJobTemplate',
    'Find-Team',
    'Find-Token',
    'Find-UnifiedJob',
    'Find-UnifiedJobTemplate',
    'Find-User',
    'Find-WorkflowApprovalRequest',
    'Find-WorkflowJob',
    'Find-WorkflowJobNode',
    'Find-WorkflowJobTemplate',
    'Find-WorkflowJobTemplateNode',
    'Get-ActivityStream',
    'Get-AdHocCommandJob',
    'Get-ApiConfig',
    'Get-ApiHelp',
    'Get-Application',
    'Get-Config',
    'Get-Credential',
    'Get-CredentialInputSource',
    'Get-CredentialType',
    'Get-Dashboard',
    'Get-ExecutionEnvironment',
    'Get-Group',
    'Get-Host',
    'Get-HostFactsCache',
    'Get-HostMetric',
    'Get-Instance',
    'Get-InstanceGroup',
    'Get-Inventory',
    'Get-InventoryFile',
    'Get-InventorySource',
    'Get-InventoryUpdateJob',
    'Get-Job',
    'Get-JobHostSummary',
    'Get-JobLog',
    'Get-JobStatistics',
    'Get-JobTemplate',
    'Get-Label',
    'Get-Me',
    'Get-Metric',
    'Get-Notification',
    'Get-NotificationTemplate',
    'Get-Organization',
    'Get-Ping',
    'Get-Playbook',
    'Get-Project',
    'Get-ProjectUpdateJob',
    'Get-Role',
    'Get-Schedule',
    'Get-Setting',
    'Get-SurveySpec',
    'Get-SystemJob',
    'Get-SystemJobTemplate',
    'Get-Team',
    'Get-Token',
    'Get-User',
    'Get-VariableData',
    'Get-WorkflowApprovalRequest',
    'Get-WorkflowApprovalTemplate',
    'Get-WorkflowJob',
    'Get-WorkflowJobNode',
    'Get-WorkflowJobTemplate',
    'Get-WorkflowJobTemplateNode',
    'Grant-Role',
    'Invoke-AdHocCommand',
    'Invoke-API',
    'Invoke-InventoryUpdate',
    'Invoke-JobTemplate',
    'Invoke-ProjectUpdate',
    'Invoke-SystemJobTemplate',
    'Invoke-WorkflowJobTemplate',
    'New-ApiConfig',
    'New-Application',
    'New-Credential',
    'New-CredentialType',
    'New-ExecutionEnvironment',
    'New-Group',
    'New-Host',
    'New-Inventory',
    'New-InventorySource',
    'New-JobTemplate',
    'New-Label',
    'New-NotificationTemplate',
    'New-Organization',
    'New-Project',
    'New-Schedule',
    'New-Team',
    'New-Token',
    'New-User',
    'New-WorkflowJobTemplate',
    'New-WorkflowJobTemplateNode',
    'Register-Credential',
    'Register-Group',
    'Register-Host',
    'Register-Label',
    'Register-SurveySpec',
    'Register-User',
    'Register-WorkflowJobTemplateNode',
    'Remove-AdHocCommandJob',
    'Remove-Application',
    'Remove-Credential',
    'Remove-CredentialType',
    'Remove-ExecutionEnvironment',
    'Remove-Group',
    'Remove-Host',
    'Remove-Inventory',
    'Remove-InventorySource',
    'Remove-InventoryUpdateJob',
    'Remove-Job',
    'Remove-JobTemplate',
    'Remove-NotificationTemplate',
    'Remove-Organization',
    'Remove-Project',
    'Remove-ProjectUpdateJob',
    'Remove-Schedule',
    'Remove-SurveySpec',
    'Remove-SystemJob',
    'Remove-Team',
    'Remove-Token',
    'Remove-User',
    'Remove-WorkflowApprovalRequest',
    'Remove-WorkflowJob',
    'Remove-WorkflowJobTemplate',
    'Remove-WorkflowJobTemplateNode',
    'Revoke-Role',
    'Start-AdHocCommand',
    'Start-InventoryUpdate',
    'Start-JobTemplate',
    'Start-ProjectUpdate',
    'Start-SystemJobTemplate',
    'Start-WorkflowJobTemplate',
    'Stop-UnifiedJob',
    'Switch-ApiConfig',
    'Unregister-Credential',
    'Unregister-Group',
    'Unregister-Host',
    'Unregister-Label',
    'Unregister-User',
    'Unregister-WorkflowJobTemplateNode',
    'Update-Application',
    'Update-Credential',
    'Update-CredentialType',
    'Update-ExecutionEnvironment',
    'Update-Group',
    'Update-Host',
    'Update-Inventory',
    'Update-InventorySource',
    'Update-JobTemplate',
    'Update-Label',
    'Update-NotificationTemplate',
    'Update-Organization',
    'Update-Project',
    'Update-Schedule',
    'Update-Team',
    'Update-Token',
    'Update-User',
    'Update-WorkflowJobTemplate',
    'Update-WorkflowJobTemplateNode',
    'Wait-UnifiedJob'
)

# Variables to export from this module
# VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        Tags = @('PSEdition_Core', 'Ansible', 'AWX', 'AnsibleTower')

        # A URL to the license for this module.
        LicenseUri = 'https://github.com/teramako/Jagabata.psm/blob/main/LICENSE'

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/teramako/Jagabata.psm'

        # A URL to an icon representing this module.
        IconUri = 'https://raw.githubusercontent.com/teramako/Jagabata.psm/refs/heads/develop/docs/img/Jagabata_x85.png'

        # ReleaseNotes of this module
        ReleaseNotes = 'https://github.com/teramako/Jagabata.psm/releases'

        # Prerelease string of this module
        # Prerelease = ''

        # Flag to indicate whether the module requires explicit user acceptance for install/update/save
        # RequireLicenseAcceptance = $false

        # External dependent modules of this module
        # ExternalModuleDependencies = @()

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

