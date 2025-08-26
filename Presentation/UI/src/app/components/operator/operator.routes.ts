import { Routes } from '@angular/router';
import { OperatorLayoutComponent } from './shared/operator-layout/operator-layout.component';
import { OperatorDashboardComponent } from './dashboard/dashboard.component';
import { MyProjectComponent } from './my-project/my-project.component';
import { OperatorProjectSitesComponent } from './project-sites/project-sites.component';
import { OperatorPatternViewComponent } from './pattern-view/pattern-view.component';
import { MaintenanceReportsComponent } from './maintenance-reports/maintenance-reports.component';
import { MyMachinesComponent } from './my-machines/my-machines.component';

export const OPERATOR_ROUTES: Routes = [
    {
        path: '',
        component: OperatorLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: OperatorDashboardComponent },
            { path: 'my-project', component: MyProjectComponent },
            { path: 'my-project/sites', component: OperatorProjectSitesComponent },
            { path: 'my-project/sites/:siteId/pattern-view', component: OperatorPatternViewComponent },
            { path: 'maintenance-reports', component: MaintenanceReportsComponent },
            { path: 'my-machines', component: MyMachinesComponent }
        ]
    }
]; 