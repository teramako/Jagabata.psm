---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-SurveySpec

## SYNOPSIS
Retrieve Survey specifications.

## SYNTAX

```
Get-SurveySpec [-Type] <ResourceType> [-Id] <UInt64> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Survey specifications for JobTemplate or WorkflowJobTemplate.

Implements following Rest API:  
- `/api/v2/job_templates/{id}/survey_spec/`  
- `/api/v2/workflow_job_templates/{id}/survey_spec/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-SurveySpec -Type JobTemplate -Id 10

Name Description Spec
---- ----------- ----
                 {{ Name = User, Type = Text, Variable = user_name, Default = }, …}
```

## PARAMETERS

### -Id
Datebase ID of the target resource.
Use in conjection with the `-Type` parameter.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: (All)
Aliases:
Accepted values: JobTemplate, WorkflowJobTemplate

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.ResourceType
Input by `Type` property in the pipeline object.

Acceptable values:  
- `JobTemplate`  
- `WorkflowJobTemplate`  

### System.UInt64
Input by `Id` property in the pipeline object.

Database ID for the ResourceType

## OUTPUTS

### Jagabata.Resources.Survey
## NOTES

## RELATED LINKS

[Register-SurveySpec](Register-SurveySpec.md)

[Remove-SurveySpec](Remove-SurveySpec.md)

[Get-JobTemplate](Get-JobTemplate.md)

[Find-JobTemplate](Find-JobTemplate.md)

[Get-WorkflowJobTemplate](Get-WorkflowJobTemplate.md)

[Find-WorkflowJobTemplate](Find-WorkflowJobTemplate.md)
