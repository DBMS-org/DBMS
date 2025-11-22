import { Routes } from '@angular/router';
import { MechanicalEngineerLayoutComponent } from './shared/mechanical-engineer-layout/mechanical-engineer-layout.component';
import { DashboardHomeComponent } from './dashboard/dashboard-home/dashboard-home.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { MaintenanceDashboardComponent } from './maintenance/maintenance-dashboard/maintenance-dashboard.component';
import { MaintenanceJobsComponent } from './maintenance/maintenance-jobs/maintenance-jobs.component';
import { MaintenanceHistoryComponent } from './maintenance/maintenance-history/maintenance-history.component';
import { MaintenanceAnalyticsComponent } from './maintenance/maintenance-analytics/maintenance-analytics.component';
import { MaintenanceSettingsComponent } from './maintenance/maintenance-settings/maintenance-settings.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { MaintenanceNotificationsComponent } from './maintenance/maintenance-notifications/maintenance-notifications.component';
import { ReportsComponent } from './reports/reports.component';
import { MachineUsageComponent } from './maintenance/machine-usage/machine-usage.component';

export const MECHANICAL_ENGINEER_ROUTES: Routes = [
    {
        path: '',
        component: MechanicalEngineerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardHomeComponent },
            { path: 'profile', component: UserProfileComponent },
            {
                path: 'maintenance',
                component: MaintenanceComponent,
                children: [
                    { path: '', redirectTo: 'jobs', pathMatch: 'full' },
                    { path: 'jobs', component: MaintenanceJobsComponent },
                    { path: 'history', component: MaintenanceHistoryComponent },
                    { path: 'machine-usage/:machineId', component: MachineUsageComponent }
                ]
            },
            { path: 'notifications', component: MaintenanceNotificationsComponent },
            { path: 'reports', component: ReportsComponent }
        ]
    }
];