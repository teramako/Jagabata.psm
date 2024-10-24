# Usage and Settings

## Register AnsibleTower/AWX host and Parsonal Access Token

First, you need to create/get an OAuth2 Personal Access Token (PAT) and
create configuration using [`New-AnsibleApiConfig`](./cmdlets/New-AnsibleApiConfig.md) command to register AnsibleTower/AWX host and the PAT.


 1. Login to AnsibleTower/AWX
 2. Create/Get OAuth2 Personal Access Token  
    See detail: https://docs.ansible.com/automation-controller/latest/html/userguide/users.html#users-tokens
    1. Access to your user profile page.
    2. Click the `Tokens` tab.
    3. Click the `Add` button, which opens the Create Token Window.
    4. Enter the following data in Create Token Window.
        - Application        : blank (don't choose an application)
        - Description        : a short description (optional)
        - Scope (*required*) : specify the level of access you want this token to have
    5. When done, click `Save` or `Cancel` to abandon your changes.
       After the token is saved, the newly created token displays with the token information.
       - ⚠️ Caution: the token value is shown **only at the time**. Don't close until following registration or save other place
 3. Launch Powershell 7 and register the PAT  
    ![demo](../demo/demo_0_CreateConfig.gif)
    1. Launch PowerShell 7
    2. Import `Jagabata.psm` module
       ```powershell
       Import-Module path\to\dir\Jagabata.psm.psd1
       ```
    3. Execute `New-AnsibleApiConfig` command
       ```powershell
       New-AnsibleApiConfig -Uri https://host.domain/
       ```
       - The input prompt to register the PAT will be shown
       - After entering the token, a test connection is made.
    4. When success, the configuration is save to following file:
        - Windows: `%USERPROFILE%\.ansible_api_config.json`
        - Linux: `$HOME/.ansible_api_config.json`


### Environment Variable

Set `ANSIBLE_API_CONFIG` environment, if you want to use another configuration file.

### Get the currently used configuration

Use [`Get-AnsibleApiConfig`](./cmdlets/Get-AnsibleApiConfig.md) command

### Switch to anothor configuration

Use [`Switch-AnsibleApiConfig`](./cmdlets/Switch-AnsibleApiConfig.md) command


## Import module

Onece the above registration is completed, only module importing is required thereafter.

```powershell
Import-Module path\to\dir\Jagabata.psm
```

You may/can put the Jagabata.psm module directory(`path\to\dir\`) into `$env:PSModulePath`,
and import implicitly.

### Change prefix

By default, "Ansible" command prefix is added.
If you want to change the prefix, use `-Prefix` parameter

```powershell
Import-Module path\to\dir\Jagabata.psm -Prefix Awx
```
In this case, **ALL** imported commands will be added `AWX` prefix (_verb-`AWX`Noun_).
You can run `Get-AnsibleHost` as PowerShell native command, `Get-AwxHost` as Jagabata.psm's command.
