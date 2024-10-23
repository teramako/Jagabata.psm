---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Register-AnsibleSurveySpec

## SYNOPSIS
Register SurveySpecs.

## SYNTAX

```
Register-AnsibleSurveySpec [-Template] <IResource> [-Name <String>] [-Description <String>]
 -Spec <SurveySpec[]> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Register SurveySpecs to the target Template (JobTemplate or WorkflowJobTemplate).

Implements following Rest API:  
- `/api/v2/job_templates/{id}/survey_spec/` (POST)  
- `/api/v2/workflow_job_templates/{id}/survey_spec/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $jt = Get-AnsibleJobTemplate -Id 10
PS C:\> Register-AnsibleSurveySpec $jt -Spec @{QuestionName="User name";Variable="user";Required=$true},@{QuestionName="Password";Variable="pass";Type="password";Required=$true}
```

### Example 2
```powershell
PS C:\> $jt = Get-AnsibleJobTemplate -Id 10
PS C:\> $survey = $jt | Get-AnsibleSurveySpec
PS C:\> $survey.Spec += @{ QuestionName="Select an animal"; Variable="animal"; Choices=@("cat","doc"); Type="multiplechoice" }
PS C:\> $jt | Register-AnsibleSurveySpec -Spec $survey.Spec
```

Add multiple choice survey and re-register.

## PARAMETERS

### -Description
Optional description of the Survey.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Optional name of the Survey.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Spec
Array of `SurveySpec` objects.

`SurveySpec` properties:  
- `Type`:  
  - `Text`: For survey questions expecting a textual answer (Default).  
  - `Textarea`: For survey questions expecting a multiline textual answer.  
  - `Password`: For survey questions expecting a password or other sensitive information.  
  - `Integer`: For survey questions expecting a whole number answer.  
  - `Float`: For survey questions expecting a decimal number.  
  - `MultipleChoice`: For survey questions where one option from a list is required.  
  - `MultiSelect`: For survey questions where multiple items from a presented list can be selected.  
- `QuestionName`: The question to ask the user.  
- `QuestionDescription`: Optional description of the question.  
- `Variable`: Variable name to store the response. This is the variable to be used by the playbook. Variable names cannot contain spaces.  
- `Required`: Whether or not an answer to the question is required.  
- `Default`: Default value of the question  
- `Choices`: Array of the question's choices for `MultipleChoice` or `MultiSelect`,   
- `Min`: Minimum number or string length (Default = 0)  
- `Max`: Maximum number or string length (Default = 1024)

```yaml
Type: SurveySpec[]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Template
JobTemplate or WorkflowJobTemplate object.

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

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.IResource
JobTemplate or WorkflowJobTemplate object.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-AnsibleSurveySpec](Get-AnsibleSurveySpec.md)

[Remove-AnsibleSurveySpec](Remove-AnsibleSurveySpec.md)
