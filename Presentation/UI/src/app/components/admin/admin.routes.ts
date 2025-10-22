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

export const ADMIN_ROUTES: Routes = [
    {
        path: '',
        component: AdminLayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'profile', component: UserProfileComponent },
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
            { path: 'machine-assignments', component: MachineAssignmentsComponent },
            { path: 'stores', component: StoresComponent }
        ]
    }
];