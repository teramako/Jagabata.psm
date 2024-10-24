Output demo_1_invoke_jobtemplate.gif

Set FontSize 12
Set Width 800
Set Height 600

Hide
Type "figlet 'Jagabata.psm'"
Enter
Show
Sleep 200ms

Type "pwsh"
Enter
Sleep 1s

Type "# Demo: Synchronous job execution"
Enter 2

Type "Import-Module ./out/Jagabata.psm"
Enter
Sleep 200ms

Type "Find-AnsibleJobTemplate -Search test_ -ov jt"
Sleep 2s
Enter
Sleep 4s

Type '$jt[0] | Invoke-AnsibleJobTemplate -Limit "localhost"'
Sleep 2s
Enter
Sleep 20s

Type 'Find-AnsibleJob -Status successful -OrderBy !finished -Count 1 -ov jobs'
Sleep 2s
Enter
Sleep 4s

Type '$jobs | Get-AnsibleJobLog'
Sleep 2s
Enter
Sleep 10s

