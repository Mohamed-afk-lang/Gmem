$port = 8000
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root
Write-Host "Starting local web server on http://localhost:$port"

try {
    python -m http.server $port
} catch {
    try {
        py -3 -m http.server $port
    } catch {
        Write-Host "Python was not found. Open index.html directly in your browser." -ForegroundColor Yellow
        Read-Host "Press ENTER to exit"
    }
}
