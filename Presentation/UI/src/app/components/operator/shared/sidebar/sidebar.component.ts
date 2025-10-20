import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// Navigation item structure
interface NavItem {
  icon: string;
  label: string;
  route: string;
}

@Component({
  selector: 'app-operator-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class OperatorSidebarComponent {
  // Control sidebar collapsed state
  @Input() isCollapsed = false;

  // Define navigation menu items
  navItems: NavItem[] = [
    { icon: 'dashboard', label: 'Dashboard', route: '/operator/dashboard' },
    { icon: 'assignment', label: 'My Assigned Project', route: '/operator/my-project' },
    { icon: 'precision_manufacturing', label: 'My Machines', route: '/operator/my-machines' },
    { icon: 'report_problem', label: 'Maintenance Reports', route: '/operator/maintenance-reports' },
    { icon: 'assessment', label: 'Work Reports', route: '/operator/reports' },
    { icon: 'notifications', label: 'Notifications', route: '/operator/notifications' }
  ];
}
