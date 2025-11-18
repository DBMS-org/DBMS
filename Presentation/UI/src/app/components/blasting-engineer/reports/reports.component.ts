import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../core/services/site.service';
import { DrillHoleService, DrillHole } from '../../../core/services/drill-hole.service';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../core/services/explosive-calculations.service';
import { ExplosiveApprovalRequestService, ExplosiveApprovalRequest } from '../../../core/services/explosive-approval-request.service';
import { Project } from '../../../core/models/project.model';
import { forkJoin, of } from 'rxjs';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

interface ProjectReport {
  project: Project;
  sites: SiteReport[];
  totalDrillHoles: number;
  totalExplosiveUsage: {
    totalAnfo: number;
    totalEmulsion: number;
  };
}

interface SiteReport {
  site: ProjectSite;
  drillHoles: DrillHole[];
  explosiveCalculations: ExplosiveCalculationResultDto[];
  explosiveApprovals: ExplosiveApprovalRequest[];
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatExpansionModule,
    MatTableModule,
    MatChipsModule
  ],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);
  private authService = inject(AuthService);
  private projectService = inject(ProjectService);
  private siteService = inject(SiteService);
  private drillHoleService = inject(DrillHoleService);
  private explosiveCalculationsService = inject(ExplosiveCalculationsService);
  private explosiveApprovalService = inject(ExplosiveApprovalRequestService);

  // Component state
  isLoading = signal(false);
  isGenerating = signal(false);

  // Filter form
  filterForm!: FormGroup;

  // Report data
  projectReports = signal<ProjectReport[]>([]);
  selectedProjectId = signal<number | null>(null);

  // Table columns
  drillHoleColumns = ['name', 'easting', 'northing', 'elevation', 'depth', 'length', 'azimuth', 'dip'];
  explosiveColumns = ['calculationId', 'totalAnfo', 'totalEmulsion', 'averageDepth', 'numberOfFilledHoles', 'createdAt'];
  approvalColumns = ['status', 'blastingDate', 'blastTiming', 'expectedUsageDate', 'priority', 'approvalType'];

  // Current user
  currentUser = computed(() => this.authService.getCurrentUser());

  ngOnInit() {
    this.initializeFilterForm();
    this.loadData();
  }

  private initializeFilterForm() {
    this.filterForm = this.fb.group({
      project: [''],
      dateFrom: [null],
      dateTo: [null]
    });
  }

  private loadData() {
    this.isLoading.set(true);

    forkJoin({
      projects: this.projectService.getProjectsForCurrentUser(),
      allSites: this.siteService.getAllSites(),
      allDrillHoles: this.drillHoleService.getAllDrillHoles(),
      myApprovals: this.explosiveApprovalService.getMyExplosiveApprovalRequests()
    }).subscribe({
      next: ({ projects, allSites, allDrillHoles, myApprovals }) => {
        console.log('📊 Reports - Raw data loaded:', {
          projects: projects.length,
          allSites: allSites.length,
          allDrillHoles: allDrillHoles.length,
          myApprovals: myApprovals.length
        });
        console.log('📊 Reports - Drill holes data:', allDrillHoles);
        console.log('📊 Reports - Projects:', projects);
        console.log('📊 Reports - Sites:', allSites);

        const projectReports: ProjectReport[] = [];

        projects.forEach(project => {
          const projectSites = allSites.filter(site => site.projectId === project.id);
          console.log(`📊 Project ${project.name} (ID: ${project.id}) has ${projectSites.length} sites`);

          const siteReports: SiteReport[] = [];
          let projectTotalDrillHoles = 0;
          let projectTotalAnfo = 0;
          let projectTotalEmulsion = 0;

          projectSites.forEach(site => {
            const siteDrillHoles = allDrillHoles.filter(hole =>
              hole.projectId === project.id && hole.siteId === site.id
            );
            const siteApprovals = myApprovals.filter(approval =>
              approval.projectSiteId === site.id
            );

            console.log(`📊 Site ${site.name} (ID: ${site.id}) - Found ${siteDrillHoles.length} drill holes`);
            if (siteDrillHoles.length > 0) {
              console.log(`📊 Sample drill hole:`, siteDrillHoles[0]);
            }

            projectTotalDrillHoles += siteDrillHoles.length;

            // Load explosive calculations for this site
            this.explosiveCalculationsService.getByProjectAndSite(project.id, site.id).subscribe({
              next: (calculations) => {
                const latestCalc = calculations.length > 0 ? calculations[0] : null;
                if (latestCalc) {
                  projectTotalAnfo += latestCalc.totalAnfo;
                  projectTotalEmulsion += latestCalc.totalEmulsion;
                }

                siteReports.push({
                  site,
                  drillHoles: siteDrillHoles,
                  explosiveCalculations: calculations,
                  explosiveApprovals: siteApprovals
                });

                // Update the project report
                const existingProjectReport = projectReports.find(pr => pr.project.id === project.id);
                if (existingProjectReport) {
                  existingProjectReport.totalExplosiveUsage.totalAnfo = projectTotalAnfo;
                  existingProjectReport.totalExplosiveUsage.totalEmulsion = projectTotalEmulsion;
                }
              },
              error: () => {
                // No calculations for this site
                siteReports.push({
                  site,
                  drillHoles: siteDrillHoles,
                  explosiveCalculations: [],
                  explosiveApprovals: siteApprovals
                });
              }
            });
          });

          projectReports.push({
            project,
            sites: siteReports,
            totalDrillHoles: projectTotalDrillHoles,
            totalExplosiveUsage: {
              totalAnfo: projectTotalAnfo,
              totalEmulsion: projectTotalEmulsion
            }
          });
        });

        // Wait a bit for all explosive calculations to load
        setTimeout(() => {
          this.projectReports.set(projectReports);
          console.log('📊 Reports - Project reports:', projectReports);
          this.isLoading.set(false);
        }, 1000);
      },
      error: (error) => {
        console.error('Error loading data:', error);
        this.snackBar.open('Failed to load report data', 'Close', { duration: 3000 });
        this.isLoading.set(false);
      }
    });
  }

  selectProject(projectId: number) {
    this.selectedProjectId.set(this.selectedProjectId() === projectId ? null : projectId);
  }

  exportToPDF(projectReport?: ProjectReport) {
    this.isGenerating.set(true);

    setTimeout(() => {
      const doc = new jsPDF();
      const user = this.currentUser();
      const currentDate = new Date().toLocaleDateString();

      // Header
      doc.setFontSize(20);
      doc.text('Blasting Engineer Report', 14, 20);
      doc.setFontSize(10);
      doc.text(`Generated by: ${user?.name || 'Engineer'}`, 14, 30);
      doc.text(`Date: ${currentDate}`, 14, 36);
      doc.text(`Region: ${user?.region || 'N/A'}`, 14, 42);

      let yPosition = 50;

      const reportsToExport = projectReport ? [projectReport] : this.projectReports();

      reportsToExport.forEach((pr, index) => {
        if (index > 0) {
          doc.addPage();
          yPosition = 20;
        }

        // Project header
        doc.setFontSize(14);
        doc.text(`Project: ${pr.project.name}`, 14, yPosition);
        yPosition += 8;
        doc.setFontSize(10);
        doc.text(`Region: ${pr.project.region} | Total Drill Holes: ${pr.totalDrillHoles}`, 14, yPosition);
        yPosition += 10;

        pr.sites.forEach(siteReport => {
          // Site header
          doc.setFontSize(12);
          doc.text(`Site: ${siteReport.site.name}`, 14, yPosition);
          yPosition += 8;

          // Drill holes table
          if (siteReport.drillHoles.length > 0) {
            const drillHoleData = siteReport.drillHoles.map(hole => [
              hole.name || hole.id || 'N/A',
              hole.easting.toFixed(2),
              hole.northing.toFixed(2),
              hole.elevation.toFixed(2),
              hole.depth.toFixed(2),
              hole.length.toFixed(2)
            ]);

            autoTable(doc, {
              head: [['Hole Name', 'Easting', 'Northing', 'Elevation', 'Depth (m)', 'Length (m)']],
              body: drillHoleData,
              startY: yPosition,
              theme: 'striped',
              headStyles: { fillColor: [41, 128, 185] },
              margin: { left: 14 }
            });

            yPosition = (doc as any).lastAutoTable.finalY + 10;
          }

          // Explosive calculations table
          if (siteReport.explosiveCalculations.length > 0) {
            const explosiveData = siteReport.explosiveCalculations.map(calc => [
              calc.totalAnfo.toFixed(2),
              calc.totalEmulsion.toFixed(2),
              calc.averageDepth.toFixed(2),
              calc.numberOfFilledHoles.toString(),
              new Date(calc.createdAt).toLocaleDateString()
            ]);

            autoTable(doc, {
              head: [['Total ANFO (kg)', 'Total Emulsion (kg)', 'Avg Depth (m)', 'Filled Holes', 'Date']],
              body: explosiveData,
              startY: yPosition,
              theme: 'striped',
              headStyles: { fillColor: [231, 76, 60] },
              margin: { left: 14 }
            });

            yPosition = (doc as any).lastAutoTable.finalY + 10;
          }

          yPosition += 5;
        });
      });

      doc.save(`blasting-report-${currentDate}.pdf`);
      this.isGenerating.set(false);
      this.snackBar.open('Report exported successfully', 'Close', { duration: 3000 });
    }, 500);
  }

  exportToCSV(projectReport?: ProjectReport) {
    const reportsToExport = projectReport ? [projectReport] : this.projectReports();

    let csv = 'Project Name,Project Region,Site Name,Drill Hole,Easting,Northing,Elevation,Depth,Length,ANFO Total,Emulsion Total\n';

    reportsToExport.forEach(pr => {
      pr.sites.forEach(siteReport => {
        siteReport.drillHoles.forEach(hole => {
          const latestCalc = siteReport.explosiveCalculations[0];
          csv += `"${pr.project.name}","${pr.project.region}","${siteReport.site.name}","${hole.name || hole.id}",`;
          csv += `${hole.easting},${hole.northing},${hole.elevation},${hole.depth},${hole.length},`;
          csv += `${latestCalc ? latestCalc.totalAnfo : 0},${latestCalc ? latestCalc.totalEmulsion : 0}\n`;
        });
      });
    });

    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', `blasting-report-${new Date().toISOString().split('T')[0]}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    this.snackBar.open('CSV exported successfully', 'Close', { duration: 3000 });
  }

  refreshReports() {
    this.loadData();
    this.snackBar.open('Reports refreshed', 'Close', { duration: 2000 });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Approved': return 'primary';
      case 'Pending': return 'accent';
      case 'Rejected': return 'warn';
      default: return '';
    }
  }
}
