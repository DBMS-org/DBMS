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
    { icon: 'precision_manufacturing', label: 'Machine Operations', route: '/machine-manager/operations' },
    { icon: 'build_circle', label: 'Maintenance', route: '/machine-manager/maintenance' },
    { icon: 'inventory', label: 'Equipment', route: '/machine-manager/equipment' },
    { icon: 'schedule', label: 'Scheduling', route: '/machine-manager/scheduling' },
    { icon: 'assessment', label: 'Reports', route: '/machine-manager/reports' },
    { icon: 'settings', label: 'Settings', route: '/machine-manager/settings' }
  ];
}
