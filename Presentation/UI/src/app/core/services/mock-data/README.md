# Centralized Mock Data Management System

## Overview

This centralized mock data management system provides consistent, unified mock data across all frontend components, replacing the previous scattered and inconsistent mock data implementations.

## Architecture

### Core Services

1. **MockDataService** (`mock-data.service.ts`)
   - Central repository for all mock data
   - Provides unified data for both Store Manager and Explosive Manager perspectives
   - Handles data consistency and standardization

2. **MockDataMapperService** (`mock-data-mapper.service.ts`)
   - Handles transformation between different component data models
   - Maps between StockRequest and ExplosiveRequest formats
   - Ensures data consistency across different views

3. **MockDataValidatorService** (`mock-data-validator.service.ts`)
   - Validates data consistency and accuracy
   - Generates detailed consistency reports
   - Provides validation methods for data integrity

## Key Features

### Data Consistency
- **Unified User Data**: Centralized store managers and explosive managers
- **Consistent Store Information**: Standardized store locations and details
- **Synchronized Request Items**: Common explosive types and specifications
- **Status Mapping**: Automatic mapping between different status enums

### Type Safety
- **ExplosiveType Mapping**: Handles differences between store model and explosive manager model enums
- **Automatic Conversion**: Seamless conversion between data formats
- **Validation**: Built-in validation for data consistency

### Maintainability
- **Single Source of Truth**: All mock data in one location
- **Easy Updates**: Modify data in one place, reflects everywhere
- **Extensible**: Easy to add new data types and formats

## Usage

### Store Manager Components
```typescript
// Inject the service
constructor(private stockRequestService: StockRequestService) {}

// Service automatically uses centralized mock data
this.stockRequestService.getStockRequests().subscribe(requests => {
  // Handle unified stock request data
});
```

### Explosive Manager Components
```typescript
// Inject the service
constructor(private mockDataService: MockDataService) {}

// Initialize with centralized data
this.mockDataService.getExplosiveRequestMockData().subscribe(data => {
  // Handle unified explosive request data
});
```

### Data Validation
```typescript
// Inject validator service
constructor(private validator: MockDataValidatorService) {}

// Validate all mock data
this.validator.validateAllMockData().subscribe(result => {
  if (!result.isValid) {
    console.error('Data validation errors:', result.errors);
  }
});

// Generate consistency report
this.validator.generateConsistencyReport().subscribe(report => {
  // Handle detailed consistency analysis
});
```

## Data Structure

### Request Data
- **ID Consistency**: Same request IDs across both formats
- **User Information**: Consistent requester names and roles
- **Date Synchronization**: Matching request and required dates
- **Status Mapping**: Automatic status conversion between formats

### Explosive Types
The system handles two different ExplosiveType enums:
- **Store Model**: `'ANFO'`, `'Emulsion'`, etc.
- **Explosive Manager Model**: `'ANFO'`, `'EMULSION'`, etc.

Automatic mapping ensures compatibility between both formats.

## Migration Benefits

### Before (Scattered Mock Data)
- ❌ Duplicate data in multiple files
- ❌ Inconsistent user names and IDs
- ❌ Different date formats and values
- ❌ Mismatched status values
- ❌ Difficult to maintain and update

### After (Centralized System)
- ✅ Single source of truth
- ✅ Consistent data across all components
- ✅ Automatic validation and error detection
- ✅ Easy maintenance and updates
- ✅ Type-safe data transformations

## Validation Features

### Automatic Checks
- **ID Consistency**: Ensures matching request IDs
- **Data Integrity**: Validates field consistency
- **User Synchronization**: Checks user data across formats
- **Date Validation**: Ensures date consistency
- **Status Mapping**: Validates status conversions

### Reporting
- **Validation Results**: Detailed error and warning reports
- **Consistency Reports**: Per-request consistency analysis
- **Summary Statistics**: Overview of data health

## Future Enhancements

1. **Real API Integration**: Easy transition from mock to real data
2. **Dynamic Data Generation**: Configurable mock data generation
3. **Advanced Validation**: More sophisticated validation rules
4. **Performance Optimization**: Caching and lazy loading
5. **Testing Utilities**: Enhanced testing support

## Best Practices

1. **Always use the centralized services** instead of inline mock data
2. **Validate data consistency** regularly during development
3. **Use the mapper service** for data transformations
4. **Check validation reports** when adding new data
5. **Update documentation** when extending the system

## Troubleshooting

### Common Issues

1. **Type Mismatch Errors**
   - Ensure proper ExplosiveType mapping
   - Use the mapper service for conversions

2. **Data Inconsistency**
   - Run validation service to identify issues
   - Check consistency reports for details

3. **Missing Data**
   - Verify service injection
   - Check observable subscriptions

### Debug Tools

Use the validation service to diagnose issues:
```typescript
// Check overall data health
this.validator.validateAllMockData().subscribe(console.log);

// Get detailed consistency report
this.validator.generateConsistencyReport().subscribe(console.log);

// Validate service functionality
this.validator.validateMockDataService().subscribe(console.log);
```