Output demo_2_start_jobtemplate.gif

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

Type "# Demo: Asynchronous job execution and Wait"
Enter 2

Type "Import-Module ./out/Jagabata.psm"
Enter
Sleep 200ms

Type "Find-AnsibleJobTemplate -Search test_"
Sleep 2s
Enter
Sleep 4s

Type 'Start-AnsibleJobTemplate -Id 10 -Limit "localhost,gitrepo"'
Sleep 2s
Enter
Sleep 1s 

Type '$resultJobs = Find-AnsibleJob -Status running | Wait-AnsibleUnifiedJob'
Sleep 500ms
Enter
Sleep 15s 
Enter 2

Type '$resultJobs | fl Id,Name,JobType,Playbook,Status,Started,Finished'
Sleep 500ms
Enter
Sleep 10s

