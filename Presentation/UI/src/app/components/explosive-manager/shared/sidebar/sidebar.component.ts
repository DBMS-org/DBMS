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
    { icon: 'dashboard', label: 'Dashboard', route: '/explosive-manager/dashboard' },
    { icon: 'store', label: 'Stores', route: '/explosive-manager/stores' },
    { icon: 'inventory', label: 'Inventory', route: '/explosive-manager/inventory' },
    { icon: 'security', label: 'Safety Protocols', route: '/explosive-manager/safety' },
    { icon: 'work', label: 'Operations', route: '/explosive-manager/operations' },
    { icon: 'warning', label: 'Alerts', route: '/explosive-manager/alerts' },
    { icon: 'assessment', label: 'Reports', route: '/explosive-manager/reports' }
  ];
}
