$ErrorActionPreference = "Stop"

$package = If ($IsMacOS) {"mcr.microsoft.com/azure-sql-edge"} else {"mcr.microsoft.com/mssql/server:2022-latest"}

& docker stop SqlServer

& docker rm SqlServer

& docker run `
    -e "ACCEPT_EULA=Y" `
    -e "MSSQL_SA_PASSWORD=Aa123456" `
    -p 1433:1433 `
    --name SqlServer `
    -d $package

if ($lastexitcode -ne 0)
{
    throw "Failed to run container"
}

& docker update --restart unless-stopped SqlServer

if ($lastexitcode -ne 0)
{
    throw "Failed to upgrade container start options"
}
