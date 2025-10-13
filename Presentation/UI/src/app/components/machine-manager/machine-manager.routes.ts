import { Routes } from '@angular/router';
import { MachineManagerLayoutComponent } from './shared/machine-manager-layout/machine-manager-layout.component';
import { MachineInventoryComponent } from './machine-inventory/machine-inventory.component';
import { AssignmentRequestsComponent } from './assignment-requests/assignment-requests.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { MachineManagerNotificationsComponent } from './notifications/notifications.component';

export const MACHINE_MANAGER_ROUTES: Routes = [
    {
        path: '',
        component: MachineManagerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            {
              path: 'dashboard',
              loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent)
            },
            { path: 'profile', component: UserProfileComponent },
            {
              path: 'machine-inventory',
              component: MachineInventoryComponent
            },
            {
              path: 'assignment-requests',
              component: AssignmentRequestsComponent
            },
            {
              path: 'accessories-inventory',
              loadComponent: () => import('../../features/accessories-inventory/accessories-inventory.component').then(m => m.AccessoriesInventoryComponent)
            },
            {
              path: 'maintenance-management',
              loadComponent: () => import('../../features/maintenance-management/maintenance-management.component').then(m => m.MaintenanceManagementComponent)
            },
            {
              path: 'notifications',
              component: MachineManagerNotificationsComponent
            }
        ]
    }
];