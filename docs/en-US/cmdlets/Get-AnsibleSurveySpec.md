---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleSurveySpec

## SYNOPSIS
Retrieve Survey specifications.

## SYNTAX

```
Get-AnsibleSurveySpec [-Template] <IResource> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Survey specifications for JobTemplate or WorkflowJobTemplate.

Implements following Rest API:  
- `/api/v2/job_templates/{id}/survey_spec/`  
- `/api/v2/workflow_job_templates/{id}/survey_spec/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleSurveySpec -Template JobTemplate:10

Name Description Spec
---- ----------- ----
                 {{ Name = User, Type = Text, Variable = user_name, Default = }, …}
```

## PARAMETERS

### -Template
JobTemplate or WorkflowJobTemplate resource object which has SurveySpecs.

> [!TIP]  
> Can specify the resource as string like `JobTemplate:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Template (Get-AnsibleJobTemplate -Id 1)`  
>  - `-Template @{ type = "jobTemplate"; id = 1 }`  
>  - `-Template jobTemplate:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.IResource
JobTemplate or WorkflowJobTemplate resource object.
See `-Template` parameter.

## OUTPUTS

### Jagabata.Resources.Survey
## NOTES

## RELATED LINKS

[Register-AnsibleSurveySpec](Register-AnsibleSurveySpec.md)

[Remove-AnsibleSurveySpec](Remove-AnsibleSurveySpec.md)

[Get-AnsibleJobTemplate](Get-AnsibleJobTemplate.md)

[Find-AnsibleJobTemplate](Find-AnsibleJobTemplate.md)

[Get-AnsibleWorkflowJobTemplate](Get-AnsibleWorkflowJobTemplate.md)

[Find-AnsibleWorkflowJobTemplate](Find-AnsibleWorkflowJobTemplate.md)
