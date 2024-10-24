Output demo_0_CreateConfig.gif

Set FontSize 12
Set Width 800
Set Height 600

Type "pwsh"
Enter
Sleep 1s

Type "# Demo: Create config file"
Enter 2

Type "Import-Module ./out/Jagabata.psm"
Enter
Sleep 200ms

Type "New-AnsibleApiConfig -Uri http://localhost:8013/"
Sleep 500ms
Enter
Sleep 200ms

Type "MHTEwjCDusGGHUGI017liSTYKgHt73"
Sleep 200ms
Enter
Sleep 2s

Type "Get-AnsibleApiConfig"
Sleep 500ms
Enter
Sleep 2s

Type "Get-ChildItem ~/.ansible_api_config.json -Hidden"
Sleep 500ms
Enter
Sleep 10s

