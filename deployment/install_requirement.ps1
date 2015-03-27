Write-Host "Checking OS version..."
Write-Host ""
Start-Sleep -s 3
$os = Get-WmiObject -class Win32_OperatingSystem
$os.Caption
if ($os.version -lt 6)
    { Write-Host "Please install RXDev on Windows Vista/Sever 2008 or latest version..."
      exit
    }
    else {}
Write-Host "OS version is compatible..."
Write-Host ""

if($PSVersionTable.PSVersion.Major -lt 3)
{
    if ([System.IntPtr]::Size -eq 4)
    {
        Write-Host "Installing Powershell 4.0"	
        Start-Process -FilePath .\installer\x86\Windows6.1-KB2819745-x86-MultiPkg.exe -ArgumentList "/passive /norestart" -PassThru | Wait-Process
        Write-Host "Powershell v4.0 installed!"
        Write-Host "Please close this console and run this script again!"
    }
    else {}
    else
    {
        Write-Host "Installing Powershell 4.0"	
        Start-Process -FilePath .\installer\x64\Windows6.1-KB2819745-x64-MultiPkg.exe -ArgumentList "/passive /norestart" -PassThru | Wait-Process
        Write-Host "Powershell v4.0 installed!"
        Write-Host "Please close this console and run this script again!"        
    }
    else {}
}

Write-host "This installer will automatically install .NET 4.5.1 Framework, SQL Server LocalDB, Erlang and JRE to their default path as a requirement to run RXDev."
$Proceed = Read-Host "Do you want to proceed? (y/n)"
if ($Proceed -eq 'n')
    { Write-Host "Thank you. See you again!!!"
	exit
	}
    else
    {}

Write-Host "Installing Google Chrome"	
Start-Process -FilePath .\installer\ChromeStandaloneSetup.exe -ArgumentList "/silent /install" -PassThru | Wait-Process
Write-Host "Google Chrome installed!"
    

Write-Host "Installing .NET 4.5.1 Framework..."	
Start-Process -FilePath .\installer\NDP451-KB2858728-x86-x64-AllOS-ENU.exe -ArgumentList "/passive /norestart" -PassThru | Wait-Process
Write-Host ".Net 4.5.1 Framework installed!"
    
     
if ([System.IntPtr]::Size -eq 4) 
    
    {  
             
     Write-host "Installing SQL Server LocalDB..."
     msiexec.exe /qb /I ".\installer\x86\msodbcsql_x86.msi" iacceptmsodbcsqllicenseterms=yes | Wait-Process
     msiexec.exe /qb /I ".\installer\x86\MsSqlCmdLnUtils_x86.msi" iacceptmssqlcmdlnutilslicenseterms=yes | Wait-Process
     msiexec.exe /qb /I ".\installer\x86\SqlLocalDBx86.MSI" iacceptsqllocaldblicenseterms=yes | Wait-Process
     Write-Host "SQL Server LocalDB installed!"
     

    Write-Host "Installing Erlang..."
    Start-Process -FilePath .\installer\x86\otp_win32_17.0.exe -ArgumentList "/S" -Wait -Verb RunAs
    setx /M ERLANG_HOME "C:\Program Files\erl6.0"
    Write-Host "Erlang installed!"
    Write-Host "Installing Java Runtime Environment..."
    Start-Process -FilePath .\installer\x86\jre-7u71-windows-i586.exe -ArgumentList "/q /s /norestart" -Wait -Verb RunAs
    setx /M JAVA_HOME "C:\Program Files\Java\jre7"
    Write-Host "Java Runtime Environment installed!"
    }

    else
    {
        Write-host "Installing SQL Server LocalDB..."
        msiexec.exe /qb /I ".\installer\x64\msodbcsql_x64.msi" iacceptmsodbcsqllicenseterms=yes | Wait-Process
        msiexec.exe /qb /I ".\installer\x64\MsSqlCmdLnUtils_x64.msi" iacceptmssqlcmdlnutilslicenseterms=yes | Wait-Process
        msiexec.exe /qb /I ".\installer\x64\SqlLocalDBx64.MSI" iacceptsqllocaldblicenseterms=yes | Wait-Process
        Write-Host "SQL Server LocalDB installed!"
     

    Write-Host "Installing Erlang..."
    Start-Process -FilePath .\installer\x64\otp_win64_17.0.exe -ArgumentList "/S" -Wait -Verb RunAs
    setx /M ERLANG_HOME "C:\Program Files\erl6.0"
    Write-Host "Erlang installed!"
    Write-Host "Installing Java Runtime Environment..."
    Start-Process -FilePath .\installer\x64\jre-7u71-windows-x64.exe -ArgumentList "/q /s /norestart" -Wait -Verb RunAs
    setx /M JAVA_HOME "C:\Program Files\Java\jre7"
    Write-Host "Java Runtime Environment installed!"
    }     
    

    Write-Host "Computer must be restarted to complete the installation"
    $restart = Read-Host "Do you want to restart now? (y/n)"
    if  ($restart -eq 'y')
    {
     Start-Sleep -s 5
     Write-Host "Restarting computer..."
     Restart-Computer
    }

    else 
    { Write-Host "Installation for RXDev is not completed yet. Please restart your computer..."
      
	exit
	}