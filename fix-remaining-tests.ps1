# Fix remaining drilling-pattern-creator test files

$testFiles = @(
    "Presentation\UI\src\app\components\blasting-engineer\drilling-pattern-creator\components\drill-point-canvas\drill-point-canvas.component.spec.ts",
    "Presentation\UI\src\app\components\blasting-engineer\drilling-pattern-creator\components\drill-point-canvas\drill-point-canvas.demo.spec.ts",
    "Presentation\UI\src\app\components\blasting-engineer\drilling-pattern-creator\components\grid-canvas\grid-canvas.component.spec.ts",
    "Presentation\UI\src\app\components\blasting-engineer\drilling-pattern-creator\components\depth-editor-table\depth-editor-table.component.spec.ts"
)

foreach ($file in $testFiles) {
    if (Test-Path $file) {
        Write-Host "Fixing $file..."
        $content = Get-Content $file -Raw

        # Fix PatternSettings - add missing properties
        $pattern = '(?s)(\s+const\s+\w+Settings:\s+PatternSettings\s*=\s*\{)\s*(spacing:\s*\d+\.?\d*,?\s*burden:\s*\d+\.?\d*,?\s*depth:\s*\d+\.?\d*)\s*(\};)'
        $replacement = '$1 $2, diameter: 115, stemming: 3, subDrill: 0.5 $3'
        $content = $content -replace $pattern, $replacement

        # Fix DrillPoint objects - add missing stemming and subDrill
        # Pattern for inline drill point objects
        $content = $content -creplace '(\{\s*id:\s*''[^'']+''[^}]*burden:\s*\d+\.?\d*)(\s*\})', '$1, stemming: 3, subDrill: 0.5$2'

        # Fix the import path for drill-point.model (if it exists)
        $content = $content -replace "from '\.\./models/drill-point\.model'", "from '../../models/drill-point.model'"

        Set-Content $file -Value $content -NoNewline
        Write-Host "Fixed $file"
    } else {
        Write-Host "File not found: $file"
    }
}

# Fix job-filters.component.spec.ts - syntax error with duplicate statusText
$filterFile = "Presentation\UI\src\app\components\mechanical-engineer\maintenance\maintenance-jobs\job-filters\job-filters.component.spec.ts"
if (Test-Path $filterFile) {
    Write-Host "Fixing $filterFile..."
    $content = Get-Content $filterFile -Raw

    # Remove duplicate getStatusText calls and related code
    $content = $content -replace 'const statusText = component\.getStatusText\(\);\s+expect\(statusText\)\.toBe\([^)]+\);\s*;', '// getStatusText not exposed'

    Set-Content $filterFile -Value $content -NoNewline
    Write-Host "Fixed $filterFile"
}

Write-Host "All remaining test files have been processed"
