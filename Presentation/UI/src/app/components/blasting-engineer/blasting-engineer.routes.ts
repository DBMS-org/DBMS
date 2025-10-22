import { Routes } from '@angular/router';
import { BlastingEngineerLayoutComponent } from './shared/blasting-engineer-layout/blasting-engineer-layout.component';
import { DashboardComponent as BlastingEngineerDashboardComponent } from './dashboard/dashboard.component';
import { CsvUploadComponent } from './csv-upload/csv-upload.component';
import { DrillingPatternCreatorComponent } from './drilling-pattern-creator/drilling-pattern-creator.component';
import { BlastSequenceDesignerComponent } from './blast-sequence-designer/blast-sequence-designer.component';
import { BlastSequenceSimulatorComponent } from './blast-sequence-simulator/blast-sequence-simulator.component';
import { DrillVisualizationComponent } from './drill-visualization/drill-visualization.component';
import { ExplosiveCalculationsDisplayComponent } from './explosive-calculations-display/explosive-calculations-display.component';
import { ProjectManagementComponent as BlastingEngineerProjectManagementComponent } from './project-management/project-management.component';
import { ProjectSitesComponent as BlastingEngineerProjectSitesComponent } from './project-management/project-sites/project-sites.component';
import { AddSiteComponent } from './project-management/add-site/add-site.component';
import { SiteDashboardComponent } from './project-management/site-dashboard/site-dashboard.component';
import { ViewSequenceSimulatorComponent } from '../../shared/shared/components/view-sequence-simulator/view-sequence-simulator.component';
import { ProposalHistoryComponent } from './proposal-history/proposal-history.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';

// Route configuration for all blasting engineer pages
export const BLASTING_ENGINEER_ROUTES: Routes = [
    {
        path: '',
        component: BlastingEngineerLayoutComponent,
        children: [
            // Default route redirects to dashboard
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

            // Main navigation routes
            { path: 'dashboard', component: BlastingEngineerDashboardComponent },
            { path: 'profile', component: UserProfileComponent },
            { path: 'proposal-history', component: ProposalHistoryComponent },
            { path: 'csv-upload', component: CsvUploadComponent },
            { path: 'drilling-pattern', component: DrillingPatternCreatorComponent },
            { path: 'blast-sequence-designer', component: BlastSequenceDesignerComponent },
            { path: 'blast-sequence-simulator', component: BlastSequenceSimulatorComponent },
            { path: 'drill-visualization', component: DrillVisualizationComponent },
            { path: 'explosive-calculations', component: ExplosiveCalculationsDisplayComponent },

            // Project management routes
            { path: 'project-management', component: BlastingEngineerProjectManagementComponent },
            { path: 'project-management/:id/sites', component: BlastingEngineerProjectSitesComponent },
            { path: 'project-management/:id/sites/new', component: AddSiteComponent },

            // Site-specific routes with projectId and siteId parameters
            { path: 'project-management/:projectId/sites/:siteId/dashboard', component: SiteDashboardComponent },
            { path: 'project-management/:projectId/sites/:siteId/drill-visualization', component: DrillVisualizationComponent },
            { path: 'project-management/:projectId/sites/:siteId/pattern-creator', component: DrillingPatternCreatorComponent },
            { path: 'project-management/:projectId/sites/:siteId/sequence-designer', component: BlastSequenceDesignerComponent },
            { path: 'project-management/:projectId/sites/:siteId/simulator', component: BlastSequenceSimulatorComponent },
            { path: 'project-management/:projectId/sites/:siteId/explosive-calculations', component: ExplosiveCalculationsDisplayComponent },
            { path: 'project-management/:projectId/sites/:siteId/sequence-simulator', component: ViewSequenceSimulatorComponent }
        ]
    }
];