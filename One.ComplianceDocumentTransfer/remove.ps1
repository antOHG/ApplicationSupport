######### START #########

######### Copy files #########
"Remove files"
Remove-Item E:\Services\One.ComplianceDocumentTransfer -Recurse

######### Delete service if exists #########
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

Write-Host "Press any key to exit..."
[void][System.Console]::ReadKey($true)
######### START #########