import { Routes } from '@angular/router';
import { StoreManagerLayoutComponent } from './shared/store-manager-layout/store-manager-layout.component';
import { DashboardComponent as StoreManagerDashboardComponent } from './dashboard/dashboard.component';
import { AddStockComponent } from './add-stock/add-stock.component';
import { RequestHistoryComponent } from './request-history/request-history.component';
import { BlastingEngineerRequestsComponent } from './blasting-engineer-requests/blasting-engineer-requests.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';

export const STORE_MANAGER_ROUTES: Routes = [
    {
        path: '',
        component: StoreManagerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: StoreManagerDashboardComponent },
            { path: 'profile', component: UserProfileComponent },
            { path: 'add-stock', component: AddStockComponent },
            { path: 'request-history', component: RequestHistoryComponent },
            { path: 'blasting-engineer-requests', component: BlastingEngineerRequestsComponent },
            { path: 'dispatch', component: StoreManagerDashboardComponent } // Placeholder for Dispatch Preparation
        ]
    }
];