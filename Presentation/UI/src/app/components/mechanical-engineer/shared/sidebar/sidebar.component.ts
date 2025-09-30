import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs/operators';

interface NavItem {
  icon: string;
  label: string;
  route: string;
  matchPattern?: string; // For matching nested routes
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
  
  private router = inject(Router);

  navItems: NavItem[] = [
    { 
      icon: 'dashboard', 
      label: 'Dashboard', 
      route: '/mechanical-engineer/dashboard' 
    },
    { 
      icon: 'assignment', 
      label: 'Maintenance Jobs', 
      route: '/mechanical-engineer/maintenance/jobs',
      matchPattern: '/mechanical-engineer/maintenance/jobs'
    },
    { 
      icon: 'analytics', 
      label: 'Maintenance Analytics', 
      route: '/mechanical-engineer/maintenance/analytics',
      matchPattern: '/mechanical-engineer/maintenance/analytics'
    },
    { 
      icon: 'settings', 
      label: 'Maintenance Settings', 
      route: '/mechanical-engineer/maintenance/settings',
      matchPattern: '/mechanical-engineer/maintenance/settings'
    }
  ];

  // Check if a nav item should be active based on current route
  isNavItemActive(item: NavItem): boolean {
    const currentUrl = this.router.url;
    
    if (item.matchPattern) {
      return currentUrl.startsWith(item.matchPattern);
    }
    
    return currentUrl === item.route;
  }
}
