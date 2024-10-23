---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsiblePing

## SYNOPSIS
Retrieve some basic information about the instance.

## SYNTAX

```
Get-AnsiblePing [<CommonParameters>]
```

## DESCRIPTION
A simple view that reports very basic information about this instance, which is acceptable to be public information.

Implements following Rest API:  
- `/api/v2/ping/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsiblePing

HA             : False
Version        : 23.3.1
ActiveNode     : awx_1
InstallUuid    : 0aa779f2-fe62-4252-a01e-e163b75e12ec
Instances      : {{ Node = awx_1, NodeType = hybrid, Uuid = c327e3bc-807b-4670-acc2-925f6913c421, Heartbeat = 2024-08-05T08:17:20.194459Z, Capacity = 138, Version = 23.3.1 }}
InstanceGroups : {{ Name = controlplane, Capacity = 138, Instances: [awx_1] }, { Name = default, Capacity = 138, Instances: [awx_1] }}
```

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.Ping
## NOTES

## RELATED LINKS
