@echo off
setlocal

set SCRIPT_DIR=%~dp0
set MOBILE_DIR=%SCRIPT_DIR%..
set ROOT_DIR=%MOBILE_DIR%\..
set CONTRACTS_DIR=%ROOT_DIR%\contracts
set API_PROJECT=%ROOT_DIR%\src\HealthScan.Api
set API_URL=http://localhost:5000

if not exist "%CONTRACTS_DIR%" mkdir "%CONTRACTS_DIR%"

echo Starting backend server in background...
cd /d "%API_PROJECT%"
start /b dotnet run --urls %API_URL%

echo Waiting for server to start...
:wait_loop
timeout /t 1 /nobreak >nul
curl -s %API_URL%/health >nul 2>&1
if errorlevel 1 goto wait_loop
echo Server is ready!

echo Fetching OpenAPI spec...
curl -s %API_URL%/swagger/v1/swagger.json > "%CONTRACTS_DIR%\openapi.json"

echo Stopping backend server...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :5000 ^| findstr LISTENING') do taskkill /PID %%a /F >nul 2>&1

echo Generating TypeScript client...
cd /d "%MOBILE_DIR%"
call npx openapi-typescript-codegen --input "%CONTRACTS_DIR%\openapi.json" --output ./src/infrastructure/api/generated --client axios --useOptions --useUnionTypes
if errorlevel 1 (
    echo Error: TypeScript client generation failed
    exit /b 1
)

echo API client generated successfully!
