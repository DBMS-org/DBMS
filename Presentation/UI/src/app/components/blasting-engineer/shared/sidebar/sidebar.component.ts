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
    { path: '/blasting-engineer/project-management', icon: 'work', label: 'Project Management' },
    { path: '/blasting-engineer/proposal-history', icon: 'history', label: 'Proposal History' },
    { path: '/blasting-engineer/notifications', icon: 'notifications', label: 'Notifications' }
  ];
}
