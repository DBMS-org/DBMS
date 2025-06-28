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
    { icon: 'dashboard', label: 'Dashboard', route: '/mechanical-engineer/dashboard' },
    { icon: 'build_circle', label: 'Maintenance Management', route: '/mechanical-engineer/maintenance' },
    { icon: 'schedule', label: 'Schedule Maintenance', route: '/mechanical-engineer/schedule' },
    { icon: 'inventory_2', label: 'Machine Inventory', route: '/mechanical-engineer/inventory' },
    { icon: 'notifications_active', label: 'Maintenance Alerts', route: '/mechanical-engineer/alerts' },
    { icon: 'analytics', label: 'Operational Status', route: '/mechanical-engineer/status' },
    { icon: 'assessment', label: 'Reports', route: '/mechanical-engineer/reports' },
    { icon: 'settings', label: 'Settings', route: '/mechanical-engineer/settings' }
  ];
}
