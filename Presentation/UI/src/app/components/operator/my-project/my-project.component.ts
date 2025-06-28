import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-operator-my-project',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-project.component.html',
  styleUrl: './my-project.component.scss'
})
export class MyProjectComponent implements OnInit {
  isLoading = true;
  error: string | null = null;
  project: Project | null = null;

  constructor(
    private projectService: ProjectService, 
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) {
      this.error = 'User not found';
      this.isLoading = false;
      return;
    }

    this.projectService.getProjectByOperator(currentUser.id).subscribe({
      next: (proj: Project | null) => {
        this.project = proj;
        this.isLoading = false;
      },
      error: (err: any) => {
        this.error = err.message;
        this.isLoading = false;
      }
    });
  }

  formatDate(date?: Date): string {
    return date ? new Date(date).toLocaleDateString() : '-';
  }

  navigateToSites(): void {
    this.router.navigate(['/operator/my-project/sites']);
  }
} 