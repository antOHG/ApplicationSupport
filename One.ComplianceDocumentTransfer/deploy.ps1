######### START #########
######### Create folder structure #########
"creating folder structure"
New-Item -ItemType Directory -Force -Path E:\Services\One.ComplianceDocumentTransfer
New-Item -ItemType Directory -Force -Path E:\Services\One.ComplianceDocumentTransfer\Logs

######### Copy files #########
"copying files"
Copy-Item *.* E:\Services\One.ComplianceDocumentTransfer -Recurse -Force

######### Delete service if already exists #########
"deleting the service"
$serviceName = "One.ComplianceDocumentTransfer"

if (Get-Service $serviceName -ErrorAction SilentlyContinue)
{
    $serviceToRemove = Get-WmiObject -Class Win32_Service -Filter "name='$serviceName'"
    $serviceToRemove.delete()
    "service removed"
}
else
{
    "service does not exists"
}

"installing service"

Write-Host "Press any key to continue..."
[void][System.Console]::ReadKey($true)

######### Create service #########
$secpasswd = ConvertTo-SecureString "T3nTimesTable" -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("ohg\SRV_BSuServices", $secpasswd)
$binaryPath = "E:\Services\One.ComplianceDocumentTransfer\One.ComplianceDocumentTransfer.exe"
New-Service -name $serviceName -binaryPathName $binaryPath -displayName $serviceName -startupType Automatic -credential $mycreds

"installation completed"

Write-Host "Press any key to exit..."
[void][System.Console]::ReadKey($true)
######### START #########