@echo off
set "PORT=8000"
cd /d "%~dp0"
echo Starting local web server on http://localhost:%PORT%
python -m http.server %PORT% 2>nul || py -3 -m http.server %PORT% 2>nul
if errorlevel 1 (
  echo.
  echo Python was not found. You can still open index.html directly in your browser.
  pause
)
