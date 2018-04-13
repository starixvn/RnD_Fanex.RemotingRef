[CmdletBinding(PositionalBinding=$false)]
param(
    [bool] $CreatePackages = $true,
    [bool] $RunTests = $true
)

Write-Host "Run Parameters:" -ForegroundColor Cyan
Write-Host "CreatePackages: $CreatePackages"
Write-Host "RunTests: $RunTests"

$packageOutputFolder = "$PSScriptRoot\.nupkgs"
$projectsToBuild =
    'Fanex.RemotingRef'
  #,'Fanex.RemotingRef.Extensions'

$testsToRun =
    'Fanex.RemotingRef.Tests'
  #,'Fanex.RemotingRef.Extensions.Tests'

if ($RunTests) {
    dotnet restore /ConsoleLoggerParameters:Verbosity=Quiet
    foreach ($project in $testsToRun) {
        Write-Host "Running tests: $project (all frameworks)" -ForegroundColor "Magenta"
        Push-Location ".\Tests\$project"

        dotnet test
        if ($LastExitCode -ne 0) {
            Write-Host "Error with tests, aborting build." -Foreground "Red"
            Pop-Location
            Exit 1
        }

        Write-Host "Tests passed!" -ForegroundColor "Green"
        Pop-Location
    }
}

if ($CreatePackages) {
    mkdir -Force $packageOutputFolder | Out-Null
    Write-Host "Clearing existing $packageOutputFolder..." -NoNewline
    Get-ChildItem $packageOutputFolder | Remove-Item
    Write-Host "Done." -ForegroundColor "Green"

    Write-Host "Building all packages" -ForegroundColor "Green"

    foreach ($project in $projectsToBuild) {
        Write-Host "Working on $project`:" -ForegroundColor "Magenta"
        Write-Host "  Restoring and packing $project..." -ForegroundColor "Magenta"

        Push-Location ".\Sources\$project"

        dotnet msbuild "/t:Restore;Pack" "/p:Configuration=Release" "/p:PackageOutputPath=$packageOutputFolder"

        Pop-Location

        Write-Host "Done." -ForegroundColor "Green"
        Write-Host ""
    }

    Write-Host "Build Complete." -ForegroundColor "Green"
}