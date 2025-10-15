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
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Input() isCollapsed = false;

  navItems: NavItem[] = [
    { icon: 'dashboard', label: 'Dashboard', route: '/machine-manager/dashboard' },
    { icon: 'precision_manufacturing', label: 'Machine Inventory', route: '/machine-manager/machine-inventory' },
    { icon: 'assignment', label: 'Assignment Requests', route: '/machine-manager/assignment-requests' },
    { icon: 'inventory', label: 'Accessories Inventory', route: '/machine-manager/accessories-inventory' },
    { icon: 'build', label: 'Maintenance Management', route: '/machine-manager/maintenance-management' },
    { icon: 'notifications', label: 'Notifications', route: '/machine-manager/notifications' }
  ];
}
