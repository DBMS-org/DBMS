# Tailwind CSS + PrimeNG Migration Plan
## Angular Material → Tailwind CSS + PrimeNG

**Document Version:** 1.0
**Created:** October 13, 2025
**Project:** DBMS Blasting Management System
**Current Stack:** Angular 19 + Angular Material
**Target Stack:** Angular 19 + Tailwind CSS + PrimeNG

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Why Migrate?](#why-migrate)
3. [Technology Stack Comparison](#technology-stack-comparison)
4. [Migration Strategy](#migration-strategy)
5. [Phase 0: Preparation & Setup](#phase-0-preparation--setup)
6. [Phase 1: Shared Components](#phase-1-shared-components)
7. [Phase 2: Authentication Module](#phase-2-authentication-module)
8. [Phase 3: Store Manager Module](#phase-3-store-manager-module)
9. [Phase 4: Explosive Manager Module](#phase-4-explosive-manager-module)
10. [Phase 5: Operator Module](#phase-5-operator-module)
11. [Phase 6: Blasting Engineer Module](#phase-6-blasting-engineer-module)
12. [Phase 7: Mechanical Engineer Module](#phase-7-mechanical-engineer-module)
13. [Phase 8: Machine Manager Module](#phase-8-machine-manager-module)
14. [Phase 9: Admin Module](#phase-9-admin-module)
15. [Phase 10: Cleanup & Optimization](#phase-10-cleanup--optimization)
16. [Component Mapping Reference](#component-mapping-reference)
17. [Risk Assessment & Mitigation](#risk-assessment--mitigation)
18. [Testing Strategy](#testing-strategy)
19. [Rollback Plan](#rollback-plan)
20. [Timeline & Effort Estimates](#timeline--effort-estimates)

---

## Executive Summary

### Overview
This document outlines a **phased, incremental migration** from Angular Material to Tailwind CSS + PrimeNG for the DBMS Blasting Management System. The migration will be done module-by-module to minimize risk and maintain application stability throughout the process.

### Key Benefits
- **Better Design Flexibility**: Tailwind's utility-first approach offers more control
- **Rich Component Library**: PrimeNG provides 90+ enterprise-grade components
- **Smaller Bundle Size**: More efficient tree-shaking with Tailwind
- **Better Performance**: Optimized CSS and component rendering
- **Modern UI/UX**: More contemporary design patterns
- **Consistency**: Your existing design system maps perfectly to Tailwind config
- **Free & Open Source**: PrimeNG is completely free (MIT license)

### Migration Approach
- **Incremental**: One module at a time
- **Non-Breaking**: Both systems coexist during migration
- **Testable**: Full testing after each phase
- **Reversible**: Can rollback any phase if needed

### Total Estimated Time
- **Minimum**: 18-22 hours
- **Maximum**: 28-35 hours
- **Sessions**: 10-15 working sessions of 2-3 hours each

---

## Why Migrate?

### Current Pain Points with Angular Material
1. **Limited Customization**: Material Design has opinionated styling that's hard to override
2. **Bundle Size**: Angular Material + CDK is relatively heavy
3. **Component Gaps**: Missing some enterprise components (advanced tables, timeline, etc.)
4. **Design Constraints**: Locked into Material Design patterns

### Benefits of Tailwind CSS + PrimeNG

#### Tailwind CSS Benefits
✅ **Utility-First**: Rapid UI development with utility classes
✅ **Highly Customizable**: Your design system variables map directly to config
✅ **Smaller Bundle**: Only includes CSS you actually use
✅ **Responsive**: Built-in responsive utilities
✅ **Modern**: Industry standard for modern web development
✅ **No Context Switching**: Style directly in templates

#### PrimeNG Benefits
✅ **Native Angular**: Built specifically for Angular (not a React port)
✅ **90+ Components**: Comprehensive enterprise component library
✅ **Tailwind Support**: Official Tailwind CSS theme available
✅ **Free Forever**: MIT license, no premium tier required
✅ **Active Development**: Regular updates and maintenance
✅ **Great Documentation**: Extensive examples and API docs
✅ **Accessibility**: WCAG compliant components
✅ **Advanced Components**: DataTable, TreeTable, Charts, FileUpload, etc.

---

## Technology Stack Comparison

### Current Stack
```typescript
// package.json (Current)
{
  "@angular/material": "^19.2.5",
  "@angular/cdk": "^19.2.5",
  "material-icons": "^1.13.14"
}
```

**Components Used:**
- MatDialog / MatDialogModule
- MatSnackBar / MatSnackBarModule
- MatButton / MatButtonModule
- MatCard / MatCardModule
- MatTable / MatTableDataSource
- MatFormField / MatInput
- MatSelect / MatOption
- MatDatepicker
- MatIcon

**Current Bundle Impact:**
- Angular Material: ~500KB
- Material Icons: ~50KB
- Custom SCSS overrides: ~30KB

### Target Stack
```json
// package.json (Target)
{
  "tailwindcss": "^3.4.0",
  "primeng": "^17.18.0",
  "primeicons": "^7.0.0"
}
```

**Components Available:**
- All PrimeNG components (90+)
- Tailwind utility classes
- Your custom design system preserved

**Expected Bundle Impact:**
- Tailwind CSS (optimized): ~10-50KB
- PrimeNG (tree-shaken): ~200-300KB
- Total reduction: ~40-50% smaller

---

## Migration Strategy

### Core Principles

1. **Incremental Migration**: Migrate one module at a time
2. **Coexistence**: Angular Material and PrimeNG work side-by-side during migration
3. **Testing First**: Test each phase before moving to next
4. **Preserve Functionality**: No feature loss during migration
5. **Design System Intact**: Keep your CSS variables and design tokens
6. **Rollback Ready**: Can revert any phase if issues arise

### Migration Order (Smallest to Largest)

The migration will proceed from **simplest to most complex** modules:

1. **Shared Components** (Foundation - used everywhere)
2. **Authentication** (Small, isolated)
3. **Store Manager** (~12 components)
4. **Explosive Manager** (~15 components)
5. **Operator** (~15 components)
6. **Blasting Engineer** (~20 components)
7. **Mechanical Engineer** (~25 components)
8. **Machine Manager** (~20 components)
9. **Admin** (~25 components - most complex)

### Why This Order?

- **Shared Components First**: Sets foundation for all other modules
- **Start Small**: Auth module is small and isolated
- **Build Confidence**: Early wins with smaller modules
- **Learn Patterns**: Establish migration patterns early
- **Complex Last**: Tackle admin module when most experienced

---

## Phase 0: Preparation & Setup

**Goal**: Install and configure Tailwind CSS + PrimeNG without breaking existing functionality

**Duration**: 30-45 minutes

### Tasks

#### 0.1 Install Dependencies

```bash
# Install Tailwind CSS
npm install -D tailwindcss postcss autoprefixer

# Initialize Tailwind config
npx tailwindcss init

# Install PrimeNG
npm install primeng primeicons

# Install PrimeFlex (optional - grid system)
npm install primeflex
```

#### 0.2 Configure Tailwind

**Create/Update: `tailwind.config.js`**

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#667eea',
          dark: '#764ba2',
        },
        secondary: '#4a90e2',
        accent: '#5a67d8',
        success: '#10b981',
        warning: '#f59e0b',
        error: '#ef4444',
        info: '#3b82f6',
      },
      fontFamily: {
        sans: ['-apple-system', 'BlinkMacSystemFont', 'Segoe UI', 'Roboto', 'Helvetica', 'Arial', 'sans-serif'],
      },
      borderRadius: {
        'sm': '6px',
        'md': '8px',
        'lg': '12px',
        'xl': '16px',
      },
      boxShadow: {
        'sm': '0 2px 4px rgba(0, 0, 0, 0.05)',
        'md': '0 4px 15px rgba(0, 0, 0, 0.08)',
        'lg': '0 8px 30px rgba(102, 126, 234, 0.15)',
        'xl': '0 20px 40px rgba(0, 0, 0, 0.1)',
      },
    },
  },
  plugins: [],
}
```

#### 0.3 Update Angular Configuration

**Update: `angular.json`**

```json
{
  "styles": [
    "src/styles.scss",
    "node_modules/primeng/resources/themes/lara-light-blue/theme.css",
    "node_modules/primeng/resources/primeng.min.css",
    "node_modules/primeicons/primeicons.css"
  ]
}
```

#### 0.4 Update Global Styles

**Update: `src/styles.scss`**

Add at the top:
```scss
// Tailwind imports
@tailwind base;
@tailwind components;
@tailwind utilities;

// Your existing styles below...
```

#### 0.5 Create PrimeNG Theme Configuration

**Create: `src/primeng-theme.scss`** (Optional - for customization)

```scss
// Override PrimeNG theme variables to match your design system
:root {
  --primary-color: #667eea;
  --primary-color-text: #ffffff;
  // Add more customizations as needed
}
```

#### 0.6 Test the Setup

```bash
# Build the application
npm run build

# Serve the application
npm start
```

**Expected Result**: Application runs normally with no visual changes yet.

### Checklist

- [ ] Tailwind CSS installed
- [ ] PrimeNG installed
- [ ] tailwind.config.js configured with design system
- [ ] angular.json updated with PrimeNG styles
- [ ] styles.scss updated with Tailwind directives
- [ ] Application builds successfully
- [ ] Application runs without errors
- [ ] No visual changes to existing UI

### Verification

Run these commands to verify:
```bash
# Check if Tailwind is working
# Add a test class in any template: class="bg-primary text-white p-4"
# Should see styled element

# Check if PrimeNG is available
# Should be able to import: import { ButtonModule } from 'primeng/button';
```

---

## Phase 1: Shared Components

**Goal**: Migrate shared/common components used across all modules

**Duration**: 2-3 hours

**Components to Migrate**: 3-5 shared components

### Priority Components

#### 1.1 Confirm Dialog Component

**Current**: `shared/components/confirm-dialog` (uses MatDialog)

**Migration Steps**:

1. Create new PrimeNG version
2. Update dialog service calls
3. Replace MatDialog with PrimeNG Dialog service

**File**: `confirm-dialog.component.ts`

**Before (Material)**:
```typescript
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data.title }}</h2>
    <mat-dialog-content>...</mat-dialog-content>
    <mat-dialog-actions>
      <button mat-button>Cancel</button>
      <button mat-raised-button color="primary">OK</button>
    </mat-dialog-actions>
  `
})
```

**After (PrimeNG + Tailwind)**:
```typescript
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';

@Component({
  imports: [DialogModule, ButtonModule],
  template: `
    <p-dialog [header]="data.title || 'Confirm'" [(visible)]="display">
      <p class="text-gray-700">{{ data.message }}</p>
      <ng-template pTemplate="footer">
        <div class="flex gap-2 justify-end">
          <p-button
            label="{{ data.cancelText || 'Cancel' }}"
            icon="pi pi-times"
            (onClick)="onCancel()"
            styleClass="p-button-text">
          </p-button>
          <p-button
            label="{{ data.confirmText || 'OK' }}"
            icon="pi pi-check"
            (onClick)="onConfirm()">
          </p-button>
        </div>
      </ng-template>
    </p-dialog>
  `
})
```

#### 1.2 Logout Confirmation Dialog

**Current**: `shared/components/logout-confirmation-dialog`

**Migration**: Similar to confirm dialog

#### 1.3 Notification Service

**Current**: `core/services/notification.service.ts` (uses MatSnackBar)

**Before**:
```typescript
import { MatSnackBar } from '@angular/material/snack-bar';

constructor(private snackBar: MatSnackBar) {}

showSuccess(message: string) {
  this.snackBar.open(message, 'Close', {
    duration: 3000,
    panelClass: ['success-snackbar']
  });
}
```

**After (PrimeNG Toast)**:
```typescript
import { MessageService } from 'primeng/api';

constructor(private messageService: MessageService) {}

showSuccess(message: string) {
  this.messageService.add({
    severity: 'success',
    summary: 'Success',
    detail: message,
    life: 3000
  });
}
```

**Add to app.config.ts**:
```typescript
import { MessageService } from 'primeng/api';

providers: [
  MessageService,
  // ... other providers
]
```

**Add to root template (app.component.html)**:
```html
<p-toast></p-toast>
<router-outlet></router-outlet>
```

#### 1.4 Common Button Styles

Create reusable button classes in your global styles:

```scss
// Add to styles.scss
@layer components {
  .btn-primary {
    @apply bg-gradient-to-r from-primary to-primary-dark text-white
           px-6 py-3 rounded-md font-medium transition-all
           hover:shadow-lg hover:-translate-y-0.5;
  }

  .btn-secondary {
    @apply bg-gray-600 text-white px-6 py-3 rounded-md font-medium
           hover:bg-gray-700 transition-all;
  }

  .btn-outline {
    @apply border-2 border-primary text-primary px-6 py-3 rounded-md
           hover:bg-primary hover:text-white transition-all;
  }
}
```

### Migration Checklist

- [ ] Confirm dialog migrated to PrimeNG
- [ ] Logout dialog migrated
- [ ] Notification service migrated to Toast
- [ ] Toast component added to root
- [ ] MessageService provider added
- [ ] Common button utility classes created
- [ ] All shared components tested
- [ ] No console errors
- [ ] Visual consistency maintained

### Testing

Test shared components:
```typescript
// Test notification service
notificationService.showSuccess('Test message');
notificationService.showError('Error message');
notificationService.showWarning('Warning message');

// Test confirm dialog
dialogService.confirm({
  message: 'Are you sure?',
  accept: () => console.log('Accepted'),
  reject: () => console.log('Rejected')
});
```

---

## Phase 2: Authentication Module

**Goal**: Migrate authentication components (login, forgot password, reset password)

**Duration**: 1-2 hours

**Components**: 4 components (small, isolated)

### Components List

1. Login Component
2. Forgot Password Component
3. Verify Reset Code Component
4. Reset Password Component

### Migration Steps

#### 2.1 Login Component

**Location**: `components/auth/login/login.component.ts`

**Material Components Used**:
- Form fields (MatFormField, MatInput)
- Buttons (MatButton)
- Cards (MatCard)

**PrimeNG Replacements**:

```html
<!-- Before (Material) -->
<mat-card>
  <mat-card-content>
    <mat-form-field>
      <mat-label>Email</mat-label>
      <input matInput type="email" [(ngModel)]="email">
    </mat-form-field>
    <button mat-raised-button color="primary">Login</button>
  </mat-card-content>
</mat-card>

<!-- After (PrimeNG + Tailwind) -->
<div class="bg-white rounded-lg shadow-lg p-8">
  <div class="mb-6">
    <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
    <input
      type="email"
      pInputText
      [(ngModel)]="email"
      class="w-full"
      placeholder="Enter your email">
  </div>
  <p-button
    label="Login"
    icon="pi pi-sign-in"
    styleClass="w-full"
    (onClick)="login()">
  </p-button>
</div>
```

**Import Changes**:
```typescript
// Remove
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

// Add
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { PasswordModule } from 'primeng/password';
```

#### 2.2 Forgot Password Component

Similar migration pattern:
- Replace MatFormField with PrimeNG InputText
- Replace Material buttons with PrimeNG buttons
- Use Tailwind for layout and styling

#### 2.3 Form Validation Messages

**Before**:
```html
<mat-error *ngIf="emailControl.hasError('required')">
  Email is required
</mat-error>
```

**After**:
```html
<small
  *ngIf="emailControl.hasError('required')"
  class="text-red-500 block mt-1">
  Email is required
</small>
```

### Checklist

- [ ] Login component migrated
- [ ] Forgot password migrated
- [ ] Verify reset code migrated
- [ ] Reset password migrated
- [ ] Form validation working
- [ ] Error messages displayed correctly
- [ ] Success notifications working
- [ ] Navigation after login works
- [ ] Responsive design maintained
- [ ] All auth flows tested

---

## Phase 3: Store Manager Module

**Goal**: Migrate Store Manager module (~12 components)

**Duration**: 2-3 hours

**Complexity**: Low-Medium

### Components to Migrate

1. Store Manager Dashboard
2. Store Manager Layout (Sidebar, Navbar)
3. Inventory Components
4. Request History
5. Dispatch Info
6. Add Stock Component
7. Stores Component

### Key Migration Areas

#### 3.1 Dashboard Component

**Tables**: Replace MatTable with PrimeNG DataTable

**Before**:
```html
<table mat-table [dataSource]="dataSource">
  <ng-container matColumnDef="name">
    <th mat-header-cell *matHeaderCellDef>Name</th>
    <td mat-cell *matCellDef="let row">{{row.name}}</td>
  </ng-container>
</table>
```

**After**:
```html
<p-table [value]="items" [paginator]="true" [rows]="10">
  <ng-template pTemplate="header">
    <tr>
      <th>Name</th>
    </tr>
  </ng-template>
  <ng-template pTemplate="body" let-item>
    <tr>
      <td>{{item.name}}</td>
    </tr>
  </ng-template>
</p-table>
```

**Benefits**:
- Built-in pagination
- Built-in sorting
- Built-in filtering
- Built-in row selection
- Export to CSV/Excel
- Responsive design

#### 3.2 Sidebar Navigation

**Before (Material)**:
```html
<mat-nav-list>
  <a mat-list-item routerLink="/store-manager/dashboard">
    <mat-icon>dashboard</mat-icon>
    Dashboard
  </a>
</mat-nav-list>
```

**After (PrimeNG + Tailwind)**:
```html
<p-menu [model]="menuItems"></p-menu>

<!-- Or custom with Tailwind -->
<nav class="space-y-2">
  <a
    routerLink="/store-manager/dashboard"
    routerLinkActive="bg-primary text-white"
    class="flex items-center gap-3 px-4 py-3 rounded-md hover:bg-gray-100 transition-colors">
    <i class="pi pi-home"></i>
    <span>Dashboard</span>
  </a>
</nav>
```

#### 3.3 Forms (Add Stock, etc.)

Replace Material form fields with PrimeNG:

**Components**:
- InputText (text inputs)
- InputNumber (number inputs)
- Dropdown (selects)
- Calendar (date pickers)
- InputTextarea (textareas)

**Example**:
```html
<div class="grid grid-cols-2 gap-4">
  <div class="flex flex-col gap-2">
    <label>Item Name</label>
    <input pInputText [(ngModel)]="itemName" />
  </div>

  <div class="flex flex-col gap-2">
    <label>Quantity</label>
    <p-inputNumber [(ngModel)]="quantity" [min]="0"></p-inputNumber>
  </div>

  <div class="flex flex-col gap-2">
    <label>Category</label>
    <p-dropdown
      [options]="categories"
      [(ngModel)]="selectedCategory"
      optionLabel="name">
    </p-dropdown>
  </div>

  <div class="flex flex-col gap-2">
    <label>Expiry Date</label>
    <p-calendar [(ngModel)]="expiryDate"></p-calendar>
  </div>
</div>
```

### Checklist

- [ ] Dashboard component migrated
- [ ] Sidebar navigation migrated
- [ ] Navbar migrated
- [ ] Tables converted to p-table
- [ ] Forms migrated
- [ ] Add stock component working
- [ ] Request history working
- [ ] Dispatch info working
- [ ] All CRUD operations tested
- [ ] Responsive design verified

---

## Phase 4: Explosive Manager Module

**Goal**: Migrate Explosive Manager module (~15 components)

**Duration**: 2-3 hours

**Complexity**: Medium

### Components to Migrate

1. Explosive Manager Dashboard
2. Inventory Management
3. Requests Component
4. Approval Form
5. Dispatch Request
6. Dispatch Modal
7. Stores Statistics

### Key Features

#### 4.1 Requests Management

**Complex Tables**: PrimeNG DataTable with advanced features

```html
<p-table
  [value]="requests"
  [paginator]="true"
  [rows]="10"
  [globalFilterFields]="['requestId', 'siteName', 'status']"
  [rowHover]="true"
  dataKey="id">

  <ng-template pTemplate="caption">
    <div class="flex justify-between">
      <span class="p-input-icon-left">
        <i class="pi pi-search"></i>
        <input
          pInputText
          type="text"
          (input)="dt.filterGlobal($event.target.value, 'contains')"
          placeholder="Search requests..." />
      </span>
      <p-button label="Export" icon="pi pi-download"></p-button>
    </div>
  </ng-template>

  <ng-template pTemplate="header">
    <tr>
      <th pSortableColumn="requestId">Request ID <p-sortIcon field="requestId"></p-sortIcon></th>
      <th pSortableColumn="siteName">Site <p-sortIcon field="siteName"></p-sortIcon></th>
      <th pSortableColumn="status">Status <p-sortIcon field="status"></p-sortIcon></th>
      <th>Actions</th>
    </tr>
  </ng-template>

  <ng-template pTemplate="body" let-request>
    <tr>
      <td>{{request.requestId}}</td>
      <td>{{request.siteName}}</td>
      <td>
        <span [class]="getStatusClass(request.status)">
          {{request.status}}
        </span>
      </td>
      <td>
        <p-button
          icon="pi pi-eye"
          styleClass="p-button-rounded p-button-text"
          (onClick)="viewDetails(request)">
        </p-button>
      </td>
    </tr>
  </ng-template>
</p-table>
```

#### 4.2 Modals/Dialogs

**Before (MatDialog)**:
```typescript
const dialogRef = this.dialog.open(DispatchModalComponent, {
  width: '600px',
  data: request
});

dialogRef.afterClosed().subscribe(result => {
  if (result) {
    // Handle result
  }
});
```

**After (PrimeNG Dialog)**:
```typescript
// In component
showDispatchDialog = false;
selectedRequest: any;

openDispatchDialog(request: any) {
  this.selectedRequest = request;
  this.showDispatchDialog = true;
}

// In template
<p-dialog
  [(visible)]="showDispatchDialog"
  [header]="'Dispatch Request'"
  [modal]="true"
  [style]="{width: '600px'}">

  <app-dispatch-form
    [request]="selectedRequest"
    (onSubmit)="handleDispatch($event)">
  </app-dispatch-form>

  <ng-template pTemplate="footer">
    <p-button label="Cancel" (onClick)="showDispatchDialog=false"></p-button>
    <p-button label="Dispatch" (onClick)="confirmDispatch()"></p-button>
  </ng-template>
</p-dialog>
```

#### 4.3 Statistics Cards

Use Tailwind for beautiful stat cards:

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
  <!-- Total Requests Card -->
  <div class="bg-white rounded-lg shadow-md p-6 border-l-4 border-primary">
    <div class="flex items-center justify-between">
      <div>
        <p class="text-sm text-gray-600 font-medium">Total Requests</p>
        <p class="text-3xl font-bold text-gray-800 mt-2">{{totalRequests}}</p>
      </div>
      <div class="bg-primary/10 p-4 rounded-full">
        <i class="pi pi-inbox text-2xl text-primary"></i>
      </div>
    </div>
    <div class="mt-4 flex items-center text-sm">
      <span class="text-green-600 font-medium">↑ 12%</span>
      <span class="text-gray-600 ml-2">from last month</span>
    </div>
  </div>

  <!-- More cards... -->
</div>
```

### Checklist

- [ ] Dashboard migrated
- [ ] Inventory management migrated
- [ ] Requests table migrated
- [ ] Approval form migrated
- [ ] Dispatch modal migrated
- [ ] Statistics cards migrated
- [ ] All dialogs working
- [ ] Data filtering working
- [ ] Export functionality working
- [ ] All user flows tested

---

## Phase 5: Operator Module

**Goal**: Migrate Operator module (~15 components)

**Duration**: 2-3 hours

**Complexity**: Medium

### Components to Migrate

1. Operator Dashboard
2. My Machines Component
3. Machine Report Dialog
4. Maintenance Reports
5. Pattern View
6. My Project
7. Project Sites

### Key Features

#### 5.1 My Machines - Interactive Cards

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
  <div
    *ngFor="let machine of machines"
    class="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-xl transition-shadow">

    <!-- Machine Image -->
    <div class="h-48 bg-gradient-to-br from-primary to-primary-dark flex items-center justify-center">
      <i class="pi pi-cog text-6xl text-white"></i>
    </div>

    <!-- Machine Info -->
    <div class="p-6">
      <h3 class="text-xl font-bold text-gray-800">{{machine.name}}</h3>
      <p class="text-gray-600 mt-2">{{machine.type}}</p>

      <div class="mt-4 space-y-2">
        <div class="flex justify-between text-sm">
          <span class="text-gray-600">Status:</span>
          <span
            [ngClass]="{
              'text-green-600': machine.status === 'Active',
              'text-red-600': machine.status === 'Maintenance'
            }"
            class="font-medium">
            {{machine.status}}
          </span>
        </div>
        <div class="flex justify-between text-sm">
          <span class="text-gray-600">Last Service:</span>
          <span class="font-medium">{{machine.lastService | date}}</span>
        </div>
      </div>

      <!-- Actions -->
      <div class="mt-6 flex gap-2">
        <p-button
          label="Report Issue"
          icon="pi pi-exclamation-triangle"
          styleClass="p-button-sm p-button-outlined flex-1"
          (onClick)="reportIssue(machine)">
        </p-button>
        <p-button
          icon="pi pi-info-circle"
          styleClass="p-button-sm p-button-text"
          (onClick)="viewDetails(machine)">
        </p-button>
      </div>
    </div>
  </div>
</div>
```

#### 5.2 Maintenance Reports with Timeline

```html
<p-timeline
  [value]="maintenanceReports"
  align="alternate"
  styleClass="customized-timeline">

  <ng-template pTemplate="marker" let-report>
    <span
      [ngClass]="{
        'bg-green-500': report.status === 'Completed',
        'bg-yellow-500': report.status === 'In Progress',
        'bg-red-500': report.status === 'Pending'
      }"
      class="flex w-8 h-8 items-center justify-center text-white rounded-full z-10 shadow-lg">
      <i class="pi pi-wrench"></i>
    </span>
  </ng-template>

  <ng-template pTemplate="content" let-report>
    <p-card [header]="report.title">
      <p class="text-gray-600">{{report.description}}</p>
      <div class="mt-4 flex items-center justify-between">
        <span class="text-sm text-gray-500">{{report.date | date:'medium'}}</span>
        <span
          class="px-3 py-1 rounded-full text-xs font-medium"
          [ngClass]="getStatusBadgeClass(report.status)">
          {{report.status}}
        </span>
      </div>
    </p-card>
  </ng-template>
</p-timeline>
```

#### 5.3 Problem Category Icons (Custom Component)

Enhance with Tailwind:

```html
<div class="flex flex-wrap gap-4">
  <div
    *ngFor="let category of problemCategories"
    class="flex flex-col items-center p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors cursor-pointer"
    (click)="selectCategory(category)">

    <div
      class="w-16 h-16 rounded-full flex items-center justify-center mb-2"
      [style.background]="category.color">
      <i [class]="'pi ' + category.icon + ' text-2xl text-white'"></i>
    </div>

    <span class="text-sm font-medium text-gray-700">{{category.name}}</span>
    <span class="text-xs text-gray-500 mt-1">{{category.count}} issues</span>
  </div>
</div>
```

### Checklist

- [ ] Dashboard migrated
- [ ] My machines migrated
- [ ] Machine cards styled
- [ ] Report dialog migrated
- [ ] Maintenance reports migrated
- [ ] Timeline component working
- [ ] Pattern view migrated
- [ ] Project sites migrated
- [ ] All interactive features working
- [ ] Mobile responsive

---

## Phase 6: Blasting Engineer Module

**Goal**: Migrate Blasting Engineer module (~20 components)

**Duration**: 3-4 hours

**Complexity**: Medium-High

### Components to Migrate

1. Blasting Engineer Dashboard
2. Project Management Components
3. Site Dashboard
4. Blast Sequence Designer
5. Blast Sequence Simulator
6. CSV Upload
7. Explosive Calculations Display
8. Proposal History

### Key Features

#### 6.1 Site Dashboard - Complex Workflow

```html
<!-- Workflow Steps with PrimeNG Steps -->
<p-steps
  [model]="workflowSteps"
  [(activeIndex)]="activeStep"
  [readonly]="false">
</p-steps>

<div class="mt-8">
  <!-- Step Content -->
  <div *ngIf="activeStep === 0" class="bg-white rounded-lg shadow-md p-6">
    <h3 class="text-xl font-bold mb-4">Step 1: Site Information</h3>
    <!-- Step 1 content -->
  </div>

  <div *ngIf="activeStep === 1" class="bg-white rounded-lg shadow-md p-6">
    <h3 class="text-xl font-bold mb-4">Step 2: Pattern Design</h3>
    <!-- Step 2 content -->
  </div>

  <!-- Navigation -->
  <div class="flex justify-between mt-6">
    <p-button
      label="Previous"
      icon="pi pi-chevron-left"
      [disabled]="activeStep === 0"
      (onClick)="previousStep()">
    </p-button>
    <p-button
      label="Next"
      icon="pi pi-chevron-right"
      iconPos="right"
      [disabled]="activeStep === workflowSteps.length - 1"
      (onClick)="nextStep()">
    </p-button>
  </div>
</div>
```

#### 6.2 CSV Upload Component

```html
<p-fileUpload
  name="csvFile[]"
  accept=".csv"
  [maxFileSize]="1000000"
  [customUpload]="true"
  (uploadHandler)="onUpload($event)"
  [auto]="true">

  <ng-template pTemplate="content">
    <div class="text-center py-8">
      <i class="pi pi-cloud-upload text-6xl text-gray-400"></i>
      <p class="mt-4 text-gray-600">Drag and drop CSV file here</p>
      <p class="text-sm text-gray-500 mt-2">or click to browse</p>
    </div>
  </ng-template>
</p-fileUpload>

<!-- Upload progress -->
<p-progressBar
  *ngIf="uploading"
  [value]="uploadProgress"
  class="mt-4">
</p-progressBar>
```

#### 6.3 Explosive Calculations Display

```html
<p-accordion>
  <p-accordionTab
    *ngFor="let calculation of calculations"
    [header]="calculation.name">

    <div class="grid grid-cols-2 gap-4">
      <div
        *ngFor="let param of calculation.parameters"
        class="bg-gray-50 p-4 rounded-lg">
        <label class="text-sm text-gray-600">{{param.label}}</label>
        <p class="text-lg font-bold text-gray-800 mt-1">
          {{param.value}} {{param.unit}}
        </p>
      </div>
    </div>

    <div class="mt-4 flex justify-end gap-2">
      <p-button
        label="Edit"
        icon="pi pi-pencil"
        styleClass="p-button-sm p-button-outlined">
      </p-button>
      <p-button
        label="Duplicate"
        icon="pi pi-copy"
        styleClass="p-button-sm p-button-outlined">
      </p-button>
    </div>
  </p-accordionTab>
</p-accordion>
```

#### 6.4 Blast Sequence Simulator

This component uses Konva.js for canvas rendering, so migration focuses on the controls:

```html
<div class="grid grid-cols-4 gap-6">
  <!-- Canvas Area (unchanged) -->
  <div class="col-span-3 bg-white rounded-lg shadow-md p-4">
    <div #simulatorCanvas></div>
  </div>

  <!-- Controls Panel -->
  <div class="space-y-4">
    <p-card header="Simulation Controls">
      <!-- Play Controls -->
      <div class="flex gap-2 mb-4">
        <p-button
          icon="pi pi-play"
          [disabled]="isPlaying"
          (onClick)="play()">
        </p-button>
        <p-button
          icon="pi pi-pause"
          [disabled]="!isPlaying"
          (onClick)="pause()">
        </p-button>
        <p-button
          icon="pi pi-refresh"
          (onClick)="reset()">
        </p-button>
      </div>

      <!-- Speed Control -->
      <div>
        <label class="block text-sm font-medium mb-2">Speed</label>
        <p-slider
          [(ngModel)]="simulationSpeed"
          [min]="0.5"
          [max]="3"
          [step]="0.5">
        </p-slider>
        <span class="text-sm text-gray-600">{{simulationSpeed}}x</span>
      </div>

      <!-- Timeline -->
      <div class="mt-4">
        <label class="block text-sm font-medium mb-2">Timeline</label>
        <p-slider
          [(ngModel)]="currentTime"
          [min]="0"
          [max]="totalTime"
          (onChange)="seekToTime($event.value)">
        </p-slider>
      </div>
    </p-card>

    <!-- View Options -->
    <p-card header="View Options">
      <div class="space-y-2">
        <p-checkbox
          [(ngModel)]="showGrid"
          label="Show Grid"
          [binary]="true">
        </p-checkbox>
        <p-checkbox
          [(ngModel)]="showLabels"
          label="Show Labels"
          [binary]="true">
        </p-checkbox>
        <p-checkbox
          [(ngModel)]="showConnections"
          label="Show Connections"
          [binary]="true">
        </p-checkbox>
      </div>
    </p-card>
  </div>
</div>
```

### Checklist

- [ ] Dashboard migrated
- [ ] Project management migrated
- [ ] Site dashboard migrated
- [ ] Workflow steps working
- [ ] CSV upload migrated
- [ ] File upload working
- [ ] Calculations display migrated
- [ ] Simulator controls migrated
- [ ] Proposal history migrated
- [ ] All workflows tested end-to-end

---

## Phase 7: Mechanical Engineer Module

**Goal**: Migrate Mechanical Engineer module (~25 components)

**Duration**: 3-4 hours

**Complexity**: High

### Components to Migrate

1. Mechanical Engineer Dashboard
2. Maintenance Jobs (List, Filters, Detail Panel, Status Update)
3. Maintenance Calendar (Calendar View, Timeline View)
4. Maintenance Analytics (Charts, Metrics)
5. Maintenance Settings
6. Notification Preferences

### Key Features

#### 7.1 Maintenance Calendar

```html
<p-tabView>
  <p-tabPanel header="Calendar View" leftIcon="pi pi-calendar">
    <p-fullCalendar
      [events]="maintenanceEvents"
      [options]="calendarOptions"
      (dateClick)="handleDateClick($event)"
      (eventClick)="handleEventClick($event)">
    </p-fullCalendar>
  </p-tabPanel>

  <p-tabPanel header="Timeline View" leftIcon="pi pi-list">
    <p-timeline [value]="maintenanceJobs">
      <ng-template pTemplate="content" let-job>
        <!-- Timeline content -->
      </ng-template>
    </p-timeline>
  </p-tabPanel>
</p-tabView>
```

**Note**: You'll need to install FullCalendar:
```bash
npm install @fullcalendar/core @fullcalendar/daygrid @fullcalendar/interaction
```

#### 7.2 Maintenance Analytics - Charts

PrimeNG integrates well with Chart.js (which you already have):

```html
<div class="grid grid-cols-1 md:grid-cols-2 gap-6">
  <!-- Service Compliance Chart -->
  <p-card header="Service Compliance">
    <p-chart
      type="doughnut"
      [data]="complianceData"
      [options]="chartOptions">
    </p-chart>
  </p-card>

  <!-- Usage Metrics Chart -->
  <p-card header="Usage Metrics">
    <p-chart
      type="bar"
      [data]="usageData"
      [options]="chartOptions">
    </p-chart>
  </p-card>

  <!-- MTBF Metrics -->
  <p-card header="Mean Time Between Failures">
    <p-chart
      type="line"
      [data]="mtbfData"
      [options]="chartOptions">
    </p-chart>
  </p-card>

  <!-- Parts Usage -->
  <p-card header="Parts Usage Summary">
    <p-chart
      type="pie"
      [data]="partsData"
      [options]="chartOptions">
    </p-chart>
  </p-card>
</div>
```

#### 7.3 Maintenance Jobs - Advanced Filtering

```html
<div class="flex flex-col gap-6">
  <!-- Filters Panel -->
  <p-panel header="Filters" [toggleable]="true">
    <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
      <!-- Status Filter -->
      <div>
        <label class="block text-sm font-medium mb-2">Status</label>
        <p-multiSelect
          [options]="statusOptions"
          [(ngModel)]="selectedStatuses"
          placeholder="Select Status"
          [filter]="false">
        </p-multiSelect>
      </div>

      <!-- Priority Filter -->
      <div>
        <label class="block text-sm font-medium mb-2">Priority</label>
        <p-dropdown
          [options]="priorityOptions"
          [(ngModel)]="selectedPriority"
          placeholder="All Priorities">
        </p-dropdown>
      </div>

      <!-- Date Range -->
      <div>
        <label class="block text-sm font-medium mb-2">Date Range</label>
        <p-calendar
          [(ngModel)]="dateRange"
          selectionMode="range"
          [readonlyInput]="true"
          placeholder="Select Date Range">
        </p-calendar>
      </div>

      <!-- Search -->
      <div>
        <label class="block text-sm font-medium mb-2">Search</label>
        <span class="p-input-icon-left w-full">
          <i class="pi pi-search"></i>
          <input
            pInputText
            type="text"
            [(ngModel)]="searchTerm"
            placeholder="Search jobs..."
            class="w-full">
        </span>
      </div>
    </div>

    <div class="flex justify-end gap-2 mt-4">
      <p-button
        label="Clear Filters"
        icon="pi pi-times"
        styleClass="p-button-outlined"
        (onClick)="clearFilters()">
      </p-button>
      <p-button
        label="Apply Filters"
        icon="pi pi-check"
        (onClick)="applyFilters()">
      </p-button>
    </div>
  </p-panel>

  <!-- Jobs DataTable -->
  <p-table
    [value]="filteredJobs"
    [paginator]="true"
    [rows]="10"
    [rowsPerPageOptions]="[10, 25, 50]"
    [loading]="loading"
    styleClass="p-datatable-striped">

    <ng-template pTemplate="header">
      <tr>
        <th pSortableColumn="jobId">Job ID <p-sortIcon field="jobId"></p-sortIcon></th>
        <th pSortableColumn="machine">Machine <p-sortIcon field="machine"></p-sortIcon></th>
        <th pSortableColumn="type">Type <p-sortIcon field="type"></p-sortIcon></th>
        <th pSortableColumn="priority">Priority <p-sortIcon field="priority"></p-sortIcon></th>
        <th pSortableColumn="status">Status <p-sortIcon field="status"></p-sortIcon></th>
        <th pSortableColumn="scheduledDate">Scheduled <p-sortIcon field="scheduledDate"></p-sortIcon></th>
        <th>Actions</th>
      </tr>
    </ng-template>

    <ng-template pTemplate="body" let-job>
      <tr>
        <td>{{job.jobId}}</td>
        <td>{{job.machine}}</td>
        <td>{{job.type}}</td>
        <td>
          <span [class]="getPriorityClass(job.priority)">
            {{job.priority}}
          </span>
        </td>
        <td>
          <span [class]="getStatusClass(job.status)">
            {{job.status}}
          </span>
        </td>
        <td>{{job.scheduledDate | date:'short'}}</td>
        <td>
          <p-button
            icon="pi pi-eye"
            styleClass="p-button-rounded p-button-text"
            (onClick)="viewJob(job)">
          </p-button>
          <p-button
            icon="pi pi-pencil"
            styleClass="p-button-rounded p-button-text"
            (onClick)="editJob(job)">
          </p-button>
        </td>
      </tr>
    </ng-template>

    <ng-template pTemplate="emptymessage">
      <tr>
        <td colspan="7" class="text-center py-8">
          <i class="pi pi-inbox text-4xl text-gray-400"></i>
          <p class="text-gray-600 mt-2">No maintenance jobs found</p>
        </td>
      </tr>
    </ng-template>
  </p-table>
</div>
```

#### 7.4 Job Detail Panel - Sidebar

```html
<p-sidebar
  [(visible)]="showJobDetail"
  position="right"
  [style]="{width: '500px'}"
  [modal]="false">

  <ng-template pTemplate="header">
    <h3 class="text-xl font-bold">Job Details</h3>
  </ng-template>

  <div *ngIf="selectedJob" class="space-y-6">
    <!-- Job Header -->
    <div class="border-b pb-4">
      <h4 class="text-lg font-semibold text-gray-800">{{selectedJob.title}}</h4>
      <p class="text-sm text-gray-600 mt-1">Job ID: {{selectedJob.jobId}}</p>
    </div>

    <!-- Job Info -->
    <div class="space-y-4">
      <div class="flex justify-between">
        <span class="text-gray-600">Machine:</span>
        <span class="font-medium">{{selectedJob.machine}}</span>
      </div>
      <div class="flex justify-between">
        <span class="text-gray-600">Type:</span>
        <span class="font-medium">{{selectedJob.type}}</span>
      </div>
      <div class="flex justify-between">
        <span class="text-gray-600">Priority:</span>
        <span [class]="getPriorityClass(selectedJob.priority)">
          {{selectedJob.priority}}
        </span>
      </div>
      <div class="flex justify-between">
        <span class="text-gray-600">Status:</span>
        <span [class]="getStatusClass(selectedJob.status)">
          {{selectedJob.status}}
        </span>
      </div>
    </div>

    <!-- Description -->
    <div>
      <label class="block text-sm font-medium text-gray-700 mb-2">Description</label>
      <p class="text-gray-600 bg-gray-50 p-3 rounded">{{selectedJob.description}}</p>
    </div>

    <!-- Timeline -->
    <div>
      <label class="block text-sm font-medium text-gray-700 mb-3">Activity Timeline</label>
      <p-timeline [value]="selectedJob.activities" layout="vertical">
        <ng-template pTemplate="content" let-activity>
          <div class="text-sm">
            <p class="font-medium">{{activity.title}}</p>
            <p class="text-gray-600 text-xs mt-1">{{activity.timestamp | date:'short'}}</p>
          </div>
        </ng-template>
      </p-timeline>
    </div>

    <!-- Actions -->
    <div class="flex gap-2">
      <p-button
        label="Update Status"
        icon="pi pi-refresh"
        styleClass="flex-1"
        (onClick)="updateStatus()">
      </p-button>
      <p-button
        label="Assign"
        icon="pi pi-user"
        styleClass="flex-1 p-button-outlined"
        (onClick)="assignJob()">
      </p-button>
    </div>
  </div>
</p-sidebar>
```

#### 7.5 Notification Preferences

```html
<p-card header="Notification Preferences">
  <div class="space-y-6">
    <!-- Email Notifications -->
    <div>
      <h4 class="text-lg font-semibold mb-3">Email Notifications</h4>
      <div class="space-y-3">
        <div class="flex items-center justify-between p-3 bg-gray-50 rounded">
          <div>
            <p class="font-medium">New Maintenance Job</p>
            <p class="text-sm text-gray-600">When a new job is assigned to you</p>
          </div>
          <p-inputSwitch [(ngModel)]="notifications.newJob"></p-inputSwitch>
        </div>

        <div class="flex items-center justify-between p-3 bg-gray-50 rounded">
          <div>
            <p class="font-medium">Job Due Soon</p>
            <p class="text-sm text-gray-600">24 hours before scheduled maintenance</p>
          </div>
          <p-inputSwitch [(ngModel)]="notifications.dueSoon"></p-inputSwitch>
        </div>

        <div class="flex items-center justify-between p-3 bg-gray-50 rounded">
          <div>
            <p class="font-medium">Job Overdue</p>
            <p class="text-sm text-gray-600">When a maintenance job is overdue</p>
          </div>
          <p-inputSwitch [(ngModel)]="notifications.overdue"></p-inputSwitch>
        </div>
      </div>
    </div>

    <!-- Push Notifications -->
    <div>
      <h4 class="text-lg font-semibold mb-3">Push Notifications</h4>
      <div class="flex items-center justify-between p-3 bg-gray-50 rounded">
        <div>
          <p class="font-medium">Enable Push Notifications</p>
          <p class="text-sm text-gray-600">Receive real-time notifications in browser</p>
        </div>
        <p-inputSwitch [(ngModel)]="notifications.push"></p-inputSwitch>
      </div>
    </div>

    <!-- Save Button -->
    <div class="flex justify-end">
      <p-button
        label="Save Preferences"
        icon="pi pi-check"
        (onClick)="savePreferences()">
      </p-button>
    </div>
  </div>
</p-card>
```

### Checklist

- [ ] Dashboard migrated
- [ ] Maintenance jobs list migrated
- [ ] Job filters working
- [ ] Job detail panel migrated
- [ ] Status update working
- [ ] Calendar view migrated
- [ ] Timeline view migrated
- [ ] Analytics charts migrated
- [ ] Settings page migrated
- [ ] Notification preferences working
- [ ] All features tested

---

## Phase 8: Machine Manager Module

**Goal**: Migrate Machine Manager module (~20 components)

**Duration**: 2-3 hours

**Complexity**: Medium

### Components to Migrate

1. Machine Manager Dashboard
2. Dashboard Home
3. Machine Details
4. Machine Assignments
5. Machine Inventory

### Key Features

Similar patterns to previous modules, focusing on:
- DataTables for inventory
- Cards for machines
- Forms for assignments
- Detail panels

### Checklist

- [ ] Dashboard migrated
- [ ] Machine inventory migrated
- [ ] Machine details migrated
- [ ] Assignment forms migrated
- [ ] All CRUD operations working

---

## Phase 9: Admin Module

**Goal**: Migrate Admin module (~25 components - most complex)

**Duration**: 4-5 hours

**Complexity**: High

### Components to Migrate

1. Admin Dashboard
2. User Management (List, Add, Edit, Assign, Details)
3. Project Management (List, Add, Edit, View, Sites)
4. Machine Management (Inventory, Add, Edit, Assignments)
5. Reports & Analytics

### Key Features

#### 9.1 User Management - Advanced DataTable

```html
<p-table
  #dt
  [value]="users"
  [rows]="10"
  [paginator]="true"
  [rowsPerPageOptions]="[10, 25, 50, 100]"
  [loading]="loading"
  [globalFilterFields]="['name', 'email', 'role', 'department']"
  [rowHover]="true"
  dataKey="id"
  currentPageReportTemplate="Showing {first} to {last} of {totalRecords} users"
  [showCurrentPageReport]="true">

  <ng-template pTemplate="caption">
    <div class="flex justify-between items-center">
      <div class="flex gap-2">
        <span class="p-input-icon-left">
          <i class="pi pi-search"></i>
          <input
            pInputText
            type="text"
            (input)="dt.filterGlobal($event.target.value, 'contains')"
            placeholder="Search users..." />
        </span>

        <p-dropdown
          [options]="roles"
          [(ngModel)]="selectedRole"
          placeholder="Filter by Role"
          [showClear]="true"
          (onChange)="filterByRole()">
        </p-dropdown>
      </div>

      <div class="flex gap-2">
        <p-button
          label="Add User"
          icon="pi pi-plus"
          (onClick)="showAddUserDialog()">
        </p-button>
        <p-button
          icon="pi pi-download"
          styleClass="p-button-outlined"
          (onClick)="exportUsers()">
        </p-button>
      </div>
    </div>
  </ng-template>

  <ng-template pTemplate="header">
    <tr>
      <th style="width: 3rem">
        <p-tableHeaderCheckbox></p-tableHeaderCheckbox>
      </th>
      <th pSortableColumn="name">Name <p-sortIcon field="name"></p-sortIcon></th>
      <th pSortableColumn="email">Email <p-sortIcon field="email"></p-sortIcon></th>
      <th pSortableColumn="role">Role <p-sortIcon field="role"></p-sortIcon></th>
      <th pSortableColumn="department">Department <p-sortIcon field="department"></p-sortIcon></th>
      <th pSortableColumn="status">Status <p-sortIcon field="status"></p-sortIcon></th>
      <th>Actions</th>
    </tr>
  </ng-template>

  <ng-template pTemplate="body" let-user>
    <tr>
      <td>
        <p-tableCheckbox [value]="user"></p-tableCheckbox>
      </td>
      <td>
        <div class="flex items-center gap-3">
          <p-avatar
            [label]="getInitials(user.name)"
            styleClass="mr-2"
            [style]="{'background-color': user.avatarColor, 'color': '#ffffff'}">
          </p-avatar>
          <span class="font-medium">{{user.name}}</span>
        </div>
      </td>
      <td>{{user.email}}</td>
      <td>
        <span class="px-2 py-1 rounded text-xs font-medium bg-blue-100 text-blue-800">
          {{user.role}}
        </span>
      </td>
      <td>{{user.department}}</td>
      <td>
        <span
          [ngClass]="{
            'bg-green-100 text-green-800': user.status === 'Active',
            'bg-red-100 text-red-800': user.status === 'Inactive',
            'bg-yellow-100 text-yellow-800': user.status === 'Pending'
          }"
          class="px-2 py-1 rounded text-xs font-medium">
          {{user.status}}
        </span>
      </td>
      <td>
        <p-button
          icon="pi pi-eye"
          styleClass="p-button-rounded p-button-text p-button-sm"
          pTooltip="View Details"
          tooltipPosition="top"
          (onClick)="viewUser(user)">
        </p-button>
        <p-button
          icon="pi pi-pencil"
          styleClass="p-button-rounded p-button-text p-button-sm"
          pTooltip="Edit User"
          tooltipPosition="top"
          (onClick)="editUser(user)">
        </p-button>
        <p-button
          icon="pi pi-user"
          styleClass="p-button-rounded p-button-text p-button-sm"
          pTooltip="Assign Projects"
          tooltipPosition="top"
          (onClick)="assignUser(user)">
        </p-button>
        <p-button
          icon="pi pi-trash"
          styleClass="p-button-rounded p-button-text p-button-danger p-button-sm"
          pTooltip="Delete User"
          tooltipPosition="top"
          (onClick)="deleteUser(user)">
        </p-button>
      </td>
    </tr>
  </ng-template>

  <ng-template pTemplate="emptymessage">
    <tr>
      <td colspan="7" class="text-center py-8">
        <i class="pi pi-users text-6xl text-gray-400"></i>
        <p class="text-gray-600 mt-4 text-lg">No users found</p>
        <p-button
          label="Add First User"
          icon="pi pi-plus"
          styleClass="mt-4"
          (onClick)="showAddUserDialog()">
        </p-button>
      </td>
    </tr>
  </ng-template>
</p-table>
```

#### 9.2 Add/Edit User Dialog

```html
<p-dialog
  [(visible)]="showUserDialog"
  [header]="editMode ? 'Edit User' : 'Add New User'"
  [modal]="true"
  [style]="{width: '600px'}"
  [draggable]="false"
  [resizable]="false">

  <form [formGroup]="userForm" class="space-y-4">
    <!-- Name -->
    <div class="flex flex-col gap-2">
      <label class="font-medium">Full Name *</label>
      <input
        pInputText
        formControlName="name"
        placeholder="Enter full name"
        [class.ng-invalid]="userForm.get('name')?.invalid && userForm.get('name')?.touched" />
      <small
        *ngIf="userForm.get('name')?.invalid && userForm.get('name')?.touched"
        class="text-red-500">
        Name is required
      </small>
    </div>

    <!-- Email -->
    <div class="flex flex-col gap-2">
      <label class="font-medium">Email *</label>
      <input
        pInputText
        type="email"
        formControlName="email"
        placeholder="user@example.com"
        [class.ng-invalid]="userForm.get('email')?.invalid && userForm.get('email')?.touched" />
      <small
        *ngIf="userForm.get('email')?.invalid && userForm.get('email')?.touched"
        class="text-red-500">
        Valid email is required
      </small>
    </div>

    <!-- Role -->
    <div class="flex flex-col gap-2">
      <label class="font-medium">Role *</label>
      <p-dropdown
        [options]="roles"
        formControlName="role"
        placeholder="Select a role"
        optionLabel="name"
        optionValue="value"
        [showClear]="false">
      </p-dropdown>
    </div>

    <!-- Department -->
    <div class="flex flex-col gap-2">
      <label class="font-medium">Department</label>
      <p-dropdown
        [options]="departments"
        formControlName="department"
        placeholder="Select department"
        [showClear]="true">
      </p-dropdown>
    </div>

    <!-- Phone -->
    <div class="flex flex-col gap-2">
      <label class="font-medium">Phone</label>
      <input
        pInputText
        formControlName="phone"
        placeholder="+1 (555) 123-4567" />
    </div>

    <!-- Status -->
    <div class="flex flex-col gap-2">
      <label class="font-medium">Status</label>
      <p-selectButton
        [options]="statusOptions"
        formControlName="status"
        optionLabel="label"
        optionValue="value">
      </p-selectButton>
    </div>
  </form>

  <ng-template pTemplate="footer">
    <div class="flex justify-end gap-2">
      <p-button
        label="Cancel"
        icon="pi pi-times"
        styleClass="p-button-text"
        (onClick)="closeUserDialog()">
      </p-button>
      <p-button
        [label]="editMode ? 'Update' : 'Create'"
        icon="pi pi-check"
        [disabled]="userForm.invalid"
        [loading]="saving"
        (onClick)="saveUser()">
      </p-button>
    </div>
  </ng-template>
</p-dialog>
```

#### 9.3 Project Management - Kanban Board

For a more visual project management, use PrimeNG with custom Tailwind styling:

```html
<div class="grid grid-cols-4 gap-6">
  <!-- Planning Column -->
  <div class="bg-gray-50 rounded-lg p-4">
    <div class="flex items-center justify-between mb-4">
      <h3 class="font-semibold text-gray-800">Planning</h3>
      <span class="bg-gray-200 text-gray-700 px-2 py-1 rounded-full text-xs">
        {{planningProjects.length}}
      </span>
    </div>

    <div class="space-y-3">
      <div
        *ngFor="let project of planningProjects"
        class="bg-white rounded-lg p-4 shadow-sm hover:shadow-md transition-shadow cursor-pointer">
        <h4 class="font-medium text-gray-800">{{project.name}}</h4>
        <p class="text-sm text-gray-600 mt-2">{{project.description}}</p>
        <div class="flex items-center justify-between mt-4">
          <span class="text-xs text-gray-500">{{project.sitesCount}} sites</span>
          <p-avatar
            [label]="getInitials(project.manager)"
            size="small"
            [style]="{'background-color': project.avatarColor}">
          </p-avatar>
        </div>
      </div>
    </div>
  </div>

  <!-- In Progress Column -->
  <div class="bg-blue-50 rounded-lg p-4">
    <!-- Similar structure -->
  </div>

  <!-- Testing Column -->
  <div class="bg-yellow-50 rounded-lg p-4">
    <!-- Similar structure -->
  </div>

  <!-- Completed Column -->
  <div class="bg-green-50 rounded-lg p-4">
    <!-- Similar structure -->
  </div>
</div>
```

#### 9.4 Admin Dashboard - Statistics

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
  <!-- Stat cards with animations -->
  <div
    *ngFor="let stat of dashboardStats"
    class="bg-white rounded-lg shadow-md p-6 border-l-4 hover:shadow-xl transition-all"
    [style.border-color]="stat.color">

    <div class="flex items-center justify-between">
      <div>
        <p class="text-sm text-gray-600 font-medium uppercase">{{stat.label}}</p>
        <p class="text-3xl font-bold text-gray-800 mt-2">
          <p-progressSpinner
            *ngIf="stat.loading"
            [style]="{width: '30px', height: '30px'}">
          </p-progressSpinner>
          <span *ngIf="!stat.loading">{{stat.value}}</span>
        </p>
        <p class="text-sm mt-2" [class]="stat.changeClass">
          <i [class]="stat.changeIcon"></i>
          {{stat.change}} from last month
        </p>
      </div>

      <div
        class="p-4 rounded-full"
        [style.background]="stat.color + '20'">
        <i [class]="'pi ' + stat.icon + ' text-2xl'" [style.color]="stat.color"></i>
      </div>
    </div>
  </div>
</div>

<!-- Recent Activity -->
<div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
  <p-card header="Recent Activity">
    <p-timeline [value]="recentActivity">
      <ng-template pTemplate="content" let-activity>
        <div class="flex items-start gap-3">
          <p-avatar
            [icon]="activity.icon"
            [style]="{'background-color': activity.color}">
          </p-avatar>
          <div>
            <p class="font-medium">{{activity.title}}</p>
            <p class="text-sm text-gray-600">{{activity.description}}</p>
            <p class="text-xs text-gray-500 mt-1">{{activity.timestamp | date:'short'}}</p>
          </div>
        </div>
      </ng-template>
    </p-timeline>
  </p-card>

  <p-card header="System Overview">
    <div class="space-y-4">
      <div *ngFor="let metric of systemMetrics">
        <div class="flex justify-between mb-2">
          <span class="text-sm font-medium">{{metric.label}}</span>
          <span class="text-sm text-gray-600">{{metric.value}}%</span>
        </div>
        <p-progressBar [value]="metric.value" [showValue]="false"></p-progressBar>
      </div>
    </div>
  </p-card>
</div>
```

### Checklist

- [ ] Admin dashboard migrated
- [ ] User management migrated
- [ ] User CRUD operations working
- [ ] Project management migrated
- [ ] Project CRUD operations working
- [ ] Machine management migrated
- [ ] Reports & analytics migrated
- [ ] All admin features tested
- [ ] Permissions working correctly

---

## Phase 10: Cleanup & Optimization

**Goal**: Remove Angular Material, optimize bundle, final testing

**Duration**: 2-3 hours

### Tasks

#### 10.1 Remove Material Dependencies

```bash
# Uninstall Angular Material
npm uninstall @angular/material @angular/cdk

# Uninstall Material Icons (if not using)
npm uninstall material-icons

# Clean up package-lock.json
npm install
```

#### 10.2 Update angular.json

Remove Material theme imports:

```json
{
  "styles": [
    // Remove these:
    // "@angular/material/prebuilt-themes/indigo-pink.css",
    // "node_modules/material-icons/iconfont/material-icons.css",

    // Keep these:
    "src/styles.scss",
    "node_modules/primeng/resources/themes/lara-light-blue/theme.css",
    "node_modules/primeng/resources/primeng.min.css",
    "node_modules/primeicons/primeicons.css"
  ]
}
```

#### 10.3 Remove Material Imports

Search and remove any remaining Material imports:

```bash
# Search for remaining Material imports
grep -r "@angular/material" src/
grep -r "mat-" src/

# Should return no results
```

#### 10.4 Optimize Tailwind CSS

Update `tailwind.config.js` for production:

```javascript
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    // your theme
  },
  plugins: [],
  // Add production optimizations
  purge: {
    enabled: true,
    content: ['./src/**/*.{html,ts}'],
  },
}
```

#### 10.5 Bundle Analysis

```bash
# Build with stats
npm run build -- --stats-json

# Analyze bundle
npx webpack-bundle-analyzer dist/clean-architecture.ui/stats.json
```

#### 10.6 Update app.config.ts

Remove MatSnackBar provider, keep only PrimeNG services:

```typescript
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, dataInterceptor])),
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    MessageService,
    ConfirmationService
  ]
};
```

#### 10.7 Performance Testing

Test application performance:

```typescript
// Check bundle sizes
// Before: ~3.5 MB (with Material)
// After: ~2.5 MB (with PrimeNG + Tailwind)
// Improvement: ~30% reduction

// Check load times
// Lighthouse audit
// First Contentful Paint
// Time to Interactive
```

#### 10.8 Browser Testing

Test in multiple browsers:
- Chrome
- Firefox
- Safari
- Edge

#### 10.9 Responsive Testing

Test on different screen sizes:
- Mobile (320px, 375px, 425px)
- Tablet (768px, 1024px)
- Desktop (1440px, 1920px)

#### 10.10 Create Migration Documentation

Document:
- Component mappings used
- Custom utilities created
- Known issues or workarounds
- Performance improvements
- Screenshots (before/after)

### Checklist

- [ ] Material dependencies removed
- [ ] angular.json cleaned up
- [ ] No Material imports remaining
- [ ] Tailwind optimized for production
- [ ] Bundle size analyzed
- [ ] Performance tested
- [ ] All browsers tested
- [ ] All screen sizes tested
- [ ] Documentation created
- [ ] Final sign-off from stakeholders

---

## Component Mapping Reference

### Quick Reference: Material → PrimeNG

| Material Component | PrimeNG Equivalent | Notes |
|-------------------|-------------------|-------|
| MatButton | p-button | More variants available |
| MatCard | p-card | Similar API |
| MatDialog | p-dialog | Declarative instead of service |
| MatSnackBar | p-toast | Better positioning options |
| MatTable | p-table | Much more powerful |
| MatPaginator | Built into p-table | No separate component needed |
| MatSort | Built into p-table | No separate component needed |
| MatFormField | Native input + pInputText | Simpler approach |
| MatInput | pInputText | Directive-based |
| MatSelect | p-dropdown | More features |
| MatCheckbox | p-checkbox | Similar API |
| MatRadio | p-radioButton | Similar API |
| MatSlideToggle | p-inputSwitch | Similar API |
| MatSlider | p-slider | More customizable |
| MatDatepicker | p-calendar | More features |
| MatAutocomplete | p-autoComplete | Better performance |
| MatChips | p-chips | Similar API |
| MatTooltip | pTooltip | Directive-based |
| MatMenu | p-menu | More powerful |
| MatSidenav | p-sidebar | More flexible |
| MatToolbar | Custom with Tailwind | More flexibility |
| MatTabs | p-tabView | Similar API |
| MatExpansionPanel | p-accordion | Similar concept |
| MatProgressBar | p-progressBar | Similar API |
| MatProgressSpinner | p-progressSpinner | Similar API |
| MatIcon | PrimeIcons | Different icon set |
| MatBadge | p-badge | Similar API |
| MatChip | p-chip | Similar API |
| MatDivider | Custom with Tailwind | Use border utilities |
| MatList | Custom with Tailwind | More flexibility |
| MatStepper | p-steps | Similar concept |

### Icons Migration

**Material Icons** → **PrimeIcons**

| Material Icon | PrimeIcon | Class |
|--------------|-----------|-------|
| home | home | pi pi-home |
| dashboard | th-large | pi pi-th-large |
| settings | cog | pi pi-cog |
| person | user | pi pi-user |
| delete | trash | pi pi-trash |
| edit | pencil | pi pi-pencil |
| add | plus | pi pi-plus |
| close | times | pi pi-times |
| check | check | pi pi-check |
| search | search | pi pi-search |
| filter | filter | pi pi-filter |
| calendar | calendar | pi pi-calendar |
| download | download | pi pi-download |
| upload | upload | pi pi-upload |
| info | info-circle | pi pi-info-circle |
| warning | exclamation-triangle | pi pi-exclamation-triangle |
| error | times-circle | pi pi-times-circle |

Full PrimeIcons list: https://primeng.org/icons

---

## Risk Assessment & Mitigation

### High-Risk Areas

#### 1. Dialog/Modal Usage Throughout App

**Risk**: MatDialog service is used extensively
**Impact**: High - affects many components
**Mitigation**:
- Migrate shared dialogs first (Phase 1)
- Create wrapper service for PrimeNG dialogs
- Test each dialog migration thoroughly

#### 2. Form Validation

**Risk**: Material form field error handling differs from PrimeNG
**Impact**: Medium - affects all forms
**Mitigation**:
- Create consistent validation utility
- Use Tailwind classes for error states
- Test all form validations

#### 3. Table Functionality

**Risk**: MatTable with MatPaginator/MatSort used extensively
**Impact**: Medium - affects data views
**Mitigation**:
- PrimeNG tables have built-in features
- May actually be easier than Material
- Test sorting, filtering, pagination

#### 4. Custom Styling Overrides

**Risk**: Existing Material theme overrides may break
**Impact**: Low-Medium - visual inconsistencies
**Mitigation**:
- Your design system uses CSS variables (good!)
- Map variables to Tailwind config
- Use PrimeNG theme customization

#### 5. Third-Party Component Integration

**Risk**: Konva.js, Chart.js, Three.js integration
**Impact**: Low - these are separate
**Mitigation**:
- These libraries don't depend on Material
- Should work without changes
- Test after migration

### Risk Matrix

| Risk | Probability | Impact | Priority | Mitigation Status |
|------|------------|--------|----------|------------------|
| Dialog Migration | High | High | P0 | Documented above |
| Form Validation | Medium | Medium | P1 | Documented above |
| Table Migration | Medium | Medium | P1 | Documented above |
| Styling Inconsistencies | Medium | Low | P2 | Documented above |
| Performance Issues | Low | Medium | P2 | Monitor during testing |
| Browser Compatibility | Low | High | P1 | Test in all browsers |
| Accessibility | Low | Medium | P2 | PrimeNG is WCAG compliant |

---

## Testing Strategy

### Unit Testing

Update component tests to use PrimeNG:

```typescript
// Before (Material)
import { MatDialogModule } from '@angular/material/dialog';

TestBed.configureTestingModule({
  imports: [MatDialogModule]
});

// After (PrimeNG)
import { DialogModule } from 'primeng/dialog';

TestBed.configureTestingModule({
  imports: [DialogModule]
});
```

### Integration Testing

Test key user flows:

1. **Auth Flow**
   - Login
   - Password reset
   - Session management

2. **Store Manager Flow**
   - View requests
   - Approve/reject requests
   - View inventory

3. **Blasting Engineer Flow**
   - Create project
   - Design pattern
   - Submit for approval

4. **Admin Flow**
   - User CRUD
   - Project CRUD
   - Reports

### Visual Regression Testing

Take screenshots before/after:

```bash
# Consider using tools like:
# - Percy (https://percy.io/)
# - Chromatic (https://www.chromatic.com/)
# - BackstopJS (https://github.com/garris/BackstopJS)
```

### Performance Testing

Monitor key metrics:

```typescript
// Key Performance Indicators (KPIs)
- First Contentful Paint (FCP): < 1.5s
- Time to Interactive (TTI): < 3.5s
- Total Bundle Size: < 2.5MB
- Main Thread Blocking: < 300ms
```

### Accessibility Testing

```bash
# Use tools like:
# - axe DevTools
# - WAVE
# - Lighthouse Accessibility Audit

# Target Score: 95+ on Lighthouse Accessibility
```

### Testing Checklist by Phase

**Phase 0-1**:
- [ ] Application builds
- [ ] No console errors
- [ ] Shared components work

**Phase 2-9** (Per Module):
- [ ] All components render
- [ ] All interactions work
- [ ] Forms validate correctly
- [ ] Dialogs open/close
- [ ] Data loads correctly
- [ ] CRUD operations work
- [ ] Navigation works
- [ ] No console errors
- [ ] No visual regressions

**Phase 10**:
- [ ] Full E2E test suite passes
- [ ] Performance benchmarks met
- [ ] Accessibility audit passes
- [ ] Browser compatibility verified
- [ ] Mobile responsive verified

---

## Rollback Plan

### Per-Phase Rollback

Each phase can be rolled back independently using git:

```bash
# Create branch before each phase
git checkout -b phase-1-shared-components

# Make changes...

# If issues arise, rollback
git checkout main
git branch -D phase-1-shared-components
```

### Component-Level Rollback

Both Material and PrimeNG can coexist:

```typescript
// Keep old Material component
@Component({
  selector: 'app-old-dialog',
  imports: [MatDialogModule]
})
export class OldDialogComponent { }

// New PrimeNG component
@Component({
  selector: 'app-new-dialog',
  imports: [DialogModule]
})
export class NewDialogComponent { }

// Toggle in parent component
useNewDialog = true; // Feature flag
```

### Emergency Rollback

If major issues in production:

```bash
# Revert to previous stable version
git revert <commit-hash>

# Or full rollback
git reset --hard <stable-commit>

# Redeploy
npm install
npm run build
# Deploy...
```

### Rollback Checklist

- [ ] Git branches for each phase
- [ ] Backup database before data changes
- [ ] Document rollback procedures
- [ ] Test rollback in staging
- [ ] Communication plan if rollback needed
- [ ] Post-rollback verification steps

---

## Timeline & Effort Estimates

### Detailed Time Breakdown

| Phase | Description | Duration | Complexity | Risk |
|-------|------------|----------|------------|------|
| Phase 0 | Setup & Config | 30-45 min | Low | Low |
| Phase 1 | Shared Components | 2-3 hours | Medium | High |
| Phase 2 | Authentication | 1-2 hours | Low | Low |
| Phase 3 | Store Manager | 2-3 hours | Medium | Medium |
| Phase 4 | Explosive Manager | 2-3 hours | Medium | Medium |
| Phase 5 | Operator | 2-3 hours | Medium | Medium |
| Phase 6 | Blasting Engineer | 3-4 hours | High | Medium |
| Phase 7 | Mechanical Engineer | 3-4 hours | High | Medium |
| Phase 8 | Machine Manager | 2-3 hours | Medium | Medium |
| Phase 9 | Admin | 4-5 hours | High | High |
| Phase 10 | Cleanup & Testing | 2-3 hours | Medium | Low |
| **Total** | **Complete Migration** | **24-35 hours** | **Medium-High** | **Medium** |

### Session Planning

#### Option A: Intensive (Full-Time)
- **Duration**: 1 week
- **Schedule**: 5-7 hours per day
- **Pros**: Fast completion, momentum maintained
- **Cons**: Intensive, requires dedicated time

**Week Schedule**:
- Monday: Phases 0-2 (Setup, Shared, Auth)
- Tuesday: Phases 3-4 (Store Manager, Explosive Manager)
- Wednesday: Phases 5-6 (Operator, Blasting Engineer)
- Thursday: Phases 7-8 (Mechanical Engineer, Machine Manager)
- Friday: Phases 9-10 (Admin, Cleanup)

#### Option B: Part-Time (Recommended)
- **Duration**: 3-4 weeks
- **Schedule**: 2-3 hours per day or full days on weekends
- **Pros**: Less intense, easier to fit into schedule
- **Cons**: Longer overall timeline

**Weekly Schedule**:
- **Week 1**: Phases 0-3 (Setup through Store Manager)
- **Week 2**: Phases 4-6 (Explosive through Blasting Engineer)
- **Week 3**: Phases 7-8 (Mechanical & Machine Manager)
- **Week 4**: Phases 9-10 (Admin & Cleanup)

#### Option C: Gradual (Low Risk)
- **Duration**: 6-8 weeks
- **Schedule**: 1-2 hours, 2-3 times per week
- **Pros**: Lowest risk, thorough testing between phases
- **Cons**: Long timeline, harder to maintain momentum

**Bi-Weekly Schedule**:
- **Weeks 1-2**: Phases 0-2
- **Weeks 3-4**: Phases 3-5
- **Weeks 5-6**: Phases 6-8
- **Weeks 7-8**: Phases 9-10

### Milestone Tracking

Create milestones to track progress:

```markdown
- [ ] Milestone 1: Foundation Ready (Phases 0-1) - Est: 3 hours
- [ ] Milestone 2: 3 Modules Migrated (Phases 2-4) - Est: 8 hours
- [ ] Milestone 3: 6 Modules Migrated (Phases 5-7) - Est: 18 hours
- [ ] Milestone 4: All Modules Migrated (Phases 8-9) - Est: 30 hours
- [ ] Milestone 5: Production Ready (Phase 10) - Est: 35 hours
```

---

## Resource Requirements

### Development Tools

**Required**:
- Node.js 18+
- Angular CLI 19+
- VS Code (or preferred IDE)
- Git

**Recommended Extensions**:
- Tailwind CSS IntelliSense
- Angular Language Service
- Prettier
- ESLint

### Documentation Resources

**Tailwind CSS**:
- Docs: https://tailwindcss.com/docs
- Playground: https://play.tailwindcss.com

**PrimeNG**:
- Docs: https://primeng.org
- Showcase: https://primeng.org/showcase
- GitHub: https://github.com/primefaces/primeng

**Migration Guides**:
- This document
- Component mapping reference (above)
- PrimeNG examples

### Team Resources

**Minimum Required**:
- 1 Frontend Developer (you + Claude Code)

**Recommended**:
- 1 Frontend Developer (primary)
- 1 QA Tester (part-time)
- 1 Designer (for UX review, optional)

### Budget Considerations

**Software Costs**:
- Tailwind CSS: FREE
- PrimeNG: FREE
- PrimeIcons: FREE
- Development tools: FREE

**Total Software Cost**: $0 🎉

**Time Cost**:
- Developer time: 24-35 hours
- QA time: 10-15 hours (optional)
- Designer review: 2-4 hours (optional)

---

## Success Criteria

### Technical Success Criteria

- [ ] All Angular Material dependencies removed
- [ ] All components migrated to PrimeNG + Tailwind
- [ ] No console errors in production build
- [ ] Bundle size reduced by at least 30%
- [ ] All unit tests passing
- [ ] All integration tests passing
- [ ] Lighthouse score > 90
- [ ] Accessibility score > 95
- [ ] No visual regressions

### Functional Success Criteria

- [ ] All user roles can complete their workflows
- [ ] All CRUD operations working
- [ ] All forms validating correctly
- [ ] All dialogs/modals working
- [ ] All tables sortable/filterable
- [ ] All navigation working
- [ ] All reports generating correctly
- [ ] File uploads working
- [ ] Data exports working

### User Experience Success Criteria

- [ ] Loading times improved
- [ ] Responsive design maintained/improved
- [ ] UI consistency across modules
- [ ] Accessibility improved
- [ ] No user-facing breaking changes
- [ ] Positive user feedback

### Business Success Criteria

- [ ] Zero downtime migration
- [ ] No data loss
- [ ] Project timeline met
- [ ] Budget maintained (should be $0!)
- [ ] Stakeholder approval

---

## Post-Migration

### Optimization Opportunities

Once migration is complete, consider:

1. **Theme Customization**: Create custom PrimeNG theme matching brand
2. **Component Library**: Build reusable component library
3. **Performance Tuning**: Further optimize bundle size
4. **Progressive Web App**: Add PWA features
5. **Dark Mode**: Add dark theme support (easier with Tailwind)
6. **Animation**: Add micro-interactions with Tailwind animations
7. **Mobile App**: Consider Ionic with your new components

### Maintenance Plan

**Monthly**:
- Update dependencies
- Review performance metrics
- Address any bugs

**Quarterly**:
- Major version updates
- Security audits
- User feedback review

**Annually**:
- Technology assessment
- Major feature additions
- Architecture review

---

## Conclusion

This phased migration plan provides a clear, low-risk path from Angular Material to Tailwind CSS + PrimeNG. By following this plan:

✅ **Minimize Risk**: Incremental approach with testing at each phase
✅ **Maintain Stability**: Application stays functional throughout
✅ **Improve Performance**: Smaller bundle, faster load times
✅ **Enhance Flexibility**: Greater design freedom with Tailwind
✅ **Zero Cost**: All tools and libraries are free
✅ **Clear Timeline**: 3-4 weeks for a complete, production-ready migration

### Next Steps

1. **Review this plan** with your team
2. **Set up Phase 0** (30 minutes)
3. **Start Phase 1** (2-3 hours)
4. **Get feedback** from users
5. **Continue incrementally** through remaining phases

### Questions or Issues?

If you encounter any problems during migration:

1. Check PrimeNG docs: https://primeng.org
2. Check Tailwind docs: https://tailwindcss.com
3. Refer to component mapping section above
4. Check PrimeNG GitHub issues
5. Ask Claude Code for help with specific components

---

**Good luck with your migration! 🚀**

You're making a great choice moving to Tailwind + PrimeNG. The flexibility and power of these tools will serve your application well for years to come.

---

## Appendix A: Package.json Changes

### Before Migration

```json
{
  "dependencies": {
    "@angular/animations": "^19.2.5",
    "@angular/cdk": "^19.2.5",
    "@angular/common": "^19.2.5",
    "@angular/material": "^19.2.5",
    "material-icons": "^1.13.14",
    // ... other deps
  }
}
```

### After Migration

```json
{
  "dependencies": {
    "@angular/animations": "^19.2.5",
    "@angular/common": "^19.2.5",
    "primeng": "^17.18.0",
    "primeicons": "^7.0.0",
    "primeflex": "^3.3.1",
    // ... other deps
  },
  "devDependencies": {
    "tailwindcss": "^3.4.0",
    "postcss": "^8.4.35",
    "autoprefixer": "^10.4.17",
    // ... other deps
  }
}
```

---

## Appendix B: File Structure Changes

No major structural changes needed. Your existing structure works well:

```
src/
├── app/
│   ├── components/         # No changes
│   ├── core/              # No changes
│   ├── shared/            # No changes
│   ├── models/            # No changes
│   └── services/          # Update notification service
├── styles/                # Add Tailwind
│   ├── styles.scss        # Add @tailwind directives
│   └── tailwind.scss      # New: custom Tailwind utilities
└── assets/                # No changes
```

---

## Appendix C: Useful Code Snippets

### Custom Tailwind Utilities

Add to `styles.scss`:

```scss
@layer utilities {
  // Custom gradient
  .gradient-primary {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  }

  // Glass morphism
  .glass {
    background: rgba(255, 255, 255, 0.7);
    backdrop-filter: blur(10px);
    border: 1px solid rgba(255, 255, 255, 0.3);
  }

  // Animated gradient
  .gradient-animated {
    background: linear-gradient(-45deg, #667eea, #764ba2, #f093fb, #4facfe);
    background-size: 400% 400%;
    animation: gradient 15s ease infinite;
  }

  @keyframes gradient {
    0% { background-position: 0% 50%; }
    50% { background-position: 100% 50%; }
    100% { background-position: 0% 50%; }
  }
}
```

### PrimeNG Global Customization

Create `primeng-overrides.scss`:

```scss
// Customize PrimeNG to match your design system
:root {
  --primary-color: #667eea;
  --primary-color-text: #ffffff;
  --surface-a: #ffffff;
  --surface-b: #f8f9fc;
  --surface-c: #e9ecef;
  --text-color: #2c3e50;
  --text-color-secondary: #6c757d;
  --border-radius: 8px;
}

// Override button styles
.p-button {
  border-radius: var(--border-radius);
  font-weight: 500;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-2px);
    box-shadow: 0 8px 30px rgba(102, 126, 234, 0.3);
  }
}

// Override table styles
.p-datatable {
  .p-datatable-thead > tr > th {
    background: var(--surface-b);
    color: var(--text-color);
    font-weight: 600;
    text-transform: uppercase;
    font-size: 0.875rem;
    letter-spacing: 0.5px;
  }

  .p-datatable-tbody > tr:hover {
    background: var(--surface-b);
  }
}
```

---

## Appendix D: Common Pitfalls & Solutions

### Pitfall 1: FormsModule Not Imported

**Error**: `Can't bind to 'ngModel'`

**Solution**:
```typescript
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule, ButtonModule],
  // ...
})
```

### Pitfall 2: PrimeNG Styles Not Loading

**Error**: Components render but look unstyled

**Solution**: Check `angular.json` includes PrimeNG styles:
```json
"styles": [
  "node_modules/primeng/resources/themes/lara-light-blue/theme.css",
  "node_modules/primeng/resources/primeng.min.css",
  "node_modules/primeicons/primeicons.css"
]
```

### Pitfall 3: Tailwind Not Purging Unused Styles

**Error**: Large CSS bundle

**Solution**: Ensure `content` array in `tailwind.config.js` includes all templates:
```javascript
content: [
  "./src/**/*.{html,ts}",
]
```

### Pitfall 4: Dialog Not Showing

**Error**: `p-dialog` doesn't appear

**Solution**: Ensure `[(visible)]` is bound to a boolean:
```typescript
showDialog = true; // Must be true to show
```

---

**End of Migration Plan**

**Document Version**: 1.0
**Last Updated**: October 13, 2025
**Next Review**: After Phase 5 completion
