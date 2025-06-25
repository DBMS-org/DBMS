$body = @{
    email = "zuhranyousaf12345@gmail.com"
} | ConvertTo-Json

try {
    Write-Host "Testing forgot password API..." -ForegroundColor Cyan
    $response = Invoke-RestMethod -Uri "http://127.0.0.1:5019/api/auth/forgot-password" -Method Post -ContentType "application/json" -Body $body
    Write-Host "Success!" -ForegroundColor Green
    Write-Host "Response: $($response | ConvertTo-Json -Depth 5)" -ForegroundColor Cyan
    
    if ($response.Data -and $response.Data.TestingCode) {
        Write-Host ""
        Write-Host "*** VERIFICATION CODE FOR TESTING ***" -ForegroundColor Yellow -BackgroundColor DarkBlue
        Write-Host "CODE: $($response.Data.TestingCode)" -ForegroundColor White -BackgroundColor DarkRed
        Write-Host "Email: zuhranyousaf12345@gmail.com" -ForegroundColor Yellow
        Write-Host "*** USE THIS CODE TO TEST THE VERIFY PAGE ***" -ForegroundColor Yellow -BackgroundColor DarkBlue
        Write-Host ""
        
        # Also test email service directly
        Write-Host "EMAIL SERVICE STATUS:" -ForegroundColor Magenta
        if ($response.Message -like "*console logs*") {
            Write-Host "⚠️  Email service may not be working - check console/logs" -ForegroundColor Yellow
        } else {
            Write-Host "✅ Email should have been sent successfully" -ForegroundColor Green
        }
    }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        Write-Host "Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
        try {
            $errorContent = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($errorContent)
            $errorBody = $reader.ReadToEnd()
            Write-Host "Error Body: $errorBody" -ForegroundColor Red
        } catch {
            Write-Host "Could not read error details" -ForegroundColor Red
        }
    }
} 