Write-Host "Testing Login Functionality" -ForegroundColor Cyan
Write-Host "=============================" -ForegroundColor Cyan

# Test invalid user
Write-Host "`n1. Testing with invalid user:" -ForegroundColor Yellow
$invalidBody = @{
    username = "NonExistentUser"
    password = "wrongpass"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://127.0.0.1:5019/api/auth/login" -Method Post -ContentType "application/json" -Body $invalidBody
    Write-Host "✅ Login Success (unexpected)" -ForegroundColor Green
    Write-Host "Message: $($response.message)" -ForegroundColor Green
} catch {
    Write-Host "❌ Login Failed (expected)" -ForegroundColor Red
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $errorResponse = $reader.ReadToEnd()
    $reader.Close()
    $stream.Close()
    
    try {
        $errorJson = $errorResponse | ConvertFrom-Json
        Write-Host "Message: $($errorJson.message)" -ForegroundColor Red
    } catch {
        Write-Host "Raw Response: $errorResponse" -ForegroundColor Red
    }
}

# Test with known admin user
Write-Host "`n2. Testing with admin user (if exists):" -ForegroundColor Yellow
$adminBody = @{
    username = "Admin"
    password = "admin123"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://127.0.0.1:5019/api/auth/login" -Method Post -ContentType "application/json" -Body $adminBody
    Write-Host "✅ Login Success" -ForegroundColor Green
    Write-Host "Message: $($response.message)" -ForegroundColor Green
    Write-Host "User: $($response.user.name) - Status: $($response.user.status)" -ForegroundColor Green
} catch {
    Write-Host "❌ Login Failed" -ForegroundColor Red
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $errorResponse = $reader.ReadToEnd()
    $reader.Close()
    $stream.Close()
    
    try {
        $errorJson = $errorResponse | ConvertFrom-Json
        Write-Host "Message: $($errorJson.message)" -ForegroundColor Red
    } catch {
        Write-Host "Raw Response: $errorResponse" -ForegroundColor Red
    }
}

Write-Host "`n=============================" -ForegroundColor Cyan
Write-Host "Login Status Messages:" -ForegroundColor White
Write-Host "• Valid Login: 'Login successful'" -ForegroundColor Green
Write-Host "• Invalid User/Password: 'Invalid name or password'" -ForegroundColor Yellow
Write-Host "• Inactive Account: 'Your account is [status]. Please contact your administrator.'" -ForegroundColor Red
Write-Host "=============================" -ForegroundColor Cyan 