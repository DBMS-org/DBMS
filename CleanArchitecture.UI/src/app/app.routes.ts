import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AdminLayoutComponent } from './components/admin/shared/admin-layout/admin-layout.component';
import { DashboardComponent } from './components/admin/dashboard/dashboard.component';
import { UsersComponent } from './components/admin/users/users.component';
import { UserDetailsComponent } from './components/admin/users/user-details/user-details.component';
import { EditUserComponent } from './components/admin/users/edit-user/edit-user.component';
import { ProjectManagementComponent } from './components/admin/project-management/project-management.component';
import { ProjectDetailsComponent } from './components/admin/project-management/project-details/project-details.component';
import { AddProjectComponent } from './components/admin/project-management/add-project/add-project.component';
import { EditProjectComponent } from './components/admin/project-management/edit-project/edit-project.component';
import { ProjectSitesComponent } from './components/admin/project-management/project-sites/project-sites.component';
import { BlastingEngineerLayoutComponent } from './components/blasting-engineer/shared/blasting-engineer-layout/blasting-engineer-layout.component';
import { DashboardComponent as BlastingEngineerDashboardComponent } from './components/blasting-engineer/dashboard/dashboard.component';
import { CsvUploadComponent } from './components/blasting-engineer/csv-upload/csv-upload.component';
import { DrillingPatternCreatorComponent } from './components/blasting-engineer/drilling-pattern-creator/drilling-pattern-creator.component';
import { BlastSequenceDesignerComponent } from './components/blasting-engineer/blast-sequence-designer/blast-sequence-designer.component';
import { BlastSequenceSimulatorComponent } from './components/blasting-engineer/blast-sequence-simulator/blast-sequence-simulator.component';
import { DrillVisualizationComponent } from './components/blasting-engineer/drill-visualization/drill-visualization.component';
import { ProjectManagementComponent as BlastingEngineerProjectManagementComponent } from './components/blasting-engineer/project-management/project-management.component';
import { ProjectSitesComponent as BlastingEngineerProjectSitesComponent } from './components/blasting-engineer/project-management/project-sites/project-sites.component';
import { AddSiteComponent } from './components/blasting-engineer/project-management/add-site/add-site.component';

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
      { path: 'users/:id/edit', component: EditUserComponent },
      { path: 'project-management', component: ProjectManagementComponent },
      { path: 'project-management/new', component: AddProjectComponent },
      { path: 'project-management/:id', component: ProjectDetailsComponent },
      { path: 'project-management/:id/edit', component: EditProjectComponent },
      { path: 'project-management/:id/sites', component: ProjectSitesComponent }
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
      { path: 'blast-sequence-designer', component: BlastSequenceDesignerComponent },
      { path: 'blast-sequence-simulator', component: BlastSequenceSimulatorComponent },
      { path: 'drill-visualization', component: DrillVisualizationComponent },
      { path: 'project-management', component: BlastingEngineerProjectManagementComponent },
      { path: 'project-management/:id/sites', component: BlastingEngineerProjectSitesComponent },
      { path: 'project-management/:id/sites/new', component: AddSiteComponent }
    ]
  },
  { path: '', redirectTo: '/admin/dashboard', pathMatch: 'full' }
];
