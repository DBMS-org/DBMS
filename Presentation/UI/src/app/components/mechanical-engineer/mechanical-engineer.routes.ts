import { Routes } from '@angular/router';
import { MechanicalEngineerLayoutComponent } from './shared/mechanical-engineer-layout/mechanical-engineer-layout.component';
import { DashboardComponent as MechanicalEngineerDashboardComponent } from './dashboard/dashboard.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { MaintenanceDashboardComponent } from './maintenance/maintenance-dashboard/maintenance-dashboard.component';
import { MaintenanceCalendarComponent } from './maintenance/maintenance-calendar/maintenance-calendar.component';
import { MaintenanceJobsComponent } from './maintenance/maintenance-jobs/maintenance-jobs.component';
import { MaintenanceAnalyticsComponent } from './maintenance/maintenance-analytics/maintenance-analytics.component';
import { MaintenanceSettingsComponent } from './maintenance/maintenance-settings/maintenance-settings.component';

export const MECHANICAL_ENGINEER_ROUTES: Routes = [
    {
        path: '',
        component: MechanicalEngineerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: MechanicalEngineerDashboardComponent },
            { 
                path: 'maintenance', 
                component: MaintenanceComponent,
                children: [
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: 'dashboard', component: MaintenanceDashboardComponent },
                    { path: 'calendar', component: MaintenanceCalendarComponent },
                    { path: 'jobs', component: MaintenanceJobsComponent },
                    { path: 'analytics', component: MaintenanceAnalyticsComponent },
                    { path: 'settings', component: MaintenanceSettingsComponent }
                ]
            },
            // Placeholder routes for future implementation
            { path: 'schedule', component: MechanicalEngineerDashboardComponent },
            { path: 'inventory', component: MechanicalEngineerDashboardComponent },
            { path: 'alerts', component: MechanicalEngineerDashboardComponent },
            { path: 'status', component: MechanicalEngineerDashboardComponent },
            { path: 'reports', component: MechanicalEngineerDashboardComponent },
            { path: 'settings', component: MechanicalEngineerDashboardComponent }
        ]
    }
]; 