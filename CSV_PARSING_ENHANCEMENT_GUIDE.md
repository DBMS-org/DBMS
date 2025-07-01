# CSV Drill Plan Parsing & Visualization Enhancement Guide

## 🎯 Overview

This enhancement provides a comprehensive CSV drill plan parsing system that supports:

1. **Multiple CSV formats** with varying column names
2. **Automatic 2D/3D detection** and fallback visualization
3. **Flexible column mapping** for different mining industry standards
4. **Safe data conversion** with robust error handling
5. **Enhanced visualization** that adapts based on available data

## 📋 Features Implemented

### ✅ 1. Flexible Column Mapping System

The system now supports various column name formats commonly used in the mining industry:

#### Supported Column Variations:

| Standard Field | Accepted Column Names |
|---|---|
| **ID** | `id`, `hole id`, `holeid`, `hole_id`, `holeidentifier`, `hole identifier`, `sr no`, `sr no.`, `serial no`, `serial no.`, `serial number`, `serialnumber` |
| **Easting** | `easting`, `east`, `x`, `x coord`, `x coordinate`, `x_coord`, `x_coordinate`, `design collar (e)`, `design collar e`, `designcollare`, `collar east`, `collar easting`, `collareasting`, `collar_east`, `collar_easting` |
| **Northing** | `northing`, `north`, `y`, `y coord`, `y coordinate`, `y_coord`, `y_coordinate`, `design collar (n)`, `design collar n`, `designcollarn`, `collar north`, `collar northing`, `collarnorthing`, `collar_north`, `collar_northing` |
| **Elevation** | `elevation`, `elev`, `z`, `z coord`, `z coordinate`, `z_coord`, `z_coordinate`, `design collar (rl)`, `design collar rl`, `designcollarrl`, `collar elevation`, `collarelevation`, `collar_elevation`, `rl`, `reduced level` |
| **Length** | `length`, `hole length`, `holelength`, `hole_length`, `total length`, `totallength`, `total_length`, `planned length`, `plannedlength`, `planned_length` |
| **Depth** | `depth`, `hole depth`, `holedepth`, `hole_depth`, `actual depth`, `actualdepth`, `actual_depth`, `final depth`, `finaldepth`, `final_depth`, `actual dep`, `actualdep`, `actual_dep` |
| **Azimuth** | `azimuth`, `azi`, `bearing`, `direction`, `angle`, `orientation`, `az`, `azimuth angle`, `azimuthangle`, `azimuth_angle` |
| **Dip** | `dip`, `inclination`, `inc`, `angle`, `slope`, `gradient`, `dip angle`, `dipangle`, `dip_angle`, `inclination angle`, `inclinationangle`, `inclination_angle` |
| **Stemming** | `stemming`, `stem`, `stemming length`, `stemminglength`, `stemming_length`, `stem length`, `stemlength`, `stem_length` |

### ✅ 2. 2D/3D Automatic Detection

The system automatically detects whether the CSV data supports:

- **3D Mode**: When azimuth and dip data are available
- **2D Mode**: When only coordinate data is available (automatic fallback)

#### Detection Logic:
- **Full 3D**: ≥50% of holes have both azimuth and dip data
- **2D Fallback**: <50% of holes have complete 3D data
- **Visual Indicators**: Clear mode display in the UI

### ✅ 3. Safe Data Conversion

#### Multi-Culture Number Parsing:
- Supports European format (comma as decimal separator)
- Handles various thousands separators
- Graceful fallback for malformed data

#### Data Validation:
- **Coordinates**: Reasonable range validation
- **Drilling Parameters**: Logical bounds checking
- **3D Data**: Optional validation with fallback to null

### ✅ 4. Enhanced Database Schema

The `DrillHole` entity now supports:
```csharp
public double? Azimuth { get; set; }  // Nullable for 2D fallback
public double? Dip { get; set; }      // Nullable for 2D fallback

// Helper properties
public bool Has3DData => Azimuth.HasValue && Dip.HasValue;
public bool RequiresFallbackTo2D => !Has3DData;
```

### ✅ 5. Enhanced Visualization

#### 3D Mode Features:
- Full orientation support with azimuth and dip
- Proper drill hole visualization with realistic angles
- Color-coded hole identification

#### 2D Mode Features:
- Vertical hole display for simplicity
- Plan view emphasis
- Clear mode indicators

## 📄 Supported CSV Formats

### Example 1: Full 3D Format (Mining Standard)
```csv
Sr No.,ID,East,North,Elev,Length,Azi,Dip,Actual Dep,Stemming,Total Explosive Mass
1,A3,442493.7,2684895,239.8,6.8,85,15,6.6,2.5,19
2,A4,442493.4,2684897,239.9,7,85,15,7,2.5,19
```

### Example 2: 2D Only Format (Coordinate Focus)
```csv
Hole Identifier,Design Collar (E),Design Collar (N),Design Collar (RL),Hole Length,Actual Depth,Stemming
C19,442476.803,2684873.302,257.415,3.715,6.215,1.5
C20,442475.588,2684784.459,258.388,4.688,7.188,1.5
```

### Example 3: Mixed Format (Partial 3D Data)
```csv
Hole ID,Easting,Northing,Elevation,Depth,Azimuth,Inclination,Stemming
F1,442520.1,2684820.5,258.2,6.5,90,15,2.0
F2,442522.3,2684822.1,258.3,6.8,,15,2.0
F3,442524.5,2684823.7,258.4,7.1,90,,2.0
```

## 🚀 Usage Instructions

### 1. Upload CSV File
1. Navigate to the CSV Upload section
2. Select any CSV file with drill hole data
3. The system automatically detects the format and column mapping

### 2. Automatic Processing
- **Header Detection**: Automatically finds header row
- **Column Mapping**: Maps columns to standard fields
- **Data Validation**: Validates and converts data safely
- **3D Detection**: Determines visualization mode

### 3. Visualization
- **3D Mode**: Full spatial visualization with drill orientations
- **2D Mode**: Plan view with vertical holes for clarity
- **Mode Indicator**: Clear visual feedback on current mode

## 🛠️ Technical Implementation

### Core Components:

1. **CsvColumnMapper**: Handles flexible column mapping
2. **SafeDataConverter**: Provides robust data conversion
3. **Enhanced DrillHole Model**: Supports nullable 3D data
4. **Auto-detection Logic**: Determines visualization capabilities
5. **Adaptive Visualization**: Renders based on available data

### Key Methods:

```csharp
// Column mapping
var mappedHeaders = CsvColumnMapper.MapHeaders(headers);
var (isValid, missingColumns) = CsvColumnMapper.ValidateRequiredColumns(mappedHeaders);
var has3DCapability = CsvColumnMapper.Has3DCapability(mappedHeaders);

// Safe conversion
var azimuth = SafeDataConverter.ParseDoubleOrNull(value, "Azimuth", lineNumber);
var coordinates = SafeDataConverter.ValidateCoordinates(easting, northing, elevation);

// 3D detection
private detect3DCapability(): void {
  const holesWithBoth3DParams = this.drillData.filter(hole => 
    hole.azimuth !== null && hole.dip !== null && 
    !isNaN(hole.azimuth) && !isNaN(hole.dip));
  this.visualization3DCapable = holesWithBoth3DParams.length >= this.drillData.length * 0.5;
}
```

## 📊 Benefits

### For Users:
- **Simplified Workflow**: Upload any format CSV without preprocessing
- **Clear Feedback**: Visual indicators for data quality and capabilities
- **Flexible Visualization**: Automatic adaptation to available data
- **Robust Processing**: Handles malformed or incomplete data gracefully

### For Developers:
- **Maintainable Code**: Clear separation of concerns
- **Extensible Design**: Easy to add new column formats
- **Error Handling**: Comprehensive validation and fallbacks
- **Performance**: Efficient processing with minimal memory usage

## 🔧 Configuration Options

### Column Mapping Extension:
To add new column variations, update the `ColumnMappings` dictionary in `CsvColumnMapper.cs`:

```csharp
["azimuth"] = new List<string> 
{ 
    "azimuth", "azi", "bearing", "direction", 
    "new_format_name", "custom_azimuth_column"  // Add new formats here
}
```

### Validation Rules:
Adjust validation ranges in `SafeDataConverter.cs`:

```csharp
"azimuth" => value >= 0 && value <= 360,     // Configurable range
"dip" => value >= -90 && value <= 90,        // Configurable range
```

## 🚨 Error Handling

### Graceful Degradation:
1. **Missing Columns**: Uses available data, skips missing optional fields
2. **Invalid Data**: Logs warnings, uses safe defaults or null values
3. **Parse Errors**: Provides detailed error messages with line numbers
4. **3D Fallback**: Automatically switches to 2D mode when 3D data is insufficient

### Error Messages:
- Clear indication of what data is missing or invalid
- Line-by-line error reporting for large files
- Suggested solutions for common format issues

## 📈 Performance Considerations

### Optimizations:
- **Streaming Processing**: Processes large files without loading entirely into memory
- **Lazy Evaluation**: Only processes necessary columns
- **Efficient Validation**: Early exit for invalid data
- **Memory Management**: Proper disposal of resources

### Scalability:
- Handles files with thousands of drill holes
- Minimal memory footprint
- Fast column detection and mapping
- Optimized 3D rendering for large datasets

## 🔮 Future Enhancements

### Planned Features:
1. **Custom Column Mapping UI**: Allow users to manually map columns
2. **Multiple File Format Support**: Excel, XML, JSON support
3. **Data Export**: Export processed data in various formats
4. **Advanced 3D Features**: Animation, cross-sections, geological layers
5. **Batch Processing**: Handle multiple files simultaneously

This enhancement significantly improves the usability and robustness of the drill plan visualization system, making it suitable for various mining industry workflows and data formats. 