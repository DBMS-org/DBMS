const fs = require('fs');
const path = require('path');

console.log('🔍 Angular Build Optimizer');
console.log('============================\n');

// Check package.json for heavy dependencies
function analyzeDependencies() {
    console.log('📦 Analyzing Dependencies...\n');
    
    const packagePath = path.join(__dirname, 'package.json');
    const packageJson = JSON.parse(fs.readFileSync(packagePath, 'utf8'));
    
    const heavyDeps = {
        'three': 'Large 3D library (~600KB). Consider lazy loading or tree shaking.',
        'konva': '2D canvas library (~400KB). Consider lazy loading for visualization components.',
        '@angular/animations': 'Consider removing if not using animations extensively.',
        'rxjs': 'Essential but large. Ensure proper tree shaking.',
        'zone.js': 'Required for Angular but consider zones-free if possible.'
    };
    
    console.log('Heavy Dependencies Found:');
    Object.keys(packageJson.dependencies || {}).forEach(dep => {
        if (heavyDeps[dep]) {
            console.log(`  ⚠️  ${dep}: ${heavyDeps[dep]}`);
        }
    });
    
    console.log('\n📋 Optimization Recommendations:');
    console.log('  1. Implement lazy loading for feature modules');
    console.log('  2. Use OnPush change detection strategy');
    console.log('  3. Tree shake unused imports');
    console.log('  4. Consider code splitting for large libraries');
}

// Check for large SCSS files
function analyzeStyles() {
    console.log('\n🎨 Analyzing SCSS Files...\n');
    
    const srcPath = path.join(__dirname, 'src');
    const largeFiles = [];
    
    function scanDirectory(dir) {
        const files = fs.readdirSync(dir, { withFileTypes: true });
        
        files.forEach(file => {
            const filePath = path.join(dir, file.name);
            
            if (file.isDirectory()) {
                scanDirectory(filePath);
            } else if (file.name.endsWith('.scss')) {
                const stats = fs.statSync(filePath);
                const sizeKB = (stats.size / 1024).toFixed(2);
                
                if (stats.size > 4096) { // > 4KB
                    largeFiles.push({
                        path: filePath.replace(__dirname, '.'),
                        size: sizeKB
                    });
                }
            }
        });
    }
    
    if (fs.existsSync(srcPath)) {
        scanDirectory(srcPath);
        
        if (largeFiles.length > 0) {
            console.log('Large SCSS Files (> 4KB):');
            largeFiles
                .sort((a, b) => parseFloat(b.size) - parseFloat(a.size))
                .forEach(file => {
                    console.log(`  📄 ${file.path} (${file.size}KB)`);
                });
            
            console.log('\n🛠️  SCSS Optimization Tips:');
            console.log('  1. Extract common styles into mixins');
            console.log('  2. Use CSS custom properties (variables)');
            console.log('  3. Remove unused styles');
            console.log('  4. Consider CSS-in-JS for dynamic styles');
        }
    }
}

// Provide build optimization suggestions
function provideBuildTips() {
    console.log('\n🚀 Build Optimization Checklist:');
    console.log('================================\n');
    
    const tips = [
        '✅ Enable production mode (--configuration=production)',
        '✅ Use AOT compilation (enabled by default)',
        '✅ Enable tree shaking (enabled by default)',
        '⚡ Implement lazy loading for routes',
        '⚡ Use OnPush change detection strategy',
        '⚡ Optimize images and assets',
        '⚡ Remove unused dependencies',
        '⚡ Use trackBy functions in *ngFor',
        '⚡ Implement virtual scrolling for large lists',
        '⚡ Use async pipe instead of manual subscriptions'
    ];
    
    tips.forEach(tip => console.log(`  ${tip}`));
    
    console.log('\n📊 Bundle Analysis Commands:');
    console.log('  ng build --stats-json');
    console.log('  npx webpack-bundle-analyzer dist/clean-architecture.ui/stats.json');
    
    console.log('\n🔧 Suggested next steps:');
    console.log('  1. Run: npm run build to test optimizations');
    console.log('  2. Analyze bundle with webpack-bundle-analyzer');
    console.log('  3. Implement lazy loading for major feature modules');
    console.log('  4. Refactor large SCSS files using shared mixins');
}

// Run analysis
analyzeDependencies();
analyzeStyles();
provideBuildTips();

console.log('\n✨ Optimization analysis complete!');
console.log('Run this script again after implementing changes to track progress.\n'); 