import { Routes } from '@angular/router';
import { MachineManagerLayoutComponent } from './shared/machine-manager-layout/machine-manager-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MachineInventoryComponent } from './machine-inventory/machine-inventory.component';
import { AssignmentRequestsComponent } from './assignment-requests/assignment-requests.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { MachineManagerNotificationsComponent } from './notifications/notifications.component';
import { ReportsComponent } from './reports/reports.component';
import { AccessoriesInventoryComponent } from '../../features/accessories-inventory/accessories-inventory.component';
import { MaintenanceManagementComponent } from '../../features/maintenance-management/maintenance-management.component';

export const MACHINE_MANAGER_ROUTES: Routes = [
    {
        path: '',
        component: MachineManagerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'profile', component: UserProfileComponent },
            { path: 'machine-inventory', component: MachineInventoryComponent },
            { path: 'assignment-requests', component: AssignmentRequestsComponent },
            { path: 'accessories-inventory', component: AccessoriesInventoryComponent },
            { path: 'maintenance-management', component: MaintenanceManagementComponent },
            { path: 'reports', component: ReportsComponent },
            { path: 'notifications', component: MachineManagerNotificationsComponent }
        ]
    }
];