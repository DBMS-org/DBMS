import { Routes } from '@angular/router';
import { MechanicalEngineerLayoutComponent } from './shared/mechanical-engineer-layout/mechanical-engineer-layout.component';
import { DashboardComponent as MechanicalEngineerDashboardComponent } from './dashboard/dashboard.component';

export const MECHANICAL_ENGINEER_ROUTES: Routes = [
    {
        path: '',
        component: MechanicalEngineerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: MechanicalEngineerDashboardComponent },
            { path: 'analysis', component: MechanicalEngineerDashboardComponent }, // Placeholder for now
            { path: 'design-tools', component: MechanicalEngineerDashboardComponent }, // Placeholder for now
            { path: 'reports', component: MechanicalEngineerDashboardComponent }, // Placeholder for now
            { path: 'settings', component: MechanicalEngineerDashboardComponent } // Placeholder for now
        ]
    }
]; 