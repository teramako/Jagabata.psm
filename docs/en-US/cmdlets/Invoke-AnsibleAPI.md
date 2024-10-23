---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Invoke-AnsibleAPI

## SYNOPSIS
Execute Ansible's (low-level) Rest API.

## SYNTAX

### NonSendData (Default)
```
Invoke-AnsibleAPI [-Method] <Method> [-Path] <String> [[-QueryString] <String>] [-AsRawString]
 [<CommonParameters>]
```

### SendData
```
Invoke-AnsibleAPI [-Method] <Method> [-Path] <String> [[-QueryString] <String>] -SenData <Object>
 [-AsRawString] [<CommonParameters>]
```

## DESCRIPTION
Make GET, POST, PUT, PATCH, DELETE or OPTIONS request to specified Path, and get the response.

## EXAMPLES

### Example 1
```powershell
PS C:\> Invoke-AnsibleAPI GET /api/v2/ping/ -AsRawString
{"ha":false,"version":"23.3.1","active_node":"awx_1", ... }
```

Make `GET` request to `/api/v2/ping/` and get response as String

### Example 2
```powershell
PS C:\> Invoke-AnsibleAPI PATCH /api/v2/users/2/ -SenData @{ last_name = "mako" }

Id Type Username Email              FirstName LastName IsSuperuser IsSystemAuditor Created            Modified            LastLogin           LdapDn ExternalAccount
-- ---- -------- -----              --------- -------- ----------- --------------- -------            --------            ---------           ------ ---------------
 2 User teramako teramako@gmail.com tera      mako           False           False 2024/05/21 0:13:43 2024/06/10 22:48:18 2024/06/10 22:48:18
```

Make `PATCH` request to `/api/v2/users/2/` for change `last_name` field.

## PARAMETERS

### -AsRawString
Returns response data as raw string instead of deserializing to object.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Method
HTTP method to make request.

```yaml
Type: Method
Parameter Sets: (All)
Aliases:
Accepted values: GET, POST, PUT, PATCH, DELETE, OPTIONS

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
URL path to make request.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -QueryString
URL query to make request.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SenData
Data to be sent to the API.
When value is not String, it will be deserialized to JSON string.

```yaml
Type: Object
Parameter Sets: SendData
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Object
Data to be sent to the API.
When value is not String, it will be deserialized to JSON string.

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS

[Get-AnsibleApiHelp](Get-AnsibleApiHelp.md)
