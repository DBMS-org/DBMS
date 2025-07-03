import { Routes } from '@angular/router';
import { OperatorLayoutComponent } from './shared/operator-layout/operator-layout.component';
import { OperatorDashboardComponent } from './dashboard/dashboard.component';
import { MyProjectComponent } from './my-project/my-project.component';
import { OperatorProjectSitesComponent } from './project-sites/project-sites.component';
import { OperatorPatternViewComponent } from './pattern-view/pattern-view.component';

export const OPERATOR_ROUTES: Routes = [
    {
        path: '',
        component: OperatorLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: OperatorDashboardComponent },
            { path: 'my-project', component: MyProjectComponent },
            { path: 'my-project/sites', component: OperatorProjectSitesComponent },
            { path: 'my-project/sites/:siteId/pattern-view', component: OperatorPatternViewComponent }
        ]
    }
]; 