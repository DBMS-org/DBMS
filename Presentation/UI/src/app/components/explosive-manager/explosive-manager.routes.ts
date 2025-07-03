import { Routes } from '@angular/router';
import { ExplosiveManagerLayoutComponent } from './shared/explosive-manager-layout/explosive-manager-layout.component';
import { DashboardComponent as ExplosiveManagerDashboardComponent } from './dashboard/dashboard.component';
import { StoresComponent } from './stores/stores.component';

export const EXPLOSIVE_MANAGER_ROUTES: Routes = [
    {
        path: '',
        component: ExplosiveManagerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: ExplosiveManagerDashboardComponent },
            { path: 'stores', component: StoresComponent }
        ]
    }
]; 