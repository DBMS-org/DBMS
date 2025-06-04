import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Input() isCollapsed = false;

  menuItems = [
    { path: '/blasting-engineer/dashboard', icon: 'dashboard', label: 'Dashboard' },
    { path: '/blasting-engineer/csv-upload', icon: 'upload_file', label: 'CSV Upload' },
    { path: '/blasting-engineer/drilling-pattern', icon: 'grid_on', label: 'Drilling Pattern' },
    { path: '/blasting-engineer/drill-visualization', icon: '3d_rotation', label: 'Drill Visualization' },
    { path: '/blasting-engineer/project-management', icon: 'work', label: 'Project Management' }
  ];
}
