---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-ApiHelp

## SYNOPSIS
Get and show Ansible's API Help.

## SYNTAX

```
Get-ApiHelp [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
Retrieve API Document and show for the specified URL path using AWX/AnsibleTower Rest API by `OPTIONS` method.

See the following URL for a list of API URL paths:  
- `https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/api_ref.html`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ApiHelp /api/v2/jobs/

Name                : Job List
Description         : List Jobs:

                      Make a GET request to this resource to retrieve the list of jobs.

(snip)

Renders             : application/json
                      text/html
Parses              : application/json
Actions             : GET
Types               : job
SearchFields        : description
                      name
RelatedSearchFields : job_host_summaries
                      modified_by
(snip)
                      schedule
                      instance_group
ObjectRoles         :
MaxPageSize         : 200
```

## PARAMETERS

### -Path
URL path of the Rest API.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.ApiHelp
## NOTES

## RELATED LINKS
