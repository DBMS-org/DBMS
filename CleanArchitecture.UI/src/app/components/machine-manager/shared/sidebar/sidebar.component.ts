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
    { icon: 'inventory_2', label: 'Machine Inventory', route: '/machine-manager/machine-inventory' },
    { icon: 'assignment', label: 'Assignment Requests', route: '/machine-manager/assignment-requests' },
    { icon: 'build', label: 'Accessories Inventory', route: '/machine-manager/accessories-inventory' },
    { icon: 'engineering', label: 'Maintenance Management', route: '/machine-manager/maintenance-management' }
  ];
}
