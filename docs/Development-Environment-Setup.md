# SQL Automation Next — Development Environment Setup

This document explains how to prepare a Windows development machine for **SQL Automation Next**.

The project uses:

- React with TypeScript for the frontend
- ASP.NET Core 10 for backend services
- YARP for the API Gateway
- SQL Server as the first database provider
- PostgreSQL and MySQL as later providers
- Docker Desktop for containers and local infrastructure
- Git and GitHub for source control

Follow the installation order in this document. Do not install PostgreSQL or MySQL directly on Windows unless a later project requirement explicitly asks for it.

---

## 1. Supported Development Environment

### Operating system

- Windows 11 64-bit is recommended.
- Windows 10 22H2 64-bit may work if it supports WSL 2 and Docker Desktop.
- Hardware virtualization must be available and enabled.

### Hardware

| Resource | Minimum | Recommended |
|---|---:|---:|
| RAM | 8 GB | 16 GB or more |
| CPU | 64-bit, 2 cores, virtualization support | 4 or more cores |
| Free disk space | 15 GB | 30 GB or more |
| Storage | SSD | SSD/NVMe |

An 8 GB system can run the project, but Docker and all microservices should not remain active unnecessarily. See [Resource guidance for an 8 GB system](#11-resource-guidance-for-an-8-gb-system).

---

## 2. Required Tools

| Tool | Project requirement | Purpose |
|---|---|---|
| Git for Windows | Current supported release | Source control |
| Node.js | Version 22 or newer LTS | React tooling |
| npm | Bundled with Node.js | Frontend packages and scripts |
| .NET SDK | .NET 10 SDK | Backend development |
| Visual Studio Code | Current supported release | Primary editor |
| Windows Subsystem for Linux | WSL 2.1.5 or newer | Docker Desktop backend |
| Docker Desktop | Current supported release | Local containers |
| SQL Server | Local Developer/Express or later container | Initial database provider |
| SQL Server Management Studio | Current supported release | SQL Server inspection and troubleshooting |

### Verified development machine

The initial project machine was verified with:

```text
Git:             2.55.0.windows.3
Node.js:         22.23.2
npm:             12.0.2
.NET SDK:        10.0.400
VS Code:         1.135.0
WSL:             2.7.12.0
Docker Engine:   29.7.2
Docker Compose:  5.5.0
```

Newer compatible patch versions are acceptable. Avoid preview SDKs unless the project intentionally adopts one.

---

## 3. Installation Order

Install and verify tools in this order:

1. Git for Windows
2. Visual Studio Code
3. Node.js LTS and npm
4. .NET 10 SDK
5. WSL 2 and Windows virtualization components
6. Docker Desktop
7. SQL Server and SQL Server Management Studio, if not already available
8. Clone the GitHub repository
9. Install project dependencies after the repository structure exists

Restart Windows whenever an installer or Windows feature explicitly requires it.

---

## 4. Install Git for Windows

### Download

Download Git from the official site:

[Download Git for Windows](https://git-scm.com/download/win)

### Installation

1. Run the downloaded installer.
2. Keep Git Credential Manager enabled.
3. Allow Git to be used from PowerShell and other command-line tools.
4. Keep the recommended line-ending configuration for Windows.
5. Complete the installation.

### Verify

Open a new PowerShell window:

```powershell
git --version
```

### Configure identity

Use the name and email associated with the GitHub account:

```powershell
git config --global user.name "Your Name"
```

```powershell
git config --global user.email "your-github-email@example.com"
```

Verify:

```powershell
git config --global --list
```

Never place a GitHub password or access token inside repository files.

---

## 5. Install Visual Studio Code

### Download

[Download Visual Studio Code for Windows](https://code.visualstudio.com/download)

The **User Installer for Windows x64** is suitable for most developers.

### Installation

Enable these installer options when available:

- Add “Open with Code” to the Windows Explorer file context menu
- Add “Open with Code” to the directory context menu
- Register VS Code as an editor for supported file types
- Add VS Code to `PATH`

### Verify

Open a new PowerShell window:

```powershell
code --version
```

### Recommended extensions

Install extensions only from trusted publishers.

Frontend:

- ESLint
- Prettier
- SCSS IntelliSense or another maintained SCSS helper

.NET:

- C# Dev Kit by Microsoft
- C# by Microsoft

Infrastructure:

- Docker by Microsoft
- GitHub Actions by GitHub

Database extensions are optional because SQL Server Management Studio will be used for detailed SQL Server work.

---

## 6. Install Node.js and npm

### Download

[Download Node.js](https://nodejs.org/en/download)

On Windows, install an **LTS** release using the official x64 installer. Node.js includes npm.

The project supports Node.js 22 or newer compatible LTS versions. The repository will later define the exact supported range in `package.json` and a version file.

### Verify

```powershell
node --version
```

```powershell
npm --version
```

Expected minimum Node output:

```text
v22.x.x
```

Do not install frontend libraries globally unless their official documentation specifically requires it. Project dependencies belong in the project’s `package.json`.

---

## 7. Install the .NET 10 SDK

The **SDK** is required. Installing only the runtime is not sufficient for development.

### Download

[Download the .NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Select the Windows x64 SDK installer.

### Verify

```powershell
dotnet --version
```

```powershell
dotnet --list-sdks
```

Expected output must contain a `10.0.x` SDK:

```text
10.0.xxx [C:\Program Files\dotnet\sdk]
```

The project will later use `global.json` to keep developer and CI SDK behaviour consistent.

---

## 8. Install and Configure WSL 2

Docker Desktop uses the WSL 2 virtualization backend on Windows.

### 8.1 Check hardware virtualization

1. Press `Ctrl + Shift + Esc` to open Task Manager.
2. Open **Performance**.
3. Select **CPU**.
4. Confirm that the page shows:

```text
Virtualization: Enabled
```

If it shows `Disabled`, enable Intel VT-x/Intel Virtualization Technology or AMD-V/SVM in the computer’s BIOS/UEFI settings. The exact menu depends on the manufacturer.

### 8.2 Check whether WSL is installed

```powershell
wsl --version
```

If WSL is not installed, open **PowerShell as Administrator** and run:

```powershell
wsl --install --no-distribution
```

The `--no-distribution` option installs the WSL platform without adding an unnecessary Ubuntu environment. If the installed Windows version does not support that option, use:

```powershell
wsl --install
```

Restart Windows when prompted.

### 8.3 Enable required Windows features manually

If WSL reports that Virtual Machine Platform is missing, open **PowerShell as Administrator** and run:

```powershell
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
```

```powershell
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
```

Restart Windows after both commands complete successfully.

### 8.4 Verify Windows features

The following commands require **PowerShell as Administrator**:

```powershell
Get-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform | Select-Object FeatureName, State
```

```powershell
Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux | Select-Object FeatureName, State
```

Both should show:

```text
State: Enabled
```

### 8.5 Ensure the hypervisor starts with Windows

If WSL still says that virtualization cannot be detected even though Task Manager shows it as enabled, open **PowerShell as Administrator** and run:

```powershell
bcdedit /set hypervisorlaunchtype auto
```

Restart Windows using **Restart**, not Shut down.

Verify that Windows detects the hypervisor:

```powershell
Get-CimInstance Win32_ComputerSystem | Select-Object HypervisorPresent
```

Expected result:

```text
HypervisorPresent
-----------------
True
```

### 8.6 Final WSL verification

```powershell
wsl --version
```

```powershell
wsl --status
```

The default version should be `2`.

Official references:

- [Install WSL](https://learn.microsoft.com/en-us/windows/wsl/install)
- [Manual WSL feature setup](https://learn.microsoft.com/en-us/windows/wsl/install-manual)
- [WSL troubleshooting](https://learn.microsoft.com/en-us/windows/wsl/troubleshooting)

---

## 9. Install Docker Desktop

### Requirements

- Hardware virtualization enabled
- WSL 2.1.5 or newer
- Virtual Machine Platform enabled
- 8 GB RAM minimum

### Download

[Install Docker Desktop on Windows](https://docs.docker.com/desktop/setup/install/windows-install/)

For a normal Intel or AMD Windows computer, download the **x86_64** installer.

### Installation

1. Run `Docker Desktop Installer.exe`.
2. Select the recommended per-user installation when offered.
3. Select **Use WSL 2 instead of Hyper-V**.
4. Keep Linux containers as the default.
5. Complete the installation.
6. Start Docker Desktop.
7. Accept the Docker agreement.
8. Signing in is optional for normal local development unless an organizational policy requires it.
9. Wait until Docker reports that the engine is running.

### Verify the CLI

```powershell
docker --version
```

```powershell
docker compose version
```

### Verify the engine

```powershell
docker info
```

A working installation shows both `Client` and `Server` sections.

Run a test container:

```powershell
docker run --rm hello-world
```

Expected output includes:

```text
Hello from Docker!
This message shows that your installation appears to be working correctly.
```

### If Docker shows “Virtualization support not detected”

1. Close Docker Desktop.
2. Confirm virtualization is enabled in Task Manager.
3. Confirm both WSL Windows features are enabled.
4. Confirm `HypervisorPresent` is `True`.
5. Run the following in **PowerShell as Administrator**:

```powershell
bcdedit /set hypervisorlaunchtype auto
```

6. Restart Windows.
7. Start Docker Desktop again.

### If `docker info` shows only Client

The CLI is installed, but the Docker Engine is not running. Start Docker Desktop and wait until its status changes to **Engine running**.

---

## 10. SQL Server and Database Tools

SQL Server is the first fully implemented database provider.

### Option A — Existing local SQL Server

Use an existing local SQL Server 2025 Developer or Express installation if it is already working. This option uses less Docker memory on an 8 GB computer.

Verify the instance through SQL Server Management Studio and record only the non-secret connection details locally.

Common local server names include:

```text
localhost
localhost\SQLEXPRESS
.
.\SQLEXPRESS
```

The correct value depends on the installed instance.

### Option B — SQL Server container

A SQL Server container will be added through the project’s Docker Compose configuration later. Do not create an unmanaged container manually before the Compose file exists.

### Download SQL Server

[Microsoft SQL Server downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

For local development and testing:

- Developer edition is free for non-production development and testing.
- Express edition is suitable for lightweight local use.

### Install SQL Server Management Studio

[Install SQL Server Management Studio](https://learn.microsoft.com/en-us/ssms/install/install)

SSMS is recommended for:

- Verifying database connectivity
- Inspecting tables and relationships
- Running diagnostic SQL
- Confirming the results of application operations
- Managing local SQL Server logins and permissions

### PostgreSQL and MySQL

Do not install PostgreSQL or MySQL directly on Windows for the initial milestone. Their provider work will use versioned Docker images later, normally one database container at a time on an 8 GB machine.

---

## 11. Resource Guidance for an 8 GB System

The project supports an 8 GB development computer with a conservative workflow.

- Do not start Docker Desktop automatically with Windows unless needed.
- Quit Docker Desktop when working only on frontend components.
- Run React through Vite directly on Windows.
- Run .NET services with `dotnet run` during normal development.
- Use Docker primarily for databases and infrastructure.
- Start only the service and database needed for the current milestone.
- Avoid running SQL Server, PostgreSQL, MySQL, RabbitMQ, and every microservice container simultaneously.
- Close unused browsers, IDEs, and database tools when memory usage is high.
- Prefer the existing local SQL Server instance for the first provider if it is already installed and healthy.

Do not add a global WSL memory limit until real usage is measured. An overly small limit can prevent database containers from starting.

---

## 12. GitHub Repository and Local Clone

Repository:

```text
https://github.com/shubhamdsk/SQL-Automation-Next
```

### Clone

```powershell
New-Item -ItemType Directory -Path "D:\Projects" -Force
```

```powershell
Set-Location "D:\Projects"
```

```powershell
git clone https://github.com/shubhamdsk/SQL-Automation-Next.git
```

```powershell
Set-Location "D:\Projects\SQL-Automation-Next"
```

### Confirm the default branch

```powershell
git branch --show-current
```

Expected output after cloning:

```text
main
```

### Create the project-foundation branch

The `main` branch is protected, so development happens on a separate branch:

```powershell
git switch -c chore/project-foundation
```

Verify:

```powershell
git branch --show-current
```

Expected output:

```text
chore/project-foundation
```

---

## 13. Complete Verification Commands

Run these commands from PowerShell after installing all required tools:

```powershell
git --version
```

```powershell
node --version
```

```powershell
npm --version
```

```powershell
dotnet --version
```

```powershell
dotnet --list-sdks
```

```powershell
code --version
```

```powershell
wsl --version
```

```powershell
wsl --status
```

Start Docker Desktop before running:

```powershell
docker --version
```

```powershell
docker compose version
```

```powershell
docker info
```

```powershell
docker run --rm hello-world
```

---

## 14. Security Rules for Local Setup

- Never commit database passwords.
- Never commit GitHub tokens.
- Never commit JWT signing keys.
- Never commit full production connection strings.
- Never store secrets in the React application.
- Frontend `.env` values are not secure secrets because they can become part of the browser bundle.
- Use .NET User Secrets during early local development.
- Use ignored local environment files only for non-committed development configuration.
- Provide safe `.example` configuration files with placeholders.
- Keep GitHub secret scanning and push protection enabled.

Commands and conventions for project secrets will be added when the first service is created.

---

## 15. Common Problems

### `docker` is not recognized

Docker Desktop is not installed, or the terminal was opened before installation updated `PATH`.

1. Install Docker Desktop.
2. Close all PowerShell windows.
3. Open a new PowerShell window.
4. Run `docker --version` again.

### WSL is not installed

Error:

```text
The Windows Subsystem for Linux is not installed.
```

Run `wsl --install --no-distribution` from PowerShell as Administrator and restart Windows.

### WSL cannot detect virtualization

Confirm all of the following:

- Task Manager shows virtualization enabled.
- `VirtualMachinePlatform` state is enabled.
- `Microsoft-Windows-Subsystem-Linux` state is enabled.
- `HypervisorPresent` is true.
- `hypervisorlaunchtype` is set to auto if necessary.

### PowerShell says an operation requires elevation

Close the current terminal and reopen PowerShell using **Run as administrator**.

### Docker remains on “Engine starting”

1. Wait several minutes on the first startup.
2. Confirm WSL and virtualization requirements.
3. Restart Docker Desktop after WSL is healthy.
4. Use Docker Desktop troubleshooting only after basic Windows checks pass.

### `docker info` shows Client but not Server

Docker Desktop is installed, but its engine is stopped. Open Docker Desktop and wait for **Engine running**.

### High memory usage

Quit Docker Desktop when it is not required, close unused applications, and run only the infrastructure needed for the current milestone.

---

## 16. Environment Readiness Checklist

Before creating the project structure, confirm:

- [ ] Windows is fully restarted after WSL/virtualization feature changes.
- [ ] Git works from PowerShell.
- [ ] Git username and email are configured.
- [ ] Node.js 22 or a newer compatible LTS release is available.
- [ ] npm works.
- [ ] .NET 10 SDK is listed by `dotnet --list-sdks`.
- [ ] VS Code opens with the `code` command.
- [ ] Task Manager shows virtualization enabled.
- [ ] WSL default version is 2.
- [ ] Windows reports `HypervisorPresent` as true.
- [ ] Docker Desktop reaches Engine running.
- [ ] `docker info` contains a Server section.
- [ ] `docker run --rm hello-world` succeeds.
- [ ] SQL Server is available locally, or its later Docker setup is planned.
- [ ] SSMS is installed if local SQL Server will be used.
- [ ] The repository is cloned.
- [ ] The active branch is `chore/project-foundation`.

---

## 17. Official References

- [Git for Windows](https://git-scm.com/download/win)
- [Node.js downloads](https://nodejs.org/en/download)
- [.NET 10 downloads](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [VS Code Windows setup](https://code.visualstudio.com/docs/setup/windows)
- [Microsoft WSL installation](https://learn.microsoft.com/en-us/windows/wsl/install)
- [Microsoft WSL troubleshooting](https://learn.microsoft.com/en-us/windows/wsl/troubleshooting)
- [Docker Desktop Windows installation](https://docs.docker.com/desktop/setup/install/windows-install/)
- [Microsoft SQL Server downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio installation](https://learn.microsoft.com/en-us/ssms/install/install)

---

## 18. Current Setup Status

The initial developer machine has completed:

- Git installation and repository clone
- Node.js and npm verification
- .NET 10 SDK verification
- Visual Studio Code verification
- Hardware virtualization verification
- WSL 2 installation
- Virtual Machine Platform enablement
- Windows hypervisor boot enablement
- Docker Desktop installation
- Docker Engine verification through the `hello-world` container
- Creation of the `chore/project-foundation` branch

The environment is ready for the monorepo and solution foundation.

---

**Document:** Development Environment Setup<br>
**Project:** SQL Automation Next<br>
**Last reviewed:** 3 September 2026
