import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AdminLayoutComponent } from './components/admin/shared/admin-layout/admin-layout.component';
import { DashboardComponent } from './components/admin/dashboard/dashboard.component';
import { UsersComponent } from './components/admin/users/users.component';
import { UserDetailsComponent } from './components/admin/users/user-details/user-details.component';
import { EditUserComponent } from './components/admin/users/edit-user/edit-user.component';
import { BlastingEngineerLayoutComponent } from './components/blasting-engineer/shared/blasting-engineer-layout/blasting-engineer-layout.component';
import { DashboardComponent as BlastingEngineerDashboardComponent } from './components/blasting-engineer/dashboard/dashboard.component';
import { CsvUploadComponent } from './components/blasting-engineer/csv-upload/csv-upload.component';
import { DrillingPatternCreatorComponent } from './components/blasting-engineer/drilling-pattern-creator/drilling-pattern-creator.component';
import { DrillVisualizationComponent } from './components/blasting-engineer/drill-visualization/drill-visualization.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'users', component: UsersComponent },
      { path: 'users/:id', component: UserDetailsComponent },
      { path: 'users/:id/edit', component: EditUserComponent }
    ]
  },
  {
    path: 'blasting-engineer',
    component: BlastingEngineerLayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: BlastingEngineerDashboardComponent },
      { path: 'csv-upload', component: CsvUploadComponent },
      { path: 'drilling-pattern', component: DrillingPatternCreatorComponent },
      { path: 'drill-visualization', component: DrillVisualizationComponent }
    ]
  },
  { path: '', redirectTo: '/admin/dashboard', pathMatch: 'full' }
];
