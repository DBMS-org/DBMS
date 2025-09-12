import { Routes } from '@angular/router';
import { ExplosiveManagerLayoutComponent } from './shared/explosive-manager-layout/explosive-manager-layout.component';
import { DashboardComponent as ExplosiveManagerDashboardComponent } from './dashboard/dashboard.component';
import { StoresComponent } from './stores/stores.component';
import { InventoryComponent } from './inventory/inventory.component';
import { AnfoInventoryComponent } from './inventory/anfo-inventory/anfo-inventory.component';
import { AnfoAddComponent } from './inventory/anfo-inventory/anfo-add/anfo-add.component';
import { AnfoEditComponent } from './inventory/anfo-inventory/anfo-edit/anfo-edit.component';
import { EmulsionInventoryComponent } from './inventory/emulsion-inventory/emulsion-inventory.component';
import { EmulsionAddComponent } from './inventory/emulsion-inventory/emulsion-add/emulsion-add.component';
import { EmulsionEditComponent } from './inventory/emulsion-inventory/emulsion-edit/emulsion-edit.component';
import { RequestsComponent } from './requests/requests.component';

export const EXPLOSIVE_MANAGER_ROUTES: Routes = [
    {
        path: '',
        component: ExplosiveManagerLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: ExplosiveManagerDashboardComponent },
            { path: 'stores', component: StoresComponent },
            { path: 'inventory', component: InventoryComponent },
            { path: 'inventory/anfo', component: AnfoInventoryComponent },
            { path: 'inventory/anfo/add', component: AnfoAddComponent },
            { path: 'inventory/anfo/edit/:id', component: AnfoEditComponent },
            { path: 'inventory/emulsion', component: EmulsionInventoryComponent },
            { path: 'inventory/emulsion/add', component: EmulsionAddComponent },
            { path: 'inventory/emulsion/edit/:id', component: EmulsionEditComponent },
            { path: 'requests', component: RequestsComponent }
        ]
    }
];