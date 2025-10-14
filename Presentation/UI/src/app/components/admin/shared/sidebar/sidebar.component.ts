import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface NavItem {
  icon: string;
  label: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {
  @Input() isCollapsed = false;

  navItems: NavItem[] = [
    { icon: 'dashboard', label: 'Dashboard', route: '/admin/dashboard' },
    { icon: 'people', label: 'User Management', route: '/admin/users' },
    { icon: 'work', label: 'Project Management', route: '/admin/project-management' },
    { icon: 'precision_manufacturing', label: 'Machine Inventory', route: '/admin/machine-inventory' },
    { icon: 'assignment', label: 'Machine Assignments', route: '/admin/machine-assignments' },
    { icon: 'store', label: 'Store Management', route: '/admin/stores' }
  ];
}
