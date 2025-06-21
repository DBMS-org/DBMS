import { Routes } from '@angular/router';
import { 
  LoginComponent, 
  ForgotPasswordComponent, 
  VerifyResetCodeComponent, 
  ResetPasswordComponent 
} from './components/auth';
import { authGuard } from './core/guards/auth.guard';
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
import { MachineInventoryComponent } from './components/admin/machine-inventory/machine-inventory.component';
import { MachineAssignmentsComponent } from './components/admin/machine-assignments/machine-assignments.component';
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
import { SiteDashboardComponent } from './components/blasting-engineer/project-management/site-dashboard/site-dashboard.component';
import { MechanicalEngineerLayoutComponent } from './components/mechanical-engineer/shared/mechanical-engineer-layout/mechanical-engineer-layout.component';
import { DashboardComponent as MechanicalEngineerDashboardComponent } from './components/mechanical-engineer/dashboard/dashboard.component';
import { OperatorLayoutComponent } from './components/operator/shared/operator-layout/operator-layout.component';
import { OperatorDashboardComponent } from './components/operator/dashboard/dashboard.component';
import { MyProjectComponent } from './components/operator/my-project/my-project.component';
import { OperatorProjectSitesComponent } from './components/operator/project-sites/project-sites.component';
import { OperatorPatternViewComponent } from './components/operator/pattern-view/pattern-view.component';
import { ViewSequenceSimulatorComponent } from './components/admin/project-management/view-sequence-simulator/view-sequence-simulator.component';
import { MachineManagerComponent } from './components/machine-manager/machine-manager.component';
import { MachineInventoryComponent as MachineManagerInventoryComponent } from './components/machine-manager/machine-inventory/machine-inventory.component';
import { AssignmentRequestsComponent } from './components/machine-manager/assignment-requests/assignment-requests.component';
import { ExplosiveManagerLayoutComponent } from './components/explosive-manager/shared/explosive-manager-layout/explosive-manager-layout.component';
import { DashboardComponent as ExplosiveManagerDashboardComponent } from './components/explosive-manager/dashboard/dashboard.component';
import { StoresComponent } from './components/explosive-manager/stores/stores.component';
import { StoreManagerLayoutComponent } from './components/store-manager/shared/store-manager-layout/store-manager-layout.component';
import { DashboardComponent as StoreManagerDashboardComponent } from './components/store-manager/dashboard/dashboard.component';
import { AddStockComponent } from './components/store-manager/add-stock/add-stock.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'verify-reset-code', component: VerifyResetCodeComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { 
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [authGuard],
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
      { path: 'project-management/:id/sites', component: ProjectSitesComponent },
      { path: 'project-management/:projectId/sites/:siteId/sequence-simulator', component: ViewSequenceSimulatorComponent },
      { path: 'machine-inventory', component: MachineInventoryComponent },
      { path: 'machine-assignments', component: MachineAssignmentsComponent }
    ]
  },
  {
    path: 'blasting-engineer',
    component: BlastingEngineerLayoutComponent,
    canActivate: [authGuard],
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
      { path: 'project-management/:id/sites/new', component: AddSiteComponent },
      { path: 'project-management/:projectId/sites/:siteId/dashboard', component: SiteDashboardComponent },
      { path: 'project-management/:projectId/sites/:siteId/drill-visualization', component: DrillVisualizationComponent },
      { path: 'project-management/:projectId/sites/:siteId/pattern-creator', component: DrillingPatternCreatorComponent },
      { path: 'project-management/:projectId/sites/:siteId/sequence-designer', component: BlastSequenceDesignerComponent },
      { path: 'project-management/:projectId/sites/:siteId/simulator', component: BlastSequenceSimulatorComponent },
      { path: 'project-management/:projectId/sites/:siteId/sequence-simulator', component: ViewSequenceSimulatorComponent }
    ]
  },
  {
    path: 'mechanical-engineer',
    component: MechanicalEngineerLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: MechanicalEngineerDashboardComponent },
      { path: 'analysis', component: MechanicalEngineerDashboardComponent }, // Placeholder for now
      { path: 'design-tools', component: MechanicalEngineerDashboardComponent }, // Placeholder for now
      { path: 'reports', component: MechanicalEngineerDashboardComponent }, // Placeholder for now
      { path: 'settings', component: MechanicalEngineerDashboardComponent } // Placeholder for now
    ]
  },
  {
    path: 'machine-manager',
    loadComponent: () => import('./components/machine-manager/shared/machine-manager-layout/machine-manager-layout.component').then(m => m.MachineManagerLayoutComponent),
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () => import('./components/machine-manager/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'machine-inventory',
        component: MachineManagerInventoryComponent
      },
      {
        path: 'assignment-requests',
        component: AssignmentRequestsComponent
      }
    ]
  },
  {
    path: 'operator',
    component: OperatorLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: OperatorDashboardComponent },
      { path: 'my-project', component: MyProjectComponent },
      { path: 'my-project/sites', component: OperatorProjectSitesComponent },
      { path: 'my-project/sites/:siteId/pattern-view', component: OperatorPatternViewComponent }
    ]
  },
  {
    path: 'explosive-manager',
    component: ExplosiveManagerLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: ExplosiveManagerDashboardComponent },
      { path: 'stores', component: StoresComponent }
    ]
  },
  {
    path: 'store-manager',
    component: StoreManagerLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: StoreManagerDashboardComponent },
      { path: 'add-stock', component: AddStockComponent },
      { path: 'edit-stock', component: StoreManagerDashboardComponent }, // Placeholder for Edit Stock Request
      { path: 'delete-stock', component: StoreManagerDashboardComponent }, // Placeholder for Delete Stock Request
      { path: 'notifications', component: StoreManagerDashboardComponent }, // Placeholder for Notifications
      { path: 'dispatch', component: StoreManagerDashboardComponent } // Placeholder for Dispatch Preparation
    ]
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
