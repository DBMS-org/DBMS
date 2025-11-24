import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './shared/admin-layout/admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UsersComponent } from './users/users.component';
import { UserDetailsComponent } from './users/user-details/user-details.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { ProjectManagementComponent } from './project-management/project-management.component';
import { ProjectDetailsComponent } from './project-management/project-details/project-details.component';
import { AddProjectComponent } from './project-management/add-project/add-project.component';
import { EditProjectComponent } from './project-management/edit-project/edit-project.component';
import { ProjectSitesComponent } from './project-management/project-sites/project-sites.component';
import { MachineInventoryComponent } from './machine-inventory/machine-inventory.component';
import { MachineAssignmentsComponent } from './machine-assignments/machine-assignments.component';
import { StoresComponent } from './stores/stores.component';
import { ViewSequenceSimulatorComponent } from '../../shared/shared/components/view-sequence-simulator/view-sequence-simulator.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';
import { ReportsComponent } from './reports/reports.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { MaintenanceManagementComponent } from '../../features/maintenance-management/maintenance-management.component';
import { JobDetailsComponent } from '../../features/maintenance-management/job-details/job-details.component';
import { ExecutiveDashboardComponent } from './executive-dashboard/executive-dashboard.component';
// REMOVED IN PHASE 1: import { FleetReportViewerComponent } from './fleet-report-viewer/fleet-report-viewer.component';
import { MaintenanceReportViewerComponent } from './maintenance-report-viewer/maintenance-report-viewer.component';
// REMOVED IN PHASE 1: import { InventoryReportViewerComponent } from './inventory-report-viewer/inventory-report-viewer.component';
// REMOVED IN PHASE 1: import { OperationalEfficiencyReportViewerComponent } from './operational-efficiency-report-viewer/operational-efficiency-report-viewer.component';
import { RegionalPerformanceReportViewerComponent } from './regional-performance-report-viewer/regional-performance-report-viewer.component';
// PHASE 3: Drilling Operations Report
import { DrillingOperationsReportViewerComponent } from './drilling-operations-report-viewer/drilling-operations-report-viewer.component';
// PHASE 4: Explosive Workflow Report
import { ExplosiveWorkflowReportViewerComponent } from './explosive-workflow-report-viewer/explosive-workflow-report-viewer.component';
// PHASE 5: User & Access Management Report
import { UserAccessReportViewerComponent } from './user-access-report-viewer/user-access-report-viewer.component';

// Admin module route configuration
export const ADMIN_ROUTES: Routes = [
    {
        path: '',
        component: AdminLayoutComponent,
        children: [
            // Default redirect to dashboard
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

            // Main navigation routes
            { path: 'dashboard', component: DashboardComponent },
            { path: 'profile', component: UserProfileComponent },

            // User management routes
            { path: 'users', component: UsersComponent },
            { path: 'users/:id', component: UserDetailsComponent },
            { path: 'users/:id/edit', component: EditUserComponent },

            // Project management routes
            { path: 'project-management', component: ProjectManagementComponent },
            { path: 'project-management/new', component: AddProjectComponent },
            { path: 'project-management/:id', component: ProjectDetailsComponent },
            { path: 'project-management/:id/edit', component: EditProjectComponent },
            { path: 'project-management/:id/sites', component: ProjectSitesComponent },
            { path: 'project-management/:projectId/sites/:siteId/sequence-simulator', component: ViewSequenceSimulatorComponent },

            // Machine and store management routes
            { path: 'machine-inventory', component: MachineInventoryComponent },
            { path: 'machine-assignments', component: MachineAssignmentsComponent },
            { path: 'stores', component: StoresComponent },

            // Reports routes
            { path: 'reports', component: ReportsComponent },
            { path: 'executive-dashboard', component: ExecutiveDashboardComponent },
            // REMOVED IN PHASE 1: { path: 'fleet-report', component: FleetReportViewerComponent },
            { path: 'maintenance-report', component: MaintenanceReportViewerComponent },
            // REMOVED IN PHASE 1: { path: 'inventory-report', component: InventoryReportViewerComponent },
            // REMOVED IN PHASE 1: { path: 'operational-efficiency-report', component: OperationalEfficiencyReportViewerComponent },
            { path: 'regional-performance-report', component: RegionalPerformanceReportViewerComponent },
            // PHASE 3: Drilling Operations Report
            { path: 'drilling-operations-report', component: DrillingOperationsReportViewerComponent },
            // PHASE 4: Explosive Workflow Report
            { path: 'explosive-workflow-report', component: ExplosiveWorkflowReportViewerComponent },
            // PHASE 5: User & Access Management Report
            { path: 'user-access-report', component: UserAccessReportViewerComponent },

            // Maintenance management routes
            { path: 'maintenance-management', component: MaintenanceManagementComponent },
            { path: 'maintenance-management/job/:id', component: JobDetailsComponent },

            // Notifications route
            { path: 'notifications', component: NotificationsComponent }
        ]
    }
];
