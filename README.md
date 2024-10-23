# Jagabata.psm 🥔🧈
<img src="docs/img/Jagabata.png" width="256" height="256" align="right"/>

PowerShell module to operate AWX/AnsibleTower using [Rest API].

- Required: PowerShell 7
- Supports Windows, GNU/Linux, macOS
- Much number of commands are available (nearly 200). See: Command list [by verb] or [by noun].
  - Retrieve various information (`Get-*`, `Find-*`)
  - Launch, wait or stop jobs such as JobTemplate, WorkflowJobTemplate, etc (`Start-*`, `Invoke-*`, `Wait-`, `Stop-*`)
  - Creating new resources such as Users, JobTemplates, Credential, etc. (`New-*`)
  - Update existing resources (`Update-*`)
  - Associate or Unassociate a resource with another resource (`Register-*`, `Unregister-*`)
  - Delete resources (`Remove-*`)

[by verb]: docs/en-US/CommandListByVerb.md
[by noun]: docs/en-US/CommandListByNoun.md

![demo1](docs/img/Jagabata.psm-demo-1.gif)

## 🚀 Get Started

### 1. Install Jagabata.psm from [PowerShell Gallery].

```powershell
Install-Module -Name Jagabata.psm -Scope CurrentUser -AllowClobber
```

### 2. Import module

```powershell
Import-Module Jagabata.psm
```

All commands of Jagabata.psm are added "Ansible" prefix by default, like `Get-Host` -> `Get-AnsibleHost`

> [!TIP]
> Please use the `-Prefix` parameter to change the prefix.
> For example:
> ```powershell
> Import-Module Jagabata.psm -Prefix Awx
> ```
> All commands of Jagabata.psm will be added "Awx" prefix instead of "Ansibl", like `Get-Host` -> `Get-AwxHost`.

### 3. Get Poersonal Access Token

Login to AWX/AnsibleTower to obtain a Personal Access Token (PAT) and
create a configuration file with the [New-ApiConfig](./docs/en-US/cmdlets/New-ApiConfig.md) command.  

See [Settings](./docs/en-US/settings.md) for more details.

### 4. Completed

Now you are ready. Now execute your favorite command!

## 🚧 Build

See [Build](./docs/en-US/build.md) document.

[Rest API]: https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/index.html "AWX API Reference — Ansible AWX community documentation"
[Powershell Gallery]: https://www.powershellgallery.com/packages/Jagabata.psm
