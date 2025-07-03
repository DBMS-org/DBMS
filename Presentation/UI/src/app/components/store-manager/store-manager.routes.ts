import { Routes } from '@angular/router';
import { StoreManagerLayoutComponent } from './shared/store-manager-layout/store-manager-layout.component';
import { DashboardComponent as StoreManagerDashboardComponent } from './dashboard/dashboard.component';
import { AddStockComponent } from './add-stock/add-stock.component';

export const STORE_MANAGER_ROUTES: Routes = [
    {
        path: '',
        component: StoreManagerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: StoreManagerDashboardComponent },
            { path: 'add-stock', component: AddStockComponent },
            { path: 'edit-stock', component: StoreManagerDashboardComponent }, // Placeholder for Edit Stock Request
            { path: 'delete-stock', component: StoreManagerDashboardComponent }, // Placeholder for Delete Stock Request
            { path: 'notifications', component: StoreManagerDashboardComponent }, // Placeholder for Notifications
            { path: 'dispatch', component: StoreManagerDashboardComponent } // Placeholder for Dispatch Preparation
        ]
    }
]; 